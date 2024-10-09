using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace Assets.Ads
{
    public class YandexAds : IAds
    {
        private string _interstitialAdId = "demo-interstitial-yandex";
        private InterstitialAdLoader _interstitialAdLoader;
        private Interstitial _interstitial;

        public YandexAds()
        {
            SetupLoader();
            RequestInterstitial();
        }

        private void SetupLoader()
        {
            _interstitialAdLoader = new InterstitialAdLoader();
            _interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
        }

        private void RequestInterstitial()
        {
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(_interstitialAdId).Build();
            _interstitialAdLoader.LoadAd(adRequestConfiguration);
        }

        private void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            _interstitial = args.Interstitial;

            _interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            _interstitial.OnAdDismissed += HandleInterstitialDismissed;
        }

        private void HandleInterstitialDismissed(object sender, EventArgs args)
        {
            DestroyInterstitial();
            RequestInterstitial();
        }

        private void HandleInterstitialFailedToShow(object sender, EventArgs args)
        {
            Debug.LogError($"Interstitial failed to show. {args}");

            DestroyInterstitial();
            RequestInterstitial();
        }

        public void ShowInterstitial()
        {
            _interstitial?.Show();
        }

        public void DestroyInterstitial()
        {
            _interstitial?.Destroy();
            _interstitial = null;
        }
    }
}