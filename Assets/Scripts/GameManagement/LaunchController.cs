using Cysharp.Threading.Tasks;
using Game2048.CubeManagement;
using Game2048.CubeManagement.AutoMerge;
using System;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace Game2048.GameManagement
{
    public class LaunchController : MonoBehaviour
    {
        public bool IsHoldingCube { get; private set; }
        public event Action<Cube> CubeLaunched;

        [SerializeField] private RectTransform _inputZone;
        [SerializeField] private Vector3 _cubeSpawnPos;
        [SerializeField] private float _maxCubePosOffset;
        [SerializeField] private float _cubeSpawnCooldown;

        [Header("Launch settings")]
        [SerializeField] private Vector3 _launchDirection;
        [SerializeField] private float _launchForce;

        [Inject] private CubeFactory _cubeFactory;
        [Inject] private MergeBoosterController _boosterController;
        private Cube _currentCube;

        private void Start()
        {
            SpawnCube();
        }

        private void Update()
        {
            if (_currentCube == null || _boosterController.IsActive) return;

            Vector3 screenPos;

            if (Input.touchCount > 0)
            {
                screenPos = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButton(0))
            {
                screenPos = Input.mousePosition;
            }
            else if (IsHoldingCube)
            {
                LaunchCube();
                IsHoldingCube = false;
                return;
            }
            else
                return;

            if (!RectTransformUtility.RectangleContainsScreenPoint(_inputZone, screenPos, null)) return;

            IsHoldingCube = true;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 pos = _currentCube.transform.position;
                pos.x = hit.point.x;
                pos.x = Mathf.Clamp(pos.x, -_maxCubePosOffset, _maxCubePosOffset);
                _currentCube.transform.position = pos;
            }
        }

        private void SpawnCube()
        {
            IsHoldingCube = false;
            _currentCube = _cubeFactory.CreateCube(-1, _cubeSpawnPos, Quaternion.identity);
        }

        private void LaunchCube()
        {
            _currentCube.Push(_launchDirection.normalized, _launchForce);
            CubeLaunched?.Invoke(_currentCube);
            _currentCube = null;

            WaitForSpawn().Forget();
        }

        private async UniTask WaitForSpawn()
        {
            await UniTask.WaitForSeconds(_cubeSpawnCooldown);
            SpawnCube();
        }
    }
}