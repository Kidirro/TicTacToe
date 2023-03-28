namespace Analytic.Interfaces
{
    public interface IStoreEventsAnalyticService
    {
        public void Player_Open_Store();
        public void Player_Try_Purchase(string bundleId);
        public void Player_Bought_Bundle(string bundleId);
        public void Player_Bought_Random_Card();
    }
}