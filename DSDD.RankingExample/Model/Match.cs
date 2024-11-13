using System.Collections;

namespace DSDD.RankingExample.Model;

public class Match: IEnumerable<Team>
{
    public Team OpenningGovernment { get; }

    public Team OpenningOposition { get; }

    public Team ClosingGovernment { get; }
    
    public Team ClosingOpossition { get; }

    public Match(Team openningGovernment, Team openningOposition, Team closingGovernment, Team closingOpossition)
    {
        OpenningGovernment = openningGovernment;
        OpenningOposition = openningOposition;
        ClosingGovernment = closingGovernment;
        ClosingOpossition = closingOpossition;
    }

    public IEnumerator<Team> GetEnumerator()
    {
        yield return OpenningGovernment;
        yield return OpenningOposition;
        yield return ClosingGovernment;
        yield return ClosingOpossition;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
        => $"Game {this.SelectMany(team => team.Select(player => player.Name))}";
}