using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum HorizontalDirection
{
    Left = 4,
    Right = 8
}

public enum VerticalDirection
{
    Up = 1,
    Down = 2
}

[Flags]
public enum MultiDirection
{
    Stationary = 0,
    Up = 1,
    Down = 2,
    Left = 4, 
    Right = 8
}