namespace DSDD.RankingExample.Model;

public class Match
{
    public string Id { get; }

    public ITeam OpenningGovernment { get; }

    public ITeam OpenningOposition { get; }

    public ITeam ClosingGovernment { get; }
    
    public ITeam ClosingOpossition { get; }

    public Match(ITeam openningGovernment, ITeam openningOposition, ITeam closingGovernment, ITeam closingOpossition)
    {
        Id = Interlocked.Increment(ref _lastId).ToString();

        OpenningGovernment = openningGovernment;
        OpenningOposition = openningOposition;
        ClosingGovernment = closingGovernment;
        ClosingOpossition = closingOpossition;
    }

    public override string ToString()
        => $"Match {Id}";

    private static int _lastId = 0;
}