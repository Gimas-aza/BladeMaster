using System.Collections.Generic;

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
