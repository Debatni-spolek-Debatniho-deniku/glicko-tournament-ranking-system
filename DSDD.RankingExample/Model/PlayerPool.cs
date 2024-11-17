using System.Collections;
using System.Text;

namespace DSDD.RankingExample.Model;

public class PlayerPool: IEnumerable<IPlayer>
{
    public PlayerPool(IEnumerable<IPlayer> players, ITeamFactory teamFactory)
    {
        _teamFactory = teamFactory;
        _players = players.ToArray();
    }

    public IEnumerable<Match> GetRandomizedMatches()
    {
        List<ITeam> unassignedTeams = new(GetRandomizedTeams());

        while (unassignedTeams.Count > 0)
        {
            ITeam? team1 = popRandomTeam();
            ITeam? team2 = popRandomTeam();
            ITeam? team3 = popRandomTeam();
            ITeam? team4 = popRandomTeam();

            if (team1 is not null && team2 is not null && team3 is not null && team4 is not null)
                yield return new(team1, team2, team3, team4);
        }

        ITeam? popRandomTeam()
            => PopRandomItem(unassignedTeams);
    }

    public IEnumerable<ITeam> GetRandomizedTeams()
    {
        List<IPlayer> unassignedPlayers = new(_players);

        while (unassignedPlayers.Count > 0)
        {
            IPlayer? player1 = popRandomPlayer();
            IPlayer? player2 = popRandomPlayer();

            if (player1 is not null && player2 is not null)
                yield return _teamFactory.Create(player1, player2);
        }

        IPlayer? popRandomPlayer()
            => PopRandomItem(unassignedPlayers);
    }

    public IEnumerator<IPlayer> GetEnumerator()
        => _players.Cast<IPlayer>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (IPlayer player in _players)
            sb.AppendLine(player.ToString());

        return sb.ToString();
    }

    private readonly IPlayer[] _players;
    private readonly ITeamFactory _teamFactory;

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