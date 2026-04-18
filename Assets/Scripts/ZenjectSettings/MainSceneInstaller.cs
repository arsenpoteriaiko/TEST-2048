using Game2048.CubeManagement;
using Game2048.CubeManagement.AutoMerge;
using Game2048.GameManagement;
using UnityEngine;
using Zenject;

namespace Game2048.ZenjectSettings
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private CubeColorPalette _colorPalette;
        [SerializeField] private LaunchController _launchController;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private MergeBoosterController _boosterController;
        [SerializeField] private CubeFactory _cubeFactory;

        public override void InstallBindings()
        {
            Container.Bind<CubeColorPalette>().FromInstance(_colorPalette).AsSingle();
            Container.Bind<LaunchController>().FromInstance(_launchController).AsSingle();
            Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
            Container.Bind<MergeBoosterController>().FromInstance(_boosterController).AsSingle();
            Container.Bind<CubeFactory>().FromInstance(_cubeFactory).AsSingle();
        }
    }
}