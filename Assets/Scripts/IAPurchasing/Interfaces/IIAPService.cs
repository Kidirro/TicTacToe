using System;

namespace IAPurchasing.Interfaces
{
    public interface IIAPService
    {
        public void IAPInitializate();
        public bool CheckBuyState(string id);
        public void BuyProductID(string productId, Action action);
        public string GetBetatestBundleId();
    }
}