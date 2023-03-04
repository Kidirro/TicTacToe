using Cards;
using ScreenScaler;
using Mana;
using Managers;
using Score;
using ScreenScaler;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameSceneLocationInstaller : MonoInstaller
    {
        [SerializeField]
        private ManaManager _manaManager;

        public override void InstallBindings()
        {
            BindManaManager();
            BindCardFactory();
            BindScaler();
            BindScore();
        }

        private void BindScore()
        {
            Container.BindInterfacesAndSelfTo<ScoreManager>().AsSingle();
        }

        private void BindScaler()
        {
            Container.Bind<IScreenScaler>().FromInstance(new ScreenScalerService());
        }

        private void BindCardFactory()
        {
            Container.BindInterfacesAndSelfTo<CardFactory>().AsSingle();
        }


        private void BindManaManager()
        {
            Container.Bind<ICurrentMana>().FromInstance(_manaManager).AsSingle();
            Container.Bind<IEnoughMana>().FromInstance(_manaManager).AsSingle();
            Container.Bind<IIncreaseMana>().FromInstance(_manaManager).AsSingle();
        }
    }
}