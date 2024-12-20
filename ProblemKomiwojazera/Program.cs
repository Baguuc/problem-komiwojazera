﻿// Zapoznaj się z problemem św. Mikołaja (zwanym również problemem komiwojażera)

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