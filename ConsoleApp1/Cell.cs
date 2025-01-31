using System;

class Cell
{
    public bool IsAlive { get; private set; }
    public Cell(bool isAlive = false) => IsAlive = isAlive;
    public override string ToString() => IsAlive ? "O" : " ";
}
