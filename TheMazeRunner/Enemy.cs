using System;
using System.Collections;

namespace TheMazeRunner
{
    class Enemy
    {
        public static bool readyToDraw = true; 
        public int x, y, lastX, lastY;
        public char type;
        public Enemy(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.lastX = -1;
            this.lastY = -1;
            this.type = '♦';
        }
        public void move(Cell[,] map)
        {
            Cell[] cells = getNeighbours(map);
            if (cells.Length <= 0)
            {
                lastX = -1;
                lastY = -1;
            }
            else
            {
                Random r = new Random();
                int rNumber = r.Next(cells.Length);
                lastX = x;
                x = cells[rNumber].X;
                lastY = y;
                y = cells[rNumber].Y;
            }
        }
        public Cell[] getNeighbours(Cell[,] map)
        {
            Stack n = new Stack();
            if (y - 1 >= 0 && map[y - 1, x].Type != '█' && !(map[y - 1, x].X == lastX && map[y - 1, x].Y == lastY))
            {
                n.Push(map[y - 1, x]);

            }
            if (y + 1 < map.GetLength(0) && !(map[y + 1, x].X == lastX && map[y + 1, x].Y == lastY) && map[y + 1, x].Type != '█')
            {
                n.Push(map[y + 1, x]);
            }
            if (x - 1 >= 0 && !(map[y, x - 1].X == lastX && map[y, x - 1].Y == lastY) && map[y, x - 1].Type != '█')
            {
                n.Push(map[y, x - 1]);
            }
            if (x + 1 < map.GetLength(1) && !(map[y, x + 1].X == lastX && map[y, x + 1].Y == lastY) && map[y, x + 1].Type != '█')
            {
                n.Push(map[y, x + 1]);
            }
            Cell[] tab = new Cell[n.Count];
            n.CopyTo(tab, 0);
            return tab;
        }
        public void draw(Cell[,] map)
        {
            if(lastX > 0)
            {
                Console.SetCursorPosition(lastX, lastY);
                Console.ForegroundColor = map[lastY, lastX].Color;
                Console.Write(map[lastY, lastX].Type); 
            }
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(type);
            Console.SetCursorPosition(0, map.GetLength(0));
        }
        public bool didCatchPlayer(int pX, int pY, int l)
        {
            if (pX == x && pY == y)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(0, l);
                Console.Write("Zostałeś złapany przez strażnika labiryntu! Przegrałeś.");
                Console.ForegroundColor = ConsoleColor.Gray;
                return true;
            }
            return false;
        }
    }
}
