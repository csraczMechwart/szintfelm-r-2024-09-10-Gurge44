namespace Foci;

public class Match(int round, int homeGoals, int enemyGoals, int halftimeHomeGoals, int halftimeEnemyGoals, string homeTeamName, string enemyTeamName)
{
    public int Round { get; set; } = round;
    public int HomeGoals { get; set; } = homeGoals;
    public int EnemyGoals { get; set; } = enemyGoals;
    public int HalftimeHomeGoals { get; set; } = halftimeHomeGoals;
    public int HalftimeEnemyGoals { get; set; } = halftimeEnemyGoals;
    public string HomeTeamName { get; set; } = homeTeamName;
    public string EnemyTeamName { get; set; } = enemyTeamName;
}