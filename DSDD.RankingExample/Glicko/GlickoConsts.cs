namespace DSDD.RankingExample.Glicko;

public static class GlickoConsts
{
    public const double InitialRating = 1500;
    
    public const double InitialRatingDeviation = 100;

    public static double Q = Math.Log(10) / 400;
}