using System;
using CardCollection;
using Cards;
using Network;
using UIPages;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainMenuLocationInstaller : MonoInstaller
    {
    [SerializeField]
        private MasterConnectorManager _masterConnectorManager;

        [SerializeField]
        private MainMenuUI _mainMenuUI;

        [SerializeField]
        private CollectionManager _collectionManager;
        
        public override void InstallBindings()
        {
            BindMasterConnector();
            BindMainMenuUI();
            BindCollectionManager();
            BindCollectionFactory();
        }

        private void BindCollectionFactory()
        {
            Container.BindInterfacesAndSelfTo<CardCollectionFactory>().AsSingle();
        }

        private void BindCollectionManager()
        {
            Container.BindInterfacesTo<CollectionManager>().FromInstance(_collectionManager).AsSingle();
        }

        private void BindMainMenuUI()
        {
            Container.BindInterfacesTo<MainMenuUI>().FromInstance(_mainMenuUI).AsSingle();
        }


        private void BindMasterConnector()
        {
            Container.BindInterfacesTo<MasterConnectorManager>().FromInstance(_masterConnectorManager).AsSingle();
        }
    }
}