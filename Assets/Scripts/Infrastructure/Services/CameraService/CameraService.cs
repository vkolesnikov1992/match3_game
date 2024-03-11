using Infrastructure.Services.CameraService.Interfaces;
using Infrastructure.Systems.Core.Components;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.CameraService
{
    public class CameraService : ICameraService, IInitializable
    {
        private Camera _mainCamera;
        
        public void Initialize()
        {
            _mainCamera = Camera.main;
        }
        
        public Vector3[] GenerateRandomPointsOutsideCamera()
        {
            float cameraHeight = _mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * _mainCamera.aspect;

            float yCenter = _mainCamera.transform.position.y;

            float yMin = yCenter - cameraHeight / 2 * 0.3f;
            float yMax = yCenter + (cameraHeight / 2) * (1 - 0.3f);

            Vector3[] randomPoints = new Vector3[2];

            float leftOffset = cameraWidth * 0.1f;
            float rightOffset = cameraWidth * 0.1f;

            float leftX = _mainCamera.transform.position.x - (cameraWidth / 2 + leftOffset);
            float rightX = _mainCamera.transform.position.x + (cameraWidth / 2 + rightOffset);

            float randomValue = Random.value;
            
            randomPoints[0] = randomValue < 0.5f ?
                new Vector3(leftX, Random.Range(yMin, yMax), 0) :
                new Vector3(rightX, Random.Range(yMin, yMax), 0);

            randomPoints[1] = randomValue < 0.5f ?
                new Vector3(rightX, Random.Range(yMin, yMax), 0) :
                new Vector3(leftX, Random.Range(yMin, yMax), 0);

            return randomPoints;
        }
        
        public void CenterCameraOnGrid(Cell[,] grid)
        {
            Bounds gridBounds = CalculateGridBounds(grid);

            Vector3 cameraTargetPosition = gridBounds.center;
            _mainCamera.transform.position = new Vector3(cameraTargetPosition.x, cameraTargetPosition.y + 1f, _mainCamera.transform.position.z);
        }

        private Bounds CalculateGridBounds(Cell[,] grid)
        {
            Bounds gridBounds = new Bounds(Vector3.zero, Vector3.zero);

            foreach (Cell cell in grid)
            {
                gridBounds.Encapsulate(new Vector3(cell.X, cell.Y, 0f));
            }

            return gridBounds;
        }
    }
}