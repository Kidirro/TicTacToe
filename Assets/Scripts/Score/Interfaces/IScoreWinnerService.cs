namespace Score.Interfaces
{
    public interface IScoreWinnerService
    {
        public bool IsExistRoundWinner();
        public int GetRoundWinner();
        public bool IsExistGameWinner();
        public int GetGameWinner();
        public void AddRoundWinner(int side);
        public void ClearRoundWinners();
        public int GetCountRoundWin(int side);
        public int GetRoundCount();
    }
}