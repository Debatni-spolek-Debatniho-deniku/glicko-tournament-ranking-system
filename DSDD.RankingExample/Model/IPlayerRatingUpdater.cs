namespace DSDD.RankingExample.Model;

public interface IPlayerRatingUpdater
{
    void UpdateRatings(IReadOnlyList<ITeam> winningOrder);
}