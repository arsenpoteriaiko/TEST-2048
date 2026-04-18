using Game2048.CubeManagement;
using Game2048.CubeManagement.AutoMerge;
using Game2048.GameManagement;
using System;
using UnityEngine;
using Zenject;

namespace Game2048.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _launchSound;
        [SerializeField] private AudioSource _mergeSound;
        [SerializeField] private AudioSource _windSound;

        [Inject] private LaunchController _launchController;
        [Inject] private MergeBoosterController _mergeBoosterController; 

        private void OnEnable()
        {
            Cube.Merged += OnMerge;
            _launchController.CubeLaunched += OnLaunch;
            _mergeBoosterController.Activated += OnBoostActivated;
            _mergeBoosterController.Finished += OnBoostMerge;
        }

        private void OnDisable()
        {
            Cube.Merged -= OnMerge;
            _launchController.CubeLaunched -= OnLaunch;
            _mergeBoosterController.Activated -= OnBoostActivated;
            _mergeBoosterController.Finished -= OnBoostMerge;
        }

        private void OnBoostActivated()
        {
            _windSound.Play();
        }

        private void OnBoostMerge()
        {
            _mergeSound.Play();
        }

        private void OnMerge(Cube cube)
        {
            _mergeSound.Play();
        }

        private void OnLaunch(Cube cube)
        {
            _launchSound.Play();
        }
    }
}