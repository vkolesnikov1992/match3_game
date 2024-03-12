using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Factory.GameFactory.Interfaces;
using Infrastructure.MonoBehaviour.View.Core;
using Infrastructure.Services.CameraService.Interfaces;
using Infrastructure.Services.DataProvider.Interfaces;
using Infrastructure.Services.InputService.Interfaces;
using Infrastructure.Services.SaveLoadService.Interfaces;
using Infrastructure.Systems.Core.Components;
using UnityEngine;
using Zenject;

namespace Infrastructure.Systems.Core
{
    public class Match3System : IInitializable
    {
        private Cell[,] _cellsGrid;
        private readonly IGameFactory _factory;
        private readonly IInputService _inputService;
        private readonly IDataProvider _dataProvider;
        private readonly ICameraService _cameraService;
        private readonly ISaveLoadService _saveLoadService;

        private CancellationTokenSource _cancellationToken;
        public event Action<int> DestroyedCubeCount;
        private int _destroyedCubeCount;

        public Match3System(IGameFactory factory, IInputService inputService, IDataProvider dataProvider,
            ICameraService cameraService, ISaveLoadService saveLoadService)
        {
            _factory = factory;
            _inputService = inputService;
            _dataProvider = dataProvider;
            _cameraService = cameraService;
            _saveLoadService = saveLoadService;
        }
        
        public void Initialize()
        {
            _cancellationToken = new CancellationTokenSource();
            _inputService.OnSwipeDetected += (startPosition, moveDirection) =>
            {
                UniTask.RunOnThreadPool(async () =>
                {
                    await SwapCells(startPosition, moveDirection);
                }, cancellationToken: _cancellationToken.Token);
            };
        }

        public void CreateLevel(int[,] level)
        {
            _destroyedCubeCount = 0;
            
            _cancellationToken = new CancellationTokenSource();
            
            int numRows = level.GetLength(0);
            int numColumns = level.GetLength(1);

            int dropSpeed = _dataProvider.ConfigsDataContainer.GameSettings.DropSpeed;
            int swapSpeed = _dataProvider.ConfigsDataContainer.GameSettings.SwapSpeed;

            _cellsGrid = new Cell[numColumns, numRows];

            for (int row = numRows - 1; row >= 0; row--)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    int reversedRow = numRows - 1 - row;

                    CubeView cubeView = null;
                    int cubeType = level[row, column];
                    if (cubeType != 0)
                    {
                        cubeView = _factory.CreateCube(column, reversedRow, cubeType);
                    }

                    Cube cube = new Cube(cubeType, cubeView, dropSpeed, swapSpeed);
                    Cell cell = new Cell(column, reversedRow, cube);
                    _cellsGrid[column, reversedRow] = cell;
                }
            }

