using UnityEngine;

namespace Game2048.CubeManagement
{
    [CreateAssetMenu(fileName = "CubeColorPalette", menuName = "Game2048/Color Palette")]
    public class CubeColorPalette : ScriptableObject
    {
        [SerializeField] private Color[] _cubeColors;

        public Color GetColor(int value)
        {
            if (value == 2) return _cubeColors[0];

            int id = (int)Mathf.Log(value, 2);

            if (id >= _cubeColors.Length)
                id = _cubeColors.Length - 1;

            return _cubeColors[id];
        }
    }
}