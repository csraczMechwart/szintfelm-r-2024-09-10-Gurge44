namespace Foci;

public class Match(int round, int homeGoals, int enemyGoals, int halftimeHomeGoals, int halftimeEnemyGoals, string homeTeamName, string enemyTeamName)
{
    public int Round { get; } = round;
    public Team HomeTeam { get; } = new(homeGoals, halftimeHomeGoals, homeTeamName);
    public Team EnemyTeam { get; } = new(enemyGoals, halftimeEnemyGoals, enemyTeamName);

    public Team GetWinnerTeam()
    {
        if (HomeTeam.Goals > EnemyTeam.Goals) return HomeTeam;
        if (HomeTeam.Goals < EnemyTeam.Goals) return EnemyTeam;
        return null;
    }

    public bool IsHomeWin() => HomeTeam.Goals > EnemyTeam.Goals;
    public bool IsEnemyWin() => HomeTeam.Goals < EnemyTeam.Goals;

    public bool IsHalfTimeHomeWin() => HomeTeam.HalftimeGoals > EnemyTeam.HalftimeGoals;
    public bool IsHalfTimeEnemyWin() => HomeTeam.HalftimeGoals < EnemyTeam.HalftimeGoals;
}

public class Team(int goals, int halftimeGoals, string name)
{
    public int Goals { get; set; } = goals;
    public int HalftimeGoals { get; set; } = halftimeGoals;
    public string Name { get; set; } = name;
}