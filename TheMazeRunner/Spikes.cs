using System;

namespace TheMazeRunner
{
    class Spikes
    {
        public static bool readyToDraw = true;
        public int x, y;
        public char type;
        public ConsoleColor color;
        public Spikes(int x, int y)
        {
            this.x = x;
            this.y = y;
            type = '▒';
            color = ConsoleColor.Red;
        }
        public void changeState()
        {
            if (color == ConsoleColor.Red)
            {
                color = ConsoleColor.White;
            }
            else
            {
                color = ConsoleColor.Red;
            }
        }
        public void draw()
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(type);
        }
        public bool isPlayerTrapped(int pX, int pY,int l)
        {
            if(pX == x && pY == y && color == ConsoleColor.Red)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(0, l);
                Console.Write("Wpadłeś w kolce! Przegrałeś.");
                Console.ForegroundColor = ConsoleColor.Gray;
                return true;
            }
            return false;
        }
    }
}
