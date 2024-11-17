namespace DSDD.RankingExample.Model;

public interface ITeamFactory
{
    ITeam Create(IPlayer player1, IPlayer player2);
}