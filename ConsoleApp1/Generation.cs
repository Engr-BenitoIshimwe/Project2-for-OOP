using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Generation : BoardBase
{
    private int cols, rows;
    private Cell[,] board;

    public Generation(int cols, int rows, int livingCells) : this(cols, rows)
    {
        Random rand = new Random();
        var indices = Enumerable.Range(0, cols * rows).OrderBy(_ => rand.Next()).Take(livingCells).ToList();
        InitializeBoard(indices);
    }

    public Generation(int cols, int rows, double density) : this(cols, rows)
    {
        int livingCells = (int)(cols * rows * density);
        Random rand = new Random();
        var indices = Enumerable.Range(0, cols * rows).OrderBy(_ => rand.Next()).Take(livingCells).ToList();
        InitializeBoard(indices);
    }

    public Generation(string boardStr)
    {
        var lines = boardStr.Split('\n');
        rows = lines.Length;
        cols = lines[0].Length;
        board = new Cell[rows, cols];
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                board[y, x] = new Cell(lines[y][x] == 'O');
    }

    private Generation(int cols, int rows)
    {
        this.cols = cols;
        this.rows = rows;
        board = new Cell[rows, cols];
    }

    private void InitializeBoard(List<int> indices)
    {
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                board[y, x] = new Cell(indices.Contains(y * cols + x));
    }

    public override void GenerateNext()
    {
        Cell[,] newBoard = new Cell[rows, cols];
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                newBoard[y, x] = new Cell(WillBeAlive(x, y));
        board = newBoard;
    }

    private bool WillBeAlive(int x, int y)
    {
        int aliveNeighbors = CountAliveNeighbors(x, y);
        return board[y, x].IsAlive ? aliveNeighbors == 2 || aliveNeighbors == 3 : aliveNeighbors == 3;
    }

    private int CountAliveNeighbors(int x, int y)
    {
        int count = 0;
        int[] directions = { -1, 0, 1 };
        foreach (int dy in directions)
            foreach (int dx in directions)
                if (!(dx == 0 && dy == 0) && InBounds(x + dx, y + dy) && board[y + dy, x + dx].IsAlive)
                    count++;
        return count;
    }

    private bool InBounds(int x, int y) => x >= 0 && x < cols && y >= 0 && y < rows;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < rows; y++)
        {
            sb.AppendLine(string.Concat(Enumerable.Range(0, cols).Select(x => board[y, x].ToString())));
        }
        return sb.ToString();
    }

    public static bool operator ==(Generation a, Generation b) => a.ToString() == b.ToString();
    public static bool operator !=(Generation a, Generation b) => !(a == b);
}
