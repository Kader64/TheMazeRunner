using System;
using System.Collections;
using System.Collections.Generic;

namespace TheMazeRunner
{
    static class Maze
    {
        public static Stack cells = new Stack();
        private static List<Cell> visitedCells = new List<Cell>();
        public static Cell[,] generateMap(int width, int height)
        {
            Cell[,] map = new Cell[height, width];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new Cell(i,j);
                }
            }
            Cell currentCell = map[1, 1];
            cells.Push(currentCell);
            visitedCells.Add(currentCell);
            currentCell.Type = ' ';
            while (cells.Count != 0)
            {
                currentCell = (Cell)cells.Pop();
                Cell[] neighbours = getNeighbours(currentCell,map);
                if (neighbours.Length > 0)
                {
                    cells.Push(currentCell);
                    Random r = new Random();
                    Cell rNeighbour = neighbours[r.Next(neighbours.Length)];
                    map[(currentCell.Y - (currentCell.Y - rNeighbour.Y) / 2), (currentCell.X - (currentCell.X - rNeighbour.X) / 2)].Type = ' ';
                    rNeighbour.Type = ' ';
                    visitedCells.Add(rNeighbour);
                    cells.Push(rNeighbour);
                }
            }
            map[map.GetLength(0) - 2, map.GetLength(1) - 2].Color = ConsoleColor.Green;
            map[map.GetLength(0) - 2, map.GetLength(1) - 2].Type = '0';
            return map;
        }
        private static Cell[] getNeighbours(Cell currentCell,Cell[,] map)
        {
            Stack n = new Stack();
            if (currentCell.Y - 2 >= 0 && visitedCells.Contains(map[currentCell.Y - 2, currentCell.X]) == false)
            {
                n.Push(map[currentCell.Y - 2, currentCell.X]);
            }
            if (currentCell.Y + 2 < map.GetLength(0) && visitedCells.Contains(map[currentCell.Y + 2, currentCell.X]) == false)
            {
                n.Push(map[currentCell.Y + 2, currentCell.X]);
            }
            if (currentCell.X - 2 >= 0 && visitedCells.Contains(map[currentCell.Y, currentCell.X - 2]) == false)
            {
                n.Push(map[currentCell.Y, currentCell.X - 2]);
            }
            if (currentCell.X + 2 < map.GetLength(1) && visitedCells.Contains(map[currentCell.Y, currentCell.X + 2]) == false)
            {
                n.Push(map[currentCell.Y, currentCell.X + 2]);
            }
            Cell[] tab = new Cell[n.Count];
            n.CopyTo(tab, 0);
            return tab;
        }
        public static void drawMap(Cell[,] map)
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.ForegroundColor = map[i, j].Color;
                    Console.Write(map[i, j].Type);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, map.GetLength(0));
        }
    }
}
