using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Elo;

public class EloFactories: ITeamFactory, IPlayerFactory
{
    public ITeam Create(IPlayer player1, IPlayer player2)
    {
        if (player1 is not EloPlayer e1)
            throw new ArgumentException("Player1 must be of type EloPlayer", nameof(player1));
        if (player2 is not EloPlayer e2)
            throw new ArgumentException("Player2 must be of type EloPlayer", nameof(player2));

        return new EloTeam(e1, e2);
    }

    public IPlayer Create(string name)
        => new EloPlayer(name);
}