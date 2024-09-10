namespace Foci;

internal static class Program
{
    private static readonly List<Match> Matches = [];
    private static int TaskNum;
    private static string? StoredTeamName;

    private static void Main()
    {
        ReadDataFromFile();
        TaskNum = 2;
        Task2();
        TaskNum = 3;
        Task3();
        TaskNum = 4;
        Task4();
        TaskNum = 5;
        Task5();
        TaskNum = 6;
        Task6();
        TaskNum = 7;
        Task7();
    }

    private static void ReadDataFromFile()
    {
        const string path = "meccs.txt";
        File.ReadAllLines(path)[1..].Do(CreateMatchFromLine);
    }

    private static void CreateMatchFromLine(string line)
    {
        var parts = line.Split(' ');
        var match = new Match(
            int.Parse(parts[0]),
            int.Parse(parts[1]),
            int.Parse(parts[2]),
            int.Parse(parts[3]),
            int.Parse(parts[4]),
            parts[5],
            parts[6]
        );
        Matches.Add(match);
    }

    private static void PrintResult(string result) => Console.WriteLine($"{TaskNum}. feladat: {result}");

    private static int RequestNumberInput(string message)
    {
        Console.Write(message);
        var input = Console.ReadLine();
        while (!int.TryParse(input, out _))
        {
            Console.WriteLine("Nem számot adott meg!");
            Console.Write(message);
            input = Console.ReadLine();
        }

        return int.Parse(input);
    }

    private static void Task2()
    {
        var round = RequestNumberInput("Kérem a forduló sorszámát: ");
        Matches.DoIf(m => m.Round == round, m => PrintResult($"{m.HomeTeam.Name}-{m.EnemyTeam.Name}: {m.HomeTeam.Goals}-{m.EnemyTeam.Goals} ({m.HomeTeam.HalftimeGoals}-{m.EnemyTeam.HalftimeGoals})"));
    }

    private static void Task3()
    {
        Matches
            .Where(m => m.IsHalfTimeEnemyWin() && m.IsHomeWin() || m.IsHalfTimeHomeWin() && m.IsEnemyWin())
            .Select(m => $"{m.Round}. {m.GetWinnerTeam().Name}")
            .Distinct()
            .Do(PrintResult);
    }

    private static void Task4()
    {
        Console.Write("Kérem a csapat nevét: ");
        StoredTeamName = Console.ReadLine() ?? string.Empty;
    }

    private static void Task5()
    {
        var matches = Matches.Where(m => m.HomeTeam.Name == StoredTeamName || m.EnemyTeam.Name == StoredTeamName).ToArray();
        var goals = matches.Sum(m => m.HomeTeam.Name == StoredTeamName ? m.HomeTeam.Goals : m.EnemyTeam.Goals);
        var goalsAgainst = matches.Sum(m => m.HomeTeam.Name == StoredTeamName ? m.EnemyTeam.Goals : m.HomeTeam.Goals);
        PrintResult($"{StoredTeamName} góljainak száma: {goals}");
        PrintResult($"{StoredTeamName} kapott góljainak száma: {goalsAgainst}");
    }

    private static void Task6()
    {
        var match = Matches.OrderBy(x => x.Round).FirstOrDefault(x => x.HomeTeam.Name == StoredTeamName && x.IsEnemyWin());
        PrintResult(match == null ? "A csapat otthon veretlen maradt." : $"Az első vereség a(z) {match.Round}. fordulóban következett be {match.EnemyTeam.Name} csapata ellen.");
    }

    private static void Task7()
    {
        Dictionary<(int Higher, int Lower), int> scoreDifferences = [];
        Matches.Do(m =>
        {
            var scores = new[] { m.EnemyTeam.Goals, m.HomeTeam.Goals };
            var key = (scores.Max(), scores.Min());
            if (!scoreDifferences.TryAdd(key, 1)) scoreDifferences[key] += 1;
        });
        File.WriteAllLines("stat.txt", scoreDifferences.Select(x => $"{x.Key.Higher}-{x.Key.Lower}: {x.Value}"));
    }
}