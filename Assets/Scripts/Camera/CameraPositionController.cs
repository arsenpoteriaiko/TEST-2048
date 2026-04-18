using DG.Tweening;
using Game2048.CubeManagement;
using Game2048.GameManagement;
using System;
using UnityEngine;
using Zenject;

namespace Game2048.Camera
{
    public class CameraPositionController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] [Range(20, 180)] private float _scopedFov;

        [Inject] private LaunchController _launchController;
        private float _defaultFov;

        private void Start()
        {
            _defaultFov = _camera.fieldOfView;
        }

        private void OnEnable()
        {
            _launchController.CubeLaunched += OnLaunch;
        }

        private void OnDisable()
        {
            _launchController.CubeLaunched -= OnLaunch;
        }

        private void OnLaunch(Cube cube)
        {
            _camera.DOFieldOfView(_scopedFov, 0.125f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCirc);
        }
    }
}
