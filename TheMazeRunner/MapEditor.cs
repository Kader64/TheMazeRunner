using System;
using System.IO;
using Newtonsoft.Json;

namespace TheMazeRunner
{
    class MapEditor
    {
        private int x = 0, y = 0;
        private char selectedBlock = '█';
        private ConsoleColor selectedColor = ConsoleColor.Gray;
        private Cell[,] map;
        private readonly int GUI_HEIGHT = 8;
        private readonly int GUI_WIDTH = 26;
        private ConsoleKey key;
        public MapEditor()
        {
            map = new Cell[21, 31];
        }
        public void open()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new Cell(i, j);
                    if (!(i==0 || i==map.GetLength(0)-1 || j==0 || j==map.GetLength(1)-1))
                    {
                        map[i, j].Type = ' ';
                    }
                }
            }
            drawEditor();
            loop();
        }
        private void drawEditor()
        {
            Console.Clear();
            Menu.setConsoleSize(map.GetLength(1) + GUI_WIDTH, map.GetLength(0) + GUI_HEIGHT);
            Maze.drawMap(map);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" Selected Block: ");
            Console.ForegroundColor = selectedColor;
            Console.WriteLine("'" + selectedBlock + "'");
            drawGUI();
        }
        private void drawGUI()
        {
            for (int i = 0; i <= 15; i++)
            {
                Console.SetCursorPosition(map.GetLength(1) + 3, i);
                Console.ResetColor();
                switch (i)
                {
                    case 0: Console.WriteLine("===================="); break;
                    case 1: Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" Keyboard shortcuts"); break;
                    case 2: Console.ResetColor(); Console.WriteLine("===================="); break;
                    case 3: Menu.printOption(" Space : ", ConsoleColor.Cyan, "Place block", ConsoleColor.Gray); break;
                    case 4: Menu.printOption(" 1 : ", ConsoleColor.Cyan, "Empty block", ConsoleColor.Gray); break;
                    case 5: Menu.printOption(" 2 : ", ConsoleColor.Cyan, "Wall", ConsoleColor.Gray); break;
                    case 6: Menu.printOption(" 3 : ", ConsoleColor.Cyan, "Escape", ConsoleColor.Gray); break;
                    case 7: Menu.printOption(" 4 : ", ConsoleColor.Cyan, "Door", ConsoleColor.Gray); break;
                    case 8: Menu.printOption(" 5 : ", ConsoleColor.Cyan, "Key", ConsoleColor.Gray); break;
                    case 9: Menu.printOption(" 6 : ", ConsoleColor.Cyan, "Player spawn", ConsoleColor.Gray); break;
                    case 10: Menu.printOption(" 7 : ", ConsoleColor.Cyan, "Enemy spawn", ConsoleColor.Gray); break;
                    case 11: Menu.printOption(" 8 : ", ConsoleColor.Cyan, "Spikes", ConsoleColor.Gray); break;
                    case 12: Menu.printOption(" G : ", ConsoleColor.Cyan, "Generate maze", ConsoleColor.Gray); break;
                    case 13: Menu.printOption(" R : ", ConsoleColor.Cyan, "Resize map", ConsoleColor.Gray); break;
                    case 14: Menu.printOption(" X : ", ConsoleColor.Cyan, "Save map", ConsoleColor.Gray); break;
                    case 15: Menu.printOption(" ESC : ", ConsoleColor.Cyan, "Quit", ConsoleColor.Gray); break;
                }
            }
        }
        private int resize()
        {
            Console.Clear();
            Menu.setConsoleSize(40, 15);
            Menu.printTitle("Change Map Size");
            try
            {
                Menu.printOption(" 1. ", ConsoleColor.Cyan, "Width (between 20 and "+ (Console.LargestWindowWidth- GUI_WIDTH) + ")", ConsoleColor.Yellow);
                Console.ForegroundColor = ConsoleColor.Green;
                int width = Convert.ToInt32(Console.ReadLine());
                if (width < 20 || width > Console.LargestWindowWidth - GUI_WIDTH) throw new Exception();
                if (width % 2 == 0) width--;
                Menu.printOption(" 2. ", ConsoleColor.Cyan, "Height (between 10 and " + (Console.LargestWindowHeight - GUI_HEIGHT) + ")", ConsoleColor.Yellow);
                Console.ForegroundColor = ConsoleColor.Green;
                int height = Convert.ToInt32(Console.ReadLine());
                if (height < 10 || height > Console.LargestWindowHeight - GUI_HEIGHT) throw new Exception();
                if (height % 2 == 0) height--;
                map = resizeMap(width,height);
                x = 0; y = 0;
                drawEditor();
                return 0;
            }
            catch (Exception)
            {
                drawEditor();
                return 0;
            }
        }
        private Cell[,] resizeMap(int width,int height)
        {
            Cell[,] newMap = new Cell[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i < map.GetLength(0) && j<map.GetLength(1))
                    {
                        newMap[i,j] = map[i, j];
                    }
                    else
                    {
                        newMap[i, j] = new Cell(i,j);
                        newMap[i, j].Type = ' ';
                    }
                }
            }
            return newMap;
        }
        private void generateMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].Type = '█';
                }
            }
            map = Maze.generateMap(map.GetLength(1),map.GetLength(0));
            drawEditor();
        }
        private int save()
        {
            Console.Clear();
            Menu.setConsoleSize(40, 15);
            Menu.printTitle("Save Map");
            if (!saveValidation())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" Can't save Map. (1 player spawn and");
                Console.WriteLine(" at least 1 escape on the map)");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("(Enter any key)");
                Console.ReadKey();
                drawEditor();
                return 0;
            }
            Menu.printOption(" Enter File name ", ConsoleColor.Cyan, "(0..9 and a..z)");
            Console.WriteLine();
            Console.WriteLine("(Leave empty if you want to cancel)");
            Console.ForegroundColor = ConsoleColor.Green;
            string fileName = Console.ReadLine();
            fileName = fileName.Trim();
            if (fileName.Length > 0 && fileName.Length <= 30)
            {
                for (int i = 0; i < fileName.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(fileName[i]))
                    {
                        save();
                        return 0;
                    }
                }
                string json = JsonConvert.SerializeObject(prepareMapToSave());
                string path = "..\\..\\..\\Maps\\" + fileName + ".json";
                try
                {
                    File.WriteAllText(path, json);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Map saved :)");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't save File. " + e);
                }
                System.Threading.Thread.Sleep(1500);
            }
            drawEditor();
            return 0;
        }
        private bool saveValidation()
        {
            int spawns = 0, escapes = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j].Type)
                    {
                        case '0':
                            escapes++;
                            break;
                        case '▲':
                            spawns++;
                            break;
                    }
                }
            }
            return spawns==1 && escapes>0;
        }
        private Cell[,] prepareMapToSave()
        {
            Cell[,] newMap = new Cell[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!(map[i, j].Type == ' ' && map[i, j].Color == ConsoleColor.Gray))
                    {
                        newMap[i, j] = map[i, j];
                    }
                }
            }
            return newMap;
        }
        private int replaceOld()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if(map[i,j].Type=='▲')
                    {
                        map[i, j].Color = ConsoleColor.Gray;
                        map[i, j].Type = ' ';
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = map[i, j].Color;
                        Console.Write(map[i,j].Type);
                        return 0;
                    }
                }
            }
            return -1;
        }
        private void loop()
        {
            bool running = true;
            while (running)
            {
                if (Console.KeyAvailable)
                {
                    Console.SetCursorPosition(0, map.GetLength(0));
                    key = Console.ReadKey().Key;
                    Console.SetCursorPosition(0, map.GetLength(0));
                    Console.ResetColor();
                    Console.Write(" ");
                    if (key == ConsoleKey.W || key == ConsoleKey.S || key == ConsoleKey.A || key == ConsoleKey.D)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ResetColor();
                        Console.ForegroundColor = map[y, x].Color;
                        Console.Write(map[y, x].Type);
                        switch (key)
                        {
                            case ConsoleKey.W:
                                if (y - 1 >= 0)
                                {
                                    y--;
                                }
                                break;
                            case ConsoleKey.S:
                                if (y + 1 < map.GetLength(0))
                                {
                                    y++;
                                }
                                break;
                            case ConsoleKey.A:
                                if (x - 1 >= 0)
                                {
                                    x--;
                                }
                                break;
                            case ConsoleKey.D:
                                if (x + 1 < map.GetLength(1))
                                {
                                    x++;
                                }
                                break;
                        }
                        Console.SetCursorPosition(x, y);
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(map[y, x].Type);
                    }
                    else
                    {
                        switch (key)
                        {
                            case ConsoleKey.D1:
                                selectedBlock = ' ';
                                selectedColor = ConsoleColor.Gray;
                                break;
                            case ConsoleKey.D2:
                                selectedBlock = '█';
                                selectedColor = ConsoleColor.Gray;
                                break;
                            case ConsoleKey.D3:
                                selectedBlock = '0';
                                selectedColor = ConsoleColor.Green;
                                break;
                            case ConsoleKey.D4:
                                selectedBlock = '█';
                                selectedColor = ConsoleColor.DarkYellow;
                                break;
                            case ConsoleKey.D5:
                                selectedBlock = '♠';
                                selectedColor = ConsoleColor.DarkYellow;
                                break;
                            case ConsoleKey.D6:
                                selectedBlock = '▲';
                                selectedColor = ConsoleColor.Yellow;
                                break;
                            case ConsoleKey.D7:
                                selectedBlock = '♦';
                                selectedColor = ConsoleColor.Red;
                                break;
                            case ConsoleKey.D8:
                                selectedBlock = '▒';
                                selectedColor = ConsoleColor.Red;
                                break;
                            case ConsoleKey.G:
                                generateMap();
                                break;
                            case ConsoleKey.R:
                                resize();
                                break;
                            case ConsoleKey.X:
                                save();
                                break;
                            case ConsoleKey.Escape:
                                running = false;
                                break;
                            case ConsoleKey.Spacebar:
                                if(selectedBlock == '▲') replaceOld();
                                map[y, x].Type = selectedBlock;
                                map[y, x].Color = selectedColor;
                                break;
                        }
                        Console.SetCursorPosition(18, map.GetLength(0) + 1);
                        Console.ResetColor();
                        Console.ForegroundColor = selectedColor;
                        Console.Write(selectedBlock);
                    }
                }
            }
            Menu.openMenu();
        }
    }
}
