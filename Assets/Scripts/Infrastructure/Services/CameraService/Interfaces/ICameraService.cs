using Infrastructure.Systems.Core.Components;
using UnityEngine;

namespace Infrastructure.Services.CameraService.Interfaces
{
    public interface ICameraService
    {
        Vector3[] GenerateRandomPointsOutsideCamera();

        void CenterCameraOnGrid(Cell[,] grid);
    }
}