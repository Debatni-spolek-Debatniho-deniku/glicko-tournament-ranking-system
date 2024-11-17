using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Glicko;

public class GlickoFactories: ITeamFactory, IPlayerFactory
{
    public ITeam Create(IPlayer player1, IPlayer player2)
    {
        if (player1 is not GlickoPlayer g1)
            throw new ArgumentException("Player1 must be of type GlickoPlayer", nameof(player1));
        if (player2 is not GlickoPlayer g2)
            throw new ArgumentException("Player2 must be of type GlickoPlayer", nameof(player2));

        return new GlickoTeam(g1, g2);
    }

    public IPlayer Create(string name)
        => new GlickoPlayer(name);
}