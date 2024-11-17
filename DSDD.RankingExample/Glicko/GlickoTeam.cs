using System.Collections;
using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Glicko;

public class GlickoTeam: ITeam, IEnumerable<GlickoPlayer>
{
    public string Name => $"{Player1.Name}-{Player2.Name}";

    public GlickoPlayer Player1 { get; }

    IPlayer ITeam.Player1 => Player1;

    public GlickoPlayer Player2 { get; }

    IPlayer ITeam.Player2 => Player2;

    public double Rating => ((IEnumerable<GlickoPlayer>)this).Average(p => p.Rating);

    public double RatingDeviation => ((IEnumerable<GlickoPlayer>)this).Average(p => p.RatingDeviation);

    public GlickoTeam(GlickoPlayer player1, GlickoPlayer player2)
    {
        Player1 = player1;
        Player2 = player2;
    }


    public IEnumerator<GlickoPlayer> GetEnumerator()
    {
        yield return Player1;
        yield return Player2;
    }

    IEnumerator<IPlayer> IEnumerable<IPlayer>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public override string ToString()
        => $"Team {Name}: Rating={Rating:F1}, RatingDeviation={RatingDeviation:F1}";
}