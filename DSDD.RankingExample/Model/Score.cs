namespace DSDD.RankingExample.Model;

public enum Score
{
    FIRST_PLACE,
    SECOND_PLACE,
    THIRD_PLACE,
    FOURTH_PLACE
}

public static class ScoreExtensions
{
    public static double GetGlickoScore(this Score score)
        => score switch
        {
            Score.FIRST_PLACE => 1,
            Score.SECOND_PLACE => 0.66,
            Score.THIRD_PLACE => 0.33,
            Score.FOURTH_PLACE => 0,
            _ => throw new IndexOutOfRangeException()
        };
}