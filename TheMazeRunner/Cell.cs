using Newtonsoft.Json;
using System;

namespace TheMazeRunner
{
    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Type { get; set; }
        public ConsoleColor Color { get; set; }
        [JsonConstructor]
        public Cell(int i, int j)
        {
            this.Y = i;
            this.X = j;
            this.Type = '█';
            this.Color = ConsoleColor.Gray;
        }
        public static void heja()
        {

        }
    }
}