            _cameraService.CenterCameraOnGrid(_cellsGrid);
        }

        private async UniTask SwapCells(Vector2Int startPosition, Vector2Int moveDirection)
        {
            if (!IsValidMove(startPosition, moveDirection))
            {
                return;
            }

            Cell firstCell = _cellsGrid[startPosition.x, startPosition.y];
            Cell secondCell = _cellsGrid[startPosition.x + moveDirection.x, startPosition.y + moveDirection.y];


            if (!IsUpMoveValid(secondCell, moveDirection))
            {
                return;
            }

            await UniTask.SwitchToMainThread();

            UniTask firstTask = firstCell.Cube.ReplaceCell(secondCell);
            UniTask secondTask = secondCell.Cube.ReplaceCell(firstCell);

            await UniTask.WhenAll(firstTask, secondTask).AttachExternalCancellation(_cancellationToken.Token);
            
            await DropBlocks();
            await GridNormalize();
        }

        private bool IsValidCellPosition(Vector2Int position)
        {
            int numRows = _cellsGrid.GetLength(0);
            int numColumns = _cellsGrid.GetLength(1);

            bool isValidPosition = position.x >= 0 && position.x < numRows &&
                                   position.y >= 0 && position.y < numColumns;

            return isValidPosition;
        }

        private bool IsValidMove(Vector2Int startPosition, Vector2Int moveDirection)
        {
            Vector2Int finishPosition = startPosition + moveDirection;

            return IsValidCellPosition(startPosition) && _cellsGrid[startPosition.x, startPosition.y].Cube.IsHasView &&
                   IsValidCellPosition(finishPosition);
        }

        private bool IsUpMoveValid(Cell secondCell, Vector2Int moveDirection)
        {
            return secondCell.Cube.IsHasView || moveDirection != Vector2Int.up;
        }

        private async UniTask GridNormalize()
        {
            FindMatches();
            await DestroyMatches();
            await DropBlocks();

            if (FindMatches())
            {
                await GridNormalize();
            }
            
            SaveProgress();
        }

        private bool FindMatches()
        {
            int numRows = _cellsGrid.GetLength(0);
            int numColumns = _cellsGrid.GetLength(1);

            bool isHorizontalMatched = false;
            bool isVerticalMatched = false;

            for (int y = 0; y < numColumns; y++)
            {
                for (int x = 0; x < numRows; x++)
                {
                    Cell cell = _cellsGrid[x, y];

                    if (cell.Cube.CubeType == 0)
                    {
                        continue;
                    }

                    bool isHorizontalMatchFounded = CheckHorizontalMatches(cell, x, y);
                    bool isVerticalMatchFounded = CheckVerticalMatches(cell, x, y);

                    if (!isHorizontalMatched && isHorizontalMatchFounded)
                    {
                        isHorizontalMatched = true;
                    }

                    if (!isVerticalMatched && isVerticalMatchFounded)
                    {
                        isVerticalMatched = true;
                    }
                }
            }

            return isHorizontalMatched || isVerticalMatched;
        }

        private bool CheckHorizontalMatches(Cell cell, int x, int y)
        {
            int numColumns = _cellsGrid.GetLength(0);

            if (x <= 0 || x >= numColumns - 1)
            {
                return false;
            }

            bool isMatched = false;
            Cell rightNeighbor = _cellsGrid[x + 1, y];

            if (rightNeighbor.Cube.IsHasView && cell.Cube.CubeType == rightNeighbor.Cube.CubeType)
            {
                Cell leftNeighbor = _cellsGrid[x - 1, y];

                if (leftNeighbor.Cube.IsHasView && cell.Cube.CubeType == leftNeighbor.Cube.CubeType)
                {
                    cell.Cube.IsMatched = leftNeighbor.Cube.IsMatched = rightNeighbor.Cube.IsMatched = true;

                    CheckVerticalNeighbors(rightNeighbor);
                    CheckVerticalNeighbors(leftNeighbor);
                    CheckVerticalNeighbors(cell);

                    isMatched = true;
                }
            }

            return isMatched;
        }

        private void CheckVerticalNeighbors(Cell cell)
        {
            if (cell.Y + 1 < _cellsGrid.GetLength(1))
            {
                Cube topNeighbor = _cellsGrid[cell.X, cell.Y + 1]?.Cube;
                if (topNeighbor != null && topNeighbor.CubeType == cell.Cube.CubeType)
                {
                    topNeighbor.IsMatched = true;
                }
            }

            if (cell.Y - 1 >= 0)
            {
                Cube bottomNeighbor = _cellsGrid[cell.X, cell.Y - 1]?.Cube;
                if (bottomNeighbor != null && bottomNeighbor.CubeType == cell.Cube.CubeType)
                {
                    bottomNeighbor.IsMatched = true;
                }
            }
        }

        private bool CheckVerticalMatches(Cell cell, int x, int y)
        {
            int numCols = _cellsGrid.GetLength(1);

            if (y <= 0 || y >= numCols - 1)
            {
                return false;
            }

            bool isMatched = false;
            Cell topNeighbor = _cellsGrid[x, y + 1];
            Cell bottomNeighbor = _cellsGrid[x, y - 1];

            if (topNeighbor != null && bottomNeighbor != null &&
                topNeighbor.Cube.IsHasView && cell.Cube.CubeType == topNeighbor.Cube.CubeType &&
                bottomNeighbor.Cube.IsHasView && cell.Cube.CubeType == bottomNeighbor.Cube.CubeType)
            {
                cell.Cube.IsMatched = topNeighbor.Cube.IsMatched = bottomNeighbor.Cube.IsMatched = true;

                CheckHorizontalNeighbors(topNeighbor);
                CheckHorizontalNeighbors(bottomNeighbor);
                CheckHorizontalNeighbors(cell);

                isMatched = true;
            }

            return isMatched;
        }

        private void CheckHorizontalNeighbors(Cell cell)
        {
            if (cell.X + 1 < _cellsGrid.GetLength(0))
            {
                Cube rightNeighbor = _cellsGrid[cell.X + 1, cell.Y]?.Cube;
                if (rightNeighbor != null && rightNeighbor.CubeType == cell.Cube.CubeType)
                {
                    rightNeighbor.IsMatched = true;
                }
            }

            if (cell.X - 1 >= 0)
            {
                Cube leftNeighbor = _cellsGrid[cell.X - 1, cell.Y]?.Cube;
                if (leftNeighbor != null && leftNeighbor.CubeType == cell.Cube.CubeType)
                {
                    leftNeighbor.IsMatched = true;
                }
            }
        }

        private async UniTask DestroyMatches()
        {
            int numRows = _cellsGrid.GetLength(0);
            int numColumns = _cellsGrid.GetLength(1);
            List<UniTask> animationTasks = new List<UniTask>();

            for (int y = 0; y < numColumns; y++)
            {
                for (int x = 0; x < numRows; x++)
                {
                    Cell cell = _cellsGrid[x, y];

                    if (cell.Cube.CubeType == 0)
                    {
                        continue;
                    }

                    if (cell.Cube.IsHasView && cell.Cube.IsMatched)
                    {
                        animationTasks.Add(cell.Cube.CubeView.PlayDestroyAnimation());
                        cell.Cube.CubeType = 0;
                        cell.Cube.IsMatched = false;

                        _destroyedCubeCount++;
                    }
                }
            }

            await UniTask.WhenAll(animationTasks).AttachExternalCancellation(_cancellationToken.Token);

            DestroyedCubeCount?.Invoke(_destroyedCubeCount);
        }

        private async UniTask DropBlocks()
        {
            int numRows = _cellsGrid.GetLength(1);
            int numColumns = _cellsGrid.GetLength(0);
            List<UniTask> animationTasks = new List<UniTask>();

            for (int x = 0; x < numColumns; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    if (_cellsGrid[x, y].Cube.CubeType != 0)
                    {
                        int emptyCellIndex = GetLowestEmptyCellIndexNew(x, y);

                        if (emptyCellIndex != -1)
                        {
                            int cellsCount = y - emptyCellIndex;

                            Vector2 newPosition = new Vector2(x, emptyCellIndex);

                            if (!_cellsGrid[x, y].Cube.IsHasView)
                            {
                                continue;
                            }

                            _cellsGrid[x, emptyCellIndex].Cube.CubeView = _cellsGrid[x, y].Cube.CubeView;
                            _cellsGrid[x, emptyCellIndex].Cube.CubeType = _cellsGrid[x, y].Cube.CubeType;

                            _cellsGrid[x, y].Cube.CubeView = null;
                            _cellsGrid[x, y].Cube.CubeType = 0;

                            animationTasks.Add(_cellsGrid[x, emptyCellIndex].Cube.Drop(newPosition, cellsCount));
                        }
                    }
                }
            }

            await UniTask.WhenAll(animationTasks).AttachExternalCancellation(_cancellationToken.Token);
        }

        private int GetLowestEmptyCellIndexNew(int x, int startY)
        {
            int lowestEmptyCellIndex = -1;

            for (int y = startY - 1; y >= 0; y--)
            {
                if (_cellsGrid[x, y].Cube.CubeType == 0)
                {
                    bool hasEmptyCellsBelow = false;
                    for (int k = y - 1; k >= 0; k--)
                    {
                        if (_cellsGrid[x, k].Cube.CubeType == 0)
                        {
                            hasEmptyCellsBelow = true;
                            break;
                        }
                    }

                    if (!hasEmptyCellsBelow)
                    {
                        lowestEmptyCellIndex = y;
                    }
                }
            }

            return lowestEmptyCellIndex;
        }


        public void ClearLevel()
        {
            _cancellationToken.Cancel();
            
            int numRows = _cellsGrid.GetLength(0);
            int numColumns = _cellsGrid.GetLength(1);

            for (int y = 0; y < numColumns; y++)
            {
                for (int x = 0; x < numRows; x++)
                {
                    Cell cell = _cellsGrid[x, y];

                    if (cell.Cube.IsHasView)
                    {
                        cell.Cube.CubeView.Destroy();
                    }
                }
            }
        }

        private void SaveProgress()
        {
            _dataProvider.PlayerDataContainer.PlayerProgress.CurrentLevel.SaveConvertCellArrayToString(_cellsGrid);
            _saveLoadService.SaveProgress();
        }

        private void PrintCellsGrid()
        {
            int numRows = _cellsGrid.GetLength(0);
            int numColumns = _cellsGrid.GetLength(1);

            for (int y = numColumns - 1; y >= 0; y--)
            {
                string line = "";
                for (int x = 0; x < numRows; x++)
                {
                    Cell cell = _cellsGrid[x, y];
                    int cubeType = cell.Cube.CubeType;
                    line += cubeType + " ";
                }

                Debug.Log(line); 
            }

            
            string separatorLine = "";
            for (int i = 0; i < numRows; i++)
            {
                separatorLine += "--";
            }

            Debug.Log(separatorLine);
        }
    }
}