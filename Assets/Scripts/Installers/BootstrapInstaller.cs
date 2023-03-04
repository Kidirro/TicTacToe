using Cards;
using GameScene.Interfaces;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCardFactory();
        BindGameScene();
    }

    private ConcreteIdArgConditionCopyNonLazyBinder BindGameScene()
    {
        return Container.BindInterfacesAndSelfTo<IGameSceneService>().AsSingle();
    }

    private void BindCardFactory()
    {
        Container.BindInterfacesAndSelfTo<DeckData>().AsSingle();
    }
}