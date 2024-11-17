namespace DSDD.RankingExample.Model;

public interface IPlayerFactory
{
    IPlayer Create(string name);
}