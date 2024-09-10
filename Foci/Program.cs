using System.Reflection;

namespace Foci;

class Program
{
    private static List<Match> Matches = [];
    private static int TaskNum;
    private static string StoredTeamName;

    static void Main(string[] args)
    {
        ReadDataFromFile();
        foreach (MethodInfo method in typeof(Program).GetMethods())
        {
            if (method.Name.StartsWith("Task"))
            {
                TaskNum = int.Parse(method.Name[4..]);
                method.Invoke(null, null);
            }
        }
    }

    static void ReadDataFromFile()
    {
        const string path = "meccs.txt";
        File.ReadAllLines(path)[1..].Do(CreateMatchFromLine);
    }

    static void CreateMatchFromLine(string line)
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

    static void PrintResult(string result) => Console.WriteLine($"{TaskNum}. feladat: {result}");

    static int RequestNumberInput(string message)
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

    static void Task2()
    {
        var round = RequestNumberInput("Kérem a forduló sorszámát: ");
        Matches.DoIf(m => m.Round == round, m => PrintResult($"{m.HomeTeam.Name}-{m.EnemyTeam.Name}: {m.HomeTeam.Goals}-{m.EnemyTeam.Goals} ({m.HomeTeam.HalftimeGoals}-{m.EnemyTeam.HalftimeGoals})"));
    }

    static void Task3()
    {
        Matches
            .Where(m => m.IsHalfTimeEnemyWin() && m.IsHomeWin() || m.IsHalfTimeHomeWin() && m.IsEnemyWin())
            .Select(m => $"{m.Round}. {m.GetWinnerTeam().Name}")
            .Distinct()
            .Do(PrintResult);
    }

    static void Task4()
    {
        Console.Write("Kérem a csapat nevét: ");
        StoredTeamName = Console.ReadLine() ?? string.Empty;
    }

    static void Task5()
    {
        var matches = Matches.Where(m => m.HomeTeam.Name == StoredTeamName || m.EnemyTeam.Name == StoredTeamName).ToArray();
        var goals = matches.Sum(m => m.HomeTeam.Name == StoredTeamName ? m.HomeTeam.Goals : m.EnemyTeam.Goals);
        var goalsAgainst = matches.Sum(m => m.HomeTeam.Name == StoredTeamName ? m.EnemyTeam.Goals : m.HomeTeam.Goals);
        PrintResult($"{StoredTeamName} góljainak száma: {goals}");
        PrintResult($"{StoredTeamName} kapott góljainak száma: {goalsAgainst}");
    }
}