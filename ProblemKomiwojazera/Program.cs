// Zapoznaj się z problemem św. Mikołaja (zwanym również problemem komiwojażera)

// Napisz program którzy przyjmie listę 50 współrzędnych gdzie -100 < x <100, -100 < y <100 jako plik lub tekst wklejony do kontrolki.

// Dokonaj deserializacji JSONa i oblicz w dowolny sposób jakąkolwiek trasę (nie musi być najkrótsza)

// Wyświetl na ekranie:

// Całkowitą długość trasy
// Kolejność odwiedzonych punktów
// (opcjonalnie) czas wykonania obliczeń
using System.Text.Json;

class Coordinate
{
    public float x { get; set; }
    public float y { get; set; }

    public string ToString()
    {
        return $"({this.x}, {this.y})";
    }
}

class Distance
{
    public Coordinate from { get; }
    public Coordinate to { get; }
    public float xLen { get; }
    public float yLen { get; }
    public float length { get; }

    public static Distance Calculate(Coordinate from, Coordinate to)
    {
        float xLen = Distance.AbsValue(from.x - to.x);
        float yLen = Distance.AbsValue(from.y - to.y);
        // obliczamy dlugosc z twierdzenia pitagorasa
        float length = MathF.Sqrt(MathF.Pow(xLen, 2) + MathF.Pow(yLen, 2));

        return new Distance(from, to, xLen, yLen, length);
    }

    private static float AbsValue(float value)
    {
        return value >= 0
            ? value
            : -value;
    }
    private Distance(Coordinate from, Coordinate to, float xLen, float yLen, float length)
    {
        this.xLen = xLen;
        this.yLen = yLen;
        this.length = length;
    }
}

class Path
{
    public List<Coordinate> nodes { get; }
    public float totalLength { get; }


    public static Path Calculate(List<Coordinate> nodes)
    {
        float totalLength = 0;
        for(int i = 0; i < nodes.Count - 2; i++)
        {
            Distance distance = Distance.Calculate(nodes[i], nodes[i + 1]);
            totalLength += distance.length;
        }

        return new Path(nodes, totalLength);
    }

    public string ToString()
    {
        string nodesStr = "";
        for(int i = 0; i < this.nodes.Count; i++)
        {
            Coordinate node = this.nodes[i];
            nodesStr += $"{i+1}. {node.ToString()}\n";
        }

        return $"Dlugosc sciezki: {this.totalLength}\nKolejnosc odwiedzonych punktow:\n{nodesStr}";
    }

    private Path(List<Coordinate> nodes, float totalLength)
    {
        this.nodes = nodes;
        this.totalLength = totalLength;
    }
}

class Util
{
    // L reprezentuje typ oryginalnego obiektu w liscie
    // T reprezentuje typ uzyskiwany po "transformacji" obiektu uzyty do porownania
    public static List<L> SortList<L, T>(List<L> list, Func<L, T> transform)
        where T : IComparable
    {
        L[] arr = list.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 1; j < i + 1; j++)
            {
                T arrI = transform(arr[i]);
                T arrJ = transform(arr[j]);

                // jesli metoda CompareTo zwraca 1 lub wiecej znaczy to ze ten przedmiot jest wiekszy
                if (arrJ.CompareTo(arrI) >= 1)
                {
                    L temp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = temp;
                }
            }
        }

        return arr.ToList();
    }
}


class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Podaj sciezke do pliku z danymi.");
        string? input = Console.ReadLine();
        if (input == null)
        {
            Console.WriteLine("Musisz podac sciezke pliku.");
            Program.Main(args);
            return;
        }

        string content;
        try
        {
            content = File.ReadAllText(input);
        }
        catch (Exception)
        {
            Console.WriteLine("Nie mozna odczytac pliku.");
            Program.Main(args);
            return;
        }
        List<Coordinate> coordinates = JsonSerializer.Deserialize<List<Coordinate>>(content) ?? new List<Coordinate>();

        if(coordinates.Count < 2)
        {
            Console.WriteLine("Za malo punktow zeby stworzy jakakolwiek sciezke.");
            Program.Main(args);
            return;
        }
    }
}