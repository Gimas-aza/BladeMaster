using Assets.Ads;

namespace Assets.EntryPoint
{
    public interface IAdService
    {
        void ShowAd(TypeAd typeAd);
        void DestroyAd(TypeAd typeAd);
    }
}