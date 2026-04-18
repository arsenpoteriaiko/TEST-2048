using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game2048.UI
{
    public class MergeBoosterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private Button _boosterButton;

        private CancellationTokenSource _errorCTS;
        private Vector2 _defaultTextScale;

        private void Start()
        {
            _defaultTextScale = _errorText.transform.localScale;
            ResetText();
        }

        private void ResetText()
        {
            _errorText.DOKill();

            _errorText.transform.localScale = _defaultTextScale;
            _errorText.DOFade(0, 0);
        }

        public void ShowError()
        {
            ResetText();

            _errorCTS?.Cancel();
            Error().Forget();
        }

        private async UniTask Error()
        {
            _errorCTS = new();

            _errorText.transform.DOScale(_defaultTextScale * 1.5f, 0.5f);
            _errorText.DOFade(1, 0.5f);

            try
            {
                await UniTask.WaitForSeconds(1, cancellationToken: _errorCTS.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            ResetText();
        }

        public void SetButtonActive(bool condition)
        {
            _boosterButton.interactable = condition;
        }
    }
}
