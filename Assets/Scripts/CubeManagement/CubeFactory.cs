using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game2048.CubeManagement
{
    public class CubeFactory : MonoBehaviour
    {
        [SerializeField] private Cube _cubePrefab;
        [Inject] private DiContainer _container;

        /// <summary>
        /// Use -1 as a value to set randomly
        /// </summary>
        /// <param name="value"></param>
        /// <param name=""></param>
        public Cube CreateCube(int value, Vector3 position, Quaternion rotation)
        {
            var cube = _container.InstantiatePrefab(_cubePrefab, position, rotation, null).GetComponent<Cube>();
            cube.Setup(value);
            return cube;
        }
    }
}
