namespace Vibration.Interfaces
{
    public interface IVibrationService
    {
        public void Init();
        public void VibratePop();
        public void VibratePeek();
        public void VibrateNope();
        public void Vibrate(long milliseconds);
        public void Vibrate(long[] pattern, int repeat);
        public void Cancel();
        public bool HasVibrator();
        public void Vibrate();
    }
}