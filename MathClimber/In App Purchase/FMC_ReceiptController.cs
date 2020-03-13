using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class FMC_ReceiptController
{

    private bool allreadyRefreshedReceiptInThisSession = false;

    public FMC_ReceiptController ()
    {

    }

    public void checkIfSubscriptionIsActive ()
    {
#if UNITY_IPHONE

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        var appleConfig = builder.Configure<IAppleConfiguration>();
        if (!string.IsNullOrEmpty(appleConfig.appReceipt))
        {
            var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
            AppleReceiptParser parser = new AppleReceiptParser();
            AppleReceipt receipt = parser.Parse(receiptData);
            //AppleReceipt receipt = new AppleReceiptParser.Parse(receiptData);

            FMC_GameDataController.instance.writeToReceiptLog("Check if Sub is active: Original Version: " + receipt.originalApplicationVersion);
            FMC_GameDataController.instance.writeToReceiptLog("Check if Sub is active: Bundle id: " + receipt.bundleID);
            FMC_GameDataController.instance.writeToReceiptLog("Check if Sub is active: Creation Date: " + receipt.receiptCreationDate);
            foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts)
            {
                FMC_GameDataController.instance.writeToReceiptLog("Check if Sub is active: Transaction ID: " + productReceipt.transactionID);
                FMC_GameDataController.instance.writeToReceiptLog("Check if Sub is active: Product ID: " + productReceipt.productID);
                FMC_GameDataController.instance.writeToReceiptLog("Check if Sub is active: Sub Expiration Date: " + productReceipt.subscriptionExpirationDate);

                // Wenn eine der IDs im Receipt mit einer der Abo-IDs übereinstimmt: Testen, ob das Abo gültig ist.
                if (String.Equals(productReceipt.productID, FMC_InAppPurchasing.subscription01, StringComparison.Ordinal)
                    || String.Equals(productReceipt.productID, FMC_InAppPurchasing.subscription02, StringComparison.Ordinal))
                {
                    //TimeSpan utcOffset = DateTime.Now - DateTime.UtcNow;
                    //DateTime newDateToCheck = productReceipt.subscriptionExpirationDate + utcOffset;
                    //TimeSpan timeToExpiry = productReceipt.subscriptionExpirationDate - DateTime.UtcNow;
                    //Debug.Log("---------------------------- Time to expiry: " + timeToExpiry);
                    //if (timeToExpiry.TotalSeconds > 0)
                    //    return true;

                    if (!JF_Utility.isExpired(productReceipt.subscriptionExpirationDate))
                    {
                        FMC_GameDataController.instance.setSubscriptionToActive();
                        FMC_GameDataController.instance.saveNewExpirationDate(productReceipt.subscriptionExpirationDate);
                    }
                }
            }
            // Wenn keine übereinstimmende ID in den Product Receipts gefunden wurde: Subscription nicht aktiv.

        }
        else
        {
            refreshAppleReceipt();
        }

#endif
    }

    private void refreshAppleReceipt ()
    {

#if UNITY_IPHONE

        if (FMC_InAppPurchasing.m_StoreExtensionProvider != null)
        {
            FMC_InAppPurchasing.m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RefreshAppReceipt(receipt =>
            {
                // This handler is invoked if the request is successful.
                // Receipt will be the latest app receipt.
                //Console.WriteLine(receipt);
                FMC_GameDataController.instance.writeToReceiptLog("Receipt was refreshed.");

                if (!allreadyRefreshedReceiptInThisSession)
                    checkIfSubscriptionIsActive();

                allreadyRefreshedReceiptInThisSession = true;
            },
            () =>
            {
                FMC_GameDataController.instance.writeToReceiptLog("Apple Receipt could not be refreshed.");
                // This handler will be invoked if the request fails,
                // such as if the network is unavailable or the user
                // enters the wrong password.
            });
        }

#endif
    }

    public void saveLongestLastingExpirationDate ()
    {
#if UNITY_IPHONE

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        var appleConfig = builder.Configure<IAppleConfiguration>();
        List<DateTime> allExpirationDates = new List<DateTime>();
        if (!string.IsNullOrEmpty(appleConfig.appReceipt))
        {
            var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
            AppleReceiptParser parser = new AppleReceiptParser();
            AppleReceipt receipt = parser.Parse(receiptData);
            //AppleReceipt receipt = new AppleReceiptParser.Parse(receiptData);

            FMC_GameDataController.instance.writeToReceiptLog("sled " + receipt.bundleID);
            FMC_GameDataController.instance.writeToReceiptLog("sled " + receipt.receiptCreationDate);
            foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts)
            {
                FMC_GameDataController.instance.writeToReceiptLog("sled Transaction ID: " + productReceipt.transactionID);
                FMC_GameDataController.instance.writeToReceiptLog("sled Product ID: " + productReceipt.productID);
                FMC_GameDataController.instance.writeToReceiptLog("sled Sub Expiration Date: " + productReceipt.subscriptionExpirationDate);

                // Wenn eine der IDs im Receipt mit einer der Abo-IDs übereinstimmt: Testen, ob das Abo gültig ist.
                if (String.Equals(productReceipt.productID, FMC_InAppPurchasing.subscription01, StringComparison.Ordinal)
                    || String.Equals(productReceipt.productID, FMC_InAppPurchasing.subscription02, StringComparison.Ordinal))
                {

                    allExpirationDates.Add(productReceipt.subscriptionExpirationDate);
                    //if (!JF_Utility.isExpired(productReceipt.subscriptionExpirationDate))
                    //{
                    //    FMC_GameDataController.instance.setSubscriptionToActive();
                    //    FMC_GameDataController.instance.saveNewExpirationDate(productReceipt.subscriptionExpirationDate);
                    //}
                }
            }

        }
        else
        {
            refreshAppleReceipt();
        }

        if (allExpirationDates.Count > 0)
        {
            DateTime longestExpirationDate = allExpirationDates[0];
            for (int i = 0; i < allExpirationDates.Count; i++)
            {
                FMC_GameDataController.instance.writeToReceiptLog("Sled: Test for longest expiration Date.");
                if (JF_Utility.getExpirationTime(allExpirationDates[i]) > JF_Utility.getExpirationTime(longestExpirationDate))
                    longestExpirationDate = allExpirationDates[i];
            }
            FMC_GameDataController.instance.setSubscriptionToActive();
            FMC_GameDataController.instance.saveNewExpirationDate(longestExpirationDate);
        }

#endif
    }
}
