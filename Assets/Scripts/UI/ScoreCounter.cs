using DG.Tweening;
using Game2048.CubeManagement;
using Game2048.GameManagement;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game2048.UI
{
    public class ScoreCounter : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private TextMeshProUGUI _scoreText;
        [Inject] private GameManager _gameManager;
        private string _defaultText;
        private Vector2 _defaultScale;

        private void Start()
        {
            Score = 0;

            _defaultScale = transform.localScale;
            _defaultText = _scoreText.text;
            _scoreText.text = _defaultText + Score;
        }

        private void OnEnable()
        {
            Cube.Merged += OnMerge;
            _gameManager.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            Cube.Merged -= OnMerge;
            _gameManager.GameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            Cube.Merged -= OnMerge;
        }

        private void OnMerge(Cube cube)
        {
            Score += cube.Value / 4;
            _scoreText.text = _defaultText + Score;

            _scoreText.DOKill();
            _scoreText.transform.localScale = _defaultScale;
            _scoreText.transform.DOScale(_defaultScale.x * 1.5f, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
        }
    }
}