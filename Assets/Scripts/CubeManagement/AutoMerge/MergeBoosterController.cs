using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game2048.GameManagement;
using Game2048.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game2048.CubeManagement.AutoMerge
{
    public class MergeBoosterController : MonoBehaviour
    {
        public event Action Activated;
        public event Action Finished;

        public bool IsActive { get; private set; }

        [SerializeField] private List<CubesPosition> _positions;
        [SerializeField] private Transform _targetPosition;
        [SerializeField] private MergeBoosterUI _mergeBoosterUI;

        [Header("Animation settings")]
        [SerializeField] private float _animationStep1Duration;

        [Inject] private GameManager _gameManager;
        [Inject] private LaunchController _launchController;
        [Inject] private CubeFactory _cubeFactory;

        private void Start()
        {
            IsActive = false;
        }

        public void ActivateBooster()
        {
            if (!TryGetCubesForMerge(out Cube cube1, out Cube cube2) || _launchController.IsHoldingCube || IsActive)
            {
                _mergeBoosterUI.ShowError();
                return;
            }

            IsActive = true;
            _mergeBoosterUI.SetButtonActive(false);
            Activated?.Invoke();

            PrepareCube(cube1);
            PrepareCube(cube2);

            Merge(cube1, cube2).Forget();
        }

        private void PrepareCube(Cube cube)
        {
            cube.enabled = false;
            cube.Collider.isTrigger = true;
            cube.SetPhysics(false);
            cube.transform.rotation = Quaternion.identity;
        }

        private async UniTask Merge(Cube cube1, Cube cube2)
        {
            var positions = _positions[UnityEngine.Random.Range(0, _positions.Count)];
            _ = Animate(cube1.transform, positions.Position1);
            await Animate(cube2.transform, positions.Position2);

            var value = cube1.Value * 2;
            Destroy(cube1.gameObject);
            Destroy(cube2.gameObject);

            var cube = _cubeFactory.CreateCube(value, _targetPosition.position, UnityEngine.Random.rotationUniform);
            _gameManager.AddToLaunched(cube);
            cube.PlayParticles();

            IsActive = false;
            _mergeBoosterUI.SetButtonActive(true);
            Finished?.Invoke();
        }

        private async UniTask Animate(Transform objectTransform, Transform target)
        {
            var tween = objectTransform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
            await objectTransform.DOMove(target.position, _animationStep1Duration).SetEase(Ease.Linear).AsyncWaitForCompletion();
            tween.Kill();

            await objectTransform.DOMoveY(objectTransform.position.y - 0.2f, 0.2f).SetEase(Ease.Linear).SetLoops(5, LoopType.Yoyo).AsyncWaitForCompletion();
            await objectTransform.DOMove(_targetPosition.position, 0.15f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        }

        private bool TryGetCubesForMerge(out Cube cube1, out Cube cube2)
        {
            var cubes = _gameManager.LaunchedCubes;
            cube1 = null;
            cube2 = null;

            if (cubes.Count < 2) return false;

            List<Cube> sorted = new(cubes);
            sorted.Sort((a, b) => b.Value.CompareTo(a.Value));

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                if (sorted[i].Value == sorted[i + 1].Value)
                {
                    cube1 = sorted[i];
                    cube2 = sorted[i + 1];
                    return true;
                }
            }

            return false;
        }

        [System.Serializable]
        public struct CubesPosition
        {
            [field: SerializeField] public Transform Position1 { get; private set; }
            [field: SerializeField] public Transform Position2 { get; private set; }
        }
    }
}
