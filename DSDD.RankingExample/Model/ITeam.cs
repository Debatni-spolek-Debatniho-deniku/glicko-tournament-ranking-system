namespace DSDD.RankingExample.Model;

public interface ITeam: IEnumerable<IPlayer>
{
    string Name { get; }

    IPlayer Player1 { get; }

    IPlayer Player2 { get; }
}