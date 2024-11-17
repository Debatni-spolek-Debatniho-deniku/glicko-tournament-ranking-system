namespace DSDD.RankingExample.Model;

public class Match
{
    public string Id { get; }

    public Team OpenningGovernment { get; }

    public Team OpenningOposition { get; }

    public Team ClosingGovernment { get; }
    
    public Team ClosingOpossition { get; }

    public Match(Team openningGovernment, Team openningOposition, Team closingGovernment, Team closingOpossition)
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