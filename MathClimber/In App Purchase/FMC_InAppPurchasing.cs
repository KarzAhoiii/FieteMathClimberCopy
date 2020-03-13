using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class FMC_InAppPurchasing : IStoreListener
{

    public enum availableProducts { sub01, sub02, oneTimePayment };

    // Apple: Non Consumable Product IDs (Identisch wie im App Store)
    public static string subscription01 = "com.ahoiii.FieteMathClimberSubscriptionMonthly";
    public static string subscription02 = "com.ahoiii.FieteMathClimberSubscriptionYearly";
    public static string oneTimePurchase = "com.ahoiii.FieteMathClimberOneTimePurchase";

    // Google: Non Consumable Product IDs (Identisch wie im App Store)
    //public static string product01 = "com.ahoiii.fietemathclimberproduct01";
    //public static string product02 = "com.ahoiii.fietemathclimberproduct02";
    //public static string fullVersion = "com.ahoiii.fietemathclimberfullVersion";

    public bool boughtSub01InThisSession { get; private set; }
    public bool boughtSub02InThisSession { get; private set; }
    public bool boughtOneTimePurchaseInThisSession { get; private set; }

    public static IStoreController m_StoreController { get; private set; }         // The Unity Purchasing system.
    public static IExtensionProvider m_StoreExtensionProvider { get; private set; } // The store-specific Purchasing subsystems.

    private FMC_InAppPurchaseController inAppPurchaseController = null;

    public FMC_InAppPurchasing ()
    {
        boughtSub01InThisSession = false;
        boughtSub02InThisSession = false;
        boughtOneTimePurchaseInThisSession = false;

        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    //void Start()
    //{
    //    if (m_StoreController == null)
    //    {
    //        InitializePurchasing();
    //    }
    //}

    public void setIAPController (FMC_InAppPurchaseController controller)
    {
        inAppPurchaseController = controller;
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Adding Products
        builder.AddProduct(subscription01, ProductType.Subscription);
        builder.AddProduct(subscription02, ProductType.Subscription);
        builder.AddProduct(oneTimePurchase, ProductType.NonConsumable);

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    public bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProduct(availableProducts productToBuy)
    {
        if (productToBuy == availableProducts.sub01)
            BuyProductID(subscription01);
        else if (productToBuy == availableProducts.sub02)
            BuyProductID(subscription02);
        else if (productToBuy == availableProducts.oneTimePayment)
            BuyProductID(oneTimePurchase);
    }

    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            // Produktreferenz über identifier holen
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

                // Buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                m_StoreController.InitiatePurchase(product);

                Debug.LogWarning("Add Loading Sign here. And remove in ProcessPurchase or OnPurchaseFailed");
            }
            else
            {
                // Report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {

        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                Debug.Log("Process Purchase sollte automatisch aufgerufen werden. Alles sollte automatisch wieder freigeschaltet werden.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("In App Purchase initialised correctly");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

        if (!FMC_GameDataController.instance.subscriptionIsActive())
        {
            FMC_GameDataController.instance.checkIfSubscriptionIsActiveFromReceipt();
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);

        if (FMC_GameDataController.instance.userDidBuySubscriptionOnce())
        {
            Debug.LogWarning("Insert Logic for Information Window here. 'Activate Wifi, so we can check whether your sub is active.'");
        }
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, subscription01, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            boughtSub01InThisSession = true;
            inAppPurchaseController.boughtProduct01();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, subscription02, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            boughtSub02InThisSession = true;
            inAppPurchaseController.boughtProduct02();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, oneTimePurchase, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            boughtOneTimePurchaseInThisSession = true;
            inAppPurchaseController.boughtFullVersion();
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


}