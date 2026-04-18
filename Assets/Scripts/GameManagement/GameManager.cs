using Game2048.CubeManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game2048.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public event Action GameOver;

        [field: SerializeField] public List<Cube> LaunchedCubes { get; private set; }

        [SerializeField] private int _maxCubeCount;
        [Inject] private LaunchController _launchController;
        private List<Cube> _cubeList;

        private void Awake()
        {
            _cubeList = new();
            LaunchedCubes = new();
        }

        private void OnEnable()
        {
            Cube.Spawned += OnCubeSpawned;
            Cube.Destroyed += OnCubeDestroyed;
            _launchController.CubeLaunched += OnCubeLaunched;
        }

        private void OnDisable()
        {
            Cube.Spawned -= OnCubeSpawned;
            Cube.Destroyed -= OnCubeDestroyed;
            _launchController.CubeLaunched -= OnCubeLaunched;
        }

        private void OnCubeSpawned(Cube cube)
        {
            _cubeList.Add(cube);

            if (_cubeList.Count >= _maxCubeCount)
            {
                GameOver?.Invoke();
            }
        }

        private void OnCubeLaunched(Cube cube)
        {
            LaunchedCubes.Add(cube);
        }

        private void OnCubeDestroyed(Cube cube)
        {
            _cubeList.Remove(cube);
            LaunchedCubes.Remove(cube);
        }

        public void AddToLaunched(Cube cube)
        {
            if (!LaunchedCubes.Contains(cube))
                LaunchedCubes.Add(cube);
        }
    }
}
