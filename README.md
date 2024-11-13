# Ranking Example

This repository demonstrates usage of player ranking algorithms to score debate tournaments.

## Adapted ELO

[Wiki](https://en.wikipedia.org/wiki/Elo_rating_system)

Implemented in DSDD.RankingExample.Elo.
This is adaptation of ELO algorithm for team based games.

## Glicko
[Wiki](https://en.wikipedia.org/wiki/Glicko_rating_system)

Implemented in DSDD.RankingExample.Glicko.
This is not Glicko 2 and as such implementation omits `v` (volatility) as it accounts for long term changes.
Purpose of this model is to score single tournament.

