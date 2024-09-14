using System.Collections.Generic;
using Assets.Knife;
using Assets.ShopManagement;

namespace Assets.GameProgression
{
    public interface IPlayerProgressionData
    {
        int Money { get; set; }
        int BestScore { get; set; }
        int FinishedLevels { get; set; } 
        int UnlockedLevels { get; set; }
        List<int> RatingScoreOfLevels { get; set; } 
    }
}
