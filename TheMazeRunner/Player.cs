using System;

namespace TheMazeRunner
{
    class Player
    {
        public int x, y;
        public char type;
        public ConsoleKey key;
        public int keys = 0;
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.type = '▲';
        }
        public void move(Cell[,] map)
        {
            if (Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, map.GetLength(0));
                key = Console.ReadKey().Key;
                Console.SetCursorPosition(0, map.GetLength(0));
                Console.Write(" ");
                if (key == ConsoleKey.W || key == ConsoleKey.S || key == ConsoleKey.A || key == ConsoleKey.D)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = map[y, x].Color;
                    Console.Write(map[y, x].Type);
                    switch (key)
                    {
                        case ConsoleKey.W:
                            if (y - 1 > 0 && canMoveOnBlock(map, x, y - 1))
                            {
                                y--;
                            }
                            break;
                        case ConsoleKey.S:
                            if (y + 1 < map.GetLength(0) && canMoveOnBlock(map, x, y + 1))
                            {
                                y++;
                            }
                            break;
                        case ConsoleKey.A:
                            if (x - 1 > 0 && canMoveOnBlock(map, x - 1, y))
                            {
                                x--;
                            }
                            break;
                        case ConsoleKey.D:
                            if (x + 1 < map.GetLength(1) && canMoveOnBlock(map, x + 1, y))
                            {
                                x++;
                            }
                            break;
                    }
                }
            }
        }
        private bool canMoveOnBlock(Cell[,] map,int nX,int nY)
        {
            if((map[nY, nX].Type == '█' && map[nY, nX].Color == ConsoleColor.DarkYellow && keys > 0) || map[nY, nX].Type != '█')
            {
                return true;
            }
            return false;
        }
        public void draw()
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(type);
        }
        public bool checkBlock(Cell[,] map)
        {
            switch (map[y, x].Type)
            {
                case '0':
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(0, map.GetLength(0));
                    Console.WriteLine("Gratulacje uciekłeś z labiryntu!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return true;
                case '♠':
                    map[y, x].Type = ' ';
                    map[y, x].Color = ConsoleColor.Gray;
                    keys++;
                    break;
                case '█':
                    if(map[y,x].Color == ConsoleColor.DarkYellow)
                    {
                        map[y, x].Type = ' ';
                        map[y, x].Color = ConsoleColor.Gray;
                        keys--;
                    }
                    break;
            }
            return false;
        }
    }
}
