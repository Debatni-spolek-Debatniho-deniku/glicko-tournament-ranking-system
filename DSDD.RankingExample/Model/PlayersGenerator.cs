using System.Text;

namespace DSDD.RankingExample.Model;

public class PlayersGenerator
{
    public static IEnumerable<Player> Generate(int count)
        => GenerateAlphabetSequence(count)
            .Select(name => name.ToUpper())
            .Select(name => new Player(name));

    private static IEnumerable<string> GenerateAlphabetSequence(int count)
    {
        var alphabet = "abcdefghijklmnopqrstuvwxyz";
        int alphabetLength = alphabet.Length;

        for (int i = 0; i < count; i++)
            yield return GetLabel(i);

        string GetLabel(int index)
        {
            StringBuilder label = new StringBuilder();

            while (index >= 0)
            {
                label.Insert(0, alphabet[index % alphabetLength]);
                index = (index / alphabetLength) - 1;
            }

            return label.ToString();
        }
    }
}