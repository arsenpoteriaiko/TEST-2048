using DG.Tweening;
using Game2048.GameManagement;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game2048.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuObject;
        [Inject] private GameManager _gameManager;

        private void Start()
        {
            menuObject.SetActive(false);
            menuObject.transform.DOScale(0.01f, 0);
        }

        private void OnEnable()
        {
            _gameManager.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            _gameManager.GameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            menuObject.SetActive(true);
            menuObject.transform.DOScale(1, 0.25f).SetEase(Ease.Linear).SetUpdate(true);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
