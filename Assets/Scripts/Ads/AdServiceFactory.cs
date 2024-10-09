using Assets.EntryPoint;

namespace Assets.Ads
{
    public class AdServiceFactory : IAdService
    {
        private IAds _ads;

        public AdServiceFactory()
        {
            _ads = new YandexAds();
        }

        public void ShowAd(TypeAd typeAd)
        {
            switch (typeAd)
            {
                case TypeAd.Interstitial:
                    _ads.ShowInterstitial();
                    break;
                case TypeAd.Banner:
                    break;
                case TypeAd.Rewarded:
                    break;
            }
        }

        public void DestroyAd(TypeAd typeAd)
        {
            switch (typeAd)
            {
                case TypeAd.Interstitial:
                    _ads.DestroyInterstitial();
                    break;
                case TypeAd.Banner:
                    break;
                case TypeAd.Rewarded:
                    break;
            }
        }
    }
}