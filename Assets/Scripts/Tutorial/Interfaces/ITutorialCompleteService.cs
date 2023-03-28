namespace Tutorial.Interfaces
{
    public interface ITutorialCompleteService
    {
        public bool GetIsTutorialComplete();
        public void SetIsTutorialComplete(bool state);
    }
}