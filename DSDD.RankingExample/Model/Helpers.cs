namespace DSDD.RankingExample.Model;

public static class Helpers
{
    /// <summary>
    /// Having a list of all teams in winning order, returns a score where 1 matches first member of the list and 0 matches the last, equally distributed in between.
    /// </summary>
    /// <param name="teamIndex"></param>
    /// <returns></returns>
    public static double DeriveScoresFromWinningIndex(int teamIndex)
    {
        if (teamIndex < 0 || teamIndex > 3)
            throw new ArgumentOutOfRangeException(nameof(teamIndex), "Team index must be between 0 and 3.");
        
        double maxScore = 1.0;
        double minScore = 0.0;
        double step = (maxScore - minScore) / 3; // 4 players: 3 intervals
        return new double[] { maxScore, maxScore - step, maxScore - 2 * step, minScore }[teamIndex];
    }
}