using System.Collections;

namespace DSDD.RankingExample.Model;

public class Team: IEnumerable<Player>
{
    public Player Player1 { get; }

    public Player Player2 { get; }

    public double Rating => this.Average(p => p.Rating);

    public double RatingDeviation => this.Average(p => p.RatingDeviation);

    public Team(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public IEnumerator<Player> GetEnumerator()
    {
        yield return Player1;
        yield return Player2;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
        => $"Team {Player1.Name}{Player2.Name}: Rating={Rating:F1}, RatingDeviation={RatingDeviation:F1}";
}