namespace Foci;

class Program
{
    private static List<Match> Matches = [];
    private static int TaskNum;
    
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
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

    static void PrintResult(string result)
    {
        TaskNum++;
        Console.WriteLine($"{TaskNum}. feladat: {result}");
    }
    
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
}