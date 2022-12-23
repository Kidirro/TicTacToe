using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Managers
{
    public class IAPManager : Singleton<IAPManager>, IStoreListener
    {
        private const string _betatestBundle = "bandle_1";

        public string BetatestBundle
        {
            get { return _betatestBundle; }
        }

        private static Action _purchased;
        private static IStoreController _storeController;
        public static IStoreController StoreController
        {
            get { return _storeController; }
        }
        private static IExtensionProvider _storeExtensionProvider;

        public static bool IsSubscribeEnable = false;

        private void Awake()
        {
            IAPInitializate();
        }

        public void IAPInitializate()
        {
            if (IsIAPInitialized())
                return;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder
                .AddProduct(_betatestBundle, ProductType.Consumable);
            UnityPurchasing.Initialize(this, builder);
        }

        public static bool IsIAPInitialized()
        {
            return _storeController != null && _storeExtensionProvider != null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {

        }

        public static IEnumerator CheckSubscription()
        {
            while (!IsIAPInitialized())
            {
                IAPManager.Instance.IAPInitializate();
                yield return new WaitForSeconds(0.5f);
            }
            if (_storeController != null || _storeController.products != null)
            {
                foreach (var product in _storeController.products.all)
                {
                    if (product.hasReceipt)
                    {
                        IsSubscribeEnable = true;
                        break;
                    }
                    IsSubscribeEnable = false;
                }
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (_purchased != null) _purchased();
            _purchased = null;
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {

            _purchased = null;
        }

        /// <summary>
        /// Ïðîâåðèòü, êóïëåí ëè òîâàð.
        /// </summary>
        /// <param name="id">Èíäåêñ òîâàðà â ñïèñêå.</param>
        /// <returns></returns>
        public static bool CheckBuyState(string id)
        {
            Product product = _storeController.products.WithID(id);
            if (product.hasReceipt) { return true; }
            else { return false; }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _storeExtensionProvider = extensions;
            //I2.Loc.GlobalParameters.SetString("PriceWeek", _storeController.products.WithStoreSpecificID("weekly_subscription").metadata.localizedPrice.ToString());
        }

        public static void BuyProductID(string productId, Action action)
        {
            Debug.Log("Try to buy: " + productId);
            if (IsIAPInitialized())
            {
                _purchased = action;
                Product product = _storeController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    _storeController.InitiatePurchase(product);
                }
            }
        }
        
    }

}