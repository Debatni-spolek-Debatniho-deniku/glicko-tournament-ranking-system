namespace DSDD.RankingExample.Model;

public class ScoredTeam
{
    public Team Team { get; }

    public Score Score { get; }

    public ScoredTeam(Team team, Score score)
    {
        Team = team;
        Score = score;
    }
}