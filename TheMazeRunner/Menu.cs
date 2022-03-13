using System;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TheMazeRunner
{
    static class Menu
    {
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static Cell[,] tempMap;

        public static void Main(string[] args)
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, 0xF030, 0x00000000);
                DeleteMenu(sysMenu, 0xF000, 0x00000000);
            }
            Console.CursorVisible = false;
            openMenu();
        }
        public static void openMenu()
        {
            setConsoleSize(40, 15);
            string[] options = { "Quick Play","Custom Game", "Map Editor", "Exit" };
            switch (showOptions(options, "The Maze Runner"))
            {
                case 0: 
                    Game game = new Game();
                    game.initQuickPlay(); 
                    break;
                case 1: openCustomMaps(); break;
                case 2:
                    MapEditor editor = new MapEditor();
                    editor.open();
                    break;
                case 3: Environment.Exit(0); break;
            }
        }
        public static void openCustomMaps()
        {
            Console.Clear();
            Console.SetWindowSize(40, 15);
            try
            {
                string[] files = Directory.GetFiles("..\\..\\..\\Maps");
                List<string> options = new List<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    if (Path.GetExtension(files[i]).Equals(".json"))
                    {
                        options.Add(Path.GetFileNameWithoutExtension(files[i]));
                    }
                }
                options.Add("::Back");
                if(options.Count>15) Console.SetBufferSize(40, 15 + options.Count);
                else Console.SetBufferSize(40, 15);
                int result = showOptions(options.ToArray(), "Custom Maps");
                if (result == options.Count - 1)
                {
                    openMenu();
                }
                else
                {
                    StreamReader reader = new StreamReader("..\\..\\..\\Maps\\" + options[result] + ".json");
                    string json = reader.ReadToEnd();
                    tempMap = JsonConvert.DeserializeObject<Cell[,]>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't find Maps directory. "+ e);
                openMenu();
            }
            
            Game game = new Game();
            game.loadCustomMap(tempMap);
        }
        public static int showOptions(string[] options,string title)
        {
            int chosenOption = 0;
            while (true)
            {
                Console.Clear();
                printTitle(title);
                for (int i = 0; i < options.Length; i++)
                {
                    printOption(" " + (i + 1) + ") ", ConsoleColor.Cyan, options[i] + " ");
                    if (i == chosenOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("◄");
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.S:
                        if (chosenOption < options.Length - 1)
                        {
                            chosenOption++;
                        }
                        break;
                    case ConsoleKey.W:
                        if (chosenOption > 0)
                        {
                            chosenOption--;
                        }
                        break;
                    case ConsoleKey.Enter: return chosenOption;
                }
            }
        }
        public static void printTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("           " + title);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
        }
        public static void printOption(string a, ConsoleColor ca, string b, ConsoleColor cb = ConsoleColor.Yellow)
        {
            Console.ForegroundColor = ca;
            Console.Write(a);
            Console.ForegroundColor = cb;
            Console.Write(b);
        }
        public static void setConsoleSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
    }
}