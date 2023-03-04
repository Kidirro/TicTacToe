using Managers;

namespace GameScene.Interfaces
{
    public interface IGameSceneService
    {
        public void BeginLoadGameScene(GameSceneManager.GameScene state);
        public void BeginTransaction();


    }
}