using System.Collections;
using System.Xml.Linq;
using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Elo;

public class EloTeam: ITeam, IEnumerable<EloPlayer>
{
    public string Name => $"{Player1.Name}-{Player2.Name}";

    public EloPlayer Player1 { get; }

    IPlayer ITeam.Player1 => Player1;

    public EloPlayer Player2 { get; }

    IPlayer ITeam.Player2 => Player2;

    public double Rating => ((IEnumerable<EloPlayer>)this).Average(p => p.Rating);

    public EloTeam(EloPlayer player1, EloPlayer player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public IEnumerator<EloPlayer> GetEnumerator()
    {
        yield return Player1;
        yield return Player2;
    }

    IEnumerator<IPlayer> IEnumerable<IPlayer>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public override string ToString()
        => $"Team {Name}: Rating={Rating:F1}";
}