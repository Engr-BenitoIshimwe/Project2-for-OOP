using System;
using System.IO;

class Game : IBoardOperations
{
    private BoardBase generation;
    private int steps;

    public Game(int cols, int rows, int steps, int livingCells) => (generation, this.steps) = (new Generation(cols, rows, livingCells), steps);
    public Game(int cols, int rows, int steps, double density) => (generation, this.steps) = (new Generation(cols, rows, density), steps);
    public Game(string boardStr, int steps) => (generation, this.steps) = (new Generation(boardStr), steps);

    public void Run()
    {
        for (int step = 0; step < steps; step++)
        {
            Console.Clear();
            Console.WriteLine($"Step {step + 1}");
            Console.WriteLine(generation);
            generation.GenerateNext();
            System.Threading.Thread.Sleep(300);
        }
    }

    public void SaveToFile(string filename) => File.WriteAllText(filename, generation.ToString());
    public void LoadFromFile(string filename) => generation = new Generation(File.ReadAllText(filename));
}
