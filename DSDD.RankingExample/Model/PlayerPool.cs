using System.Collections;
using System.Text;

namespace DSDD.RankingExample.Model;

public class PlayerPool: IEnumerable<Player>
{
    public PlayerPool(IEnumerable<Player> players)
    {
        _players = players.ToArray();
    }

    public IEnumerable<Match> GetRandomizedMatches()
    {
        List<Team> unassignedTeams = new(GetRandomizedTeams());

        while (unassignedTeams.Count > 0)
        {
            Team? team1 = popRandomTeam();
            Team? team2 = popRandomTeam();
            Team? team3 = popRandomTeam();
            Team? team4 = popRandomTeam();

            if (team1 is not null && team2 is not null && team3 is not null && team4 is not null)
                yield return new(team1, team2, team3, team4);
        }

        Team? popRandomTeam()
            => PopRandomItem(unassignedTeams);
    }

    public IEnumerable<Team> GetRandomizedTeams()
    {
        List<Player> unassignedPlayers = new(_players);

        while (unassignedPlayers.Count > 0)
        {
            Player? player1 = popRandomPlayer();
            Player? player2 = popRandomPlayer();

            if (player1 is not null && player2 is not null)
                yield return new(player1, player2);
        }

        Player? popRandomPlayer()
            => PopRandomItem(unassignedPlayers);
    }

    public IEnumerator<Player> GetEnumerator()
        => _players.Cast<Player>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (Player player in _players)
            sb.AppendLine(player.ToString());

        return sb.ToString();
    }

    private readonly Player[] _players;
    private readonly Random _random = new();

    private T? PopRandomItem<T>(IList<T> pool)
        where T : class
    {
        if (pool.Count == 0)
            return null;
        int index = _random.Next(0, pool.Count - 1);
        T item = pool[index];
        pool.RemoveAt(index);
        return item;
    }
}