using DSDD.RankingExample;
using DSDD.RankingExample.Elo;
using DSDD.RankingExample.Glicko;
using DSDD.RankingExample.Model;
using Spectre.Console;

string algoChoice = AnsiConsole
    .Prompt(new SelectionPrompt<string>()
        .Title("Choose algorithm:")
        .AddChoices(AlgorithmChoice.ELO, AlgorithmChoice.GLICKO_1));

AnsiConsole.MarkupLine($"[maroon]Current algorith is: {algoChoice}[/]");
AnsiConsole.WriteLine("");

IPlayerFactory playerFactory;
ITeamFactory teamFactory;
IPlayerRatingUpdater playerRatingUpdater;

switch (algoChoice)
{
    case AlgorithmChoice.ELO:
        EloFactories eloFactories = new();
        playerFactory = eloFactories;
        teamFactory = eloFactories;

        playerRatingUpdater = new EloPlayerRatingUpdater();
        break;
    case AlgorithmChoice.GLICKO_1:
        GlickoFactories glickoFactories = new();
        playerFactory = glickoFactories;
        teamFactory = glickoFactories;

        playerRatingUpdater = new GlickoPlayerRatingUpdater();
        break;
    default:
        throw new IndexOutOfRangeException();
}

int playerCount = AnsiConsole.Prompt(
    new TextPrompt<int>("Number of players?").Validate(value => value % 8 == 0, "Must be divisible by 8!"));

PlayerPool pool = new(PlayersGenerator.Generate(playerCount, playerFactory), teamFactory);

while (true)
    RenderMainMenu();

void RenderMainMenu()
{
    AnsiConsole.MarkupLine("[maroon]Current players:[/]");
    AnsiConsole.Write(pool.ToString());

    AnsiConsole.WriteLine("");
    string choice = AnsiConsole
        .Prompt(new SelectionPrompt<string>()
            .Title("Main menu")
            .AddChoices(MainMenuChoice.START_ROUND, MainMenuChoice.CLOSE));

    switch (choice)
    {
        case MainMenuChoice.START_ROUND:
            StartRound();
            break;
        default:
            Environment.Exit(0);
            break;
    }
}

void StartRound()
{
    AnsiConsole.WriteLine("Starting a match...");
    Thread.Sleep(500);

    AnsiConsole.WriteLine("Randomizing teams...");

    Thread.Sleep(500);

    Match[] matches = pool.GetRandomizedMatches().ToArray();

    foreach (Match match in matches)
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.MarkupLine($"[maroon]{match} of {matches.Length}[/]");
        AnsiConsole.WriteLine($"OG: {match.OpenningGovernment}");
        AnsiConsole.WriteLine($"OO: {match.OpenningOposition}");
        AnsiConsole.WriteLine($"CG: {match.ClosingGovernment}");
        AnsiConsole.WriteLine($"CO: {match.ClosingOpossition}");
        AnsiConsole.WriteLine("");

        IReadOnlyList<ITeam> winningOrder = PickTeamOrder(match);

        playerRatingUpdater.UpdateRatings(winningOrder);
    }
}

IReadOnlyList<ITeam> PickTeamOrder(Match match)
{
    HashSet<string> options = new() { "OG", "OO", "CG", "CO" };

    return keepAsking()
        .Select(teamCode => teamCode switch
        {
            "OG" => match.OpenningGovernment,
            "OO" => match.OpenningOposition,
            "CG" => match.ClosingGovernment,
            "CO" => match.ClosingOpossition,
            _ => throw new InvalidOperationException()
        })
        .ToList();

    IEnumerable<string> keepAsking()
    {
        int position = 1;
        while (options.Any())
        {
            string choice = options.Count > 1 
                ? AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"Pick {position}. team:").AddChoices(options))
                : options.Single();

            options.Remove(choice);
            yield return choice;

            AnsiConsole.WriteLine($"{position}. {choice}");

            position++;
        }

        AnsiConsole.WriteLine("");
        Thread.Sleep(500);
    }
}
