using System;
using System.Collections.Generic;
using System.Threading;

namespace TheMazeRunner
{
    class Game
    {
        public Cell[,] map;
        public Player p;
        public List<Enemy> enemies = new List<Enemy>();
        public List<Spikes> spikes = new List<Spikes>();
        public bool running = true;
        public void initQuickPlay()
        {
            Random r = new Random();
            int width = r.Next(40) + 20;
            if (width % 2 == 0) width++;
            int height = r.Next(20) + 10;
            if (height % 2 == 0) height++;
            map = Maze.generateMap(width,height);
            enemies.Add(new Enemy(map.GetLength(1) - 2, map.GetLength(0) - 2));
            p = new Player(1, 1);
            start();
        }
        public void loadCustomMap(Cell[,] newMap)
        {
            map = newMap;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j]==null)
                    {
                        map[i, j] = new Cell(i, j);
                        map[i, j].Type = ' ';
                    }
                    else
                    {
                        switch (map[i, j].Type)
                        {
                            case '♦':
                                map[i, j].Type = ' ';
                                map[i, j].Color = ConsoleColor.Gray;
                                enemies.Add(new Enemy(j, i));
                                break;
                            case '▲':
                                map[i, j].Type = ' ';
                                map[i, j].Color = ConsoleColor.Gray;
                                p = new Player(j, i);
                                break;
                            case '▒':
                                map[i, j].Color = ConsoleColor.White;
                                spikes.Add(new Spikes(j, i));
                                break;
                        }
                    }
                }
            }
            start();
        }
        public void start()
        {
            Menu.setConsoleSize(map.GetLength(1) + 1, map.GetLength(0) + 1);
            Maze.drawMap(map);
            p.draw();
            Thread th2 = new Thread(moveEntities);
            th2.Start();
            while (running)
            {
                p.move(map);
                if (p.checkBlock(map)) break;
                foreach (Spikes spike in spikes)
                {
                    if (spike.isPlayerTrapped(p.x, p.y, map.GetLength(0))) running = false;
                }
                foreach (Enemy e in enemies)
                {
                    if (e.didCatchPlayer(p.x, p.y, map.GetLength(0))) running = false;
                }
                drawEntities();
            }
            running = false;
            Thread.Sleep(2000); 
            Menu.openMenu();
        }
        private void drawEntities()
        {
            if (Spikes.readyToDraw)
            {
                foreach (Spikes spike in spikes)
                {
                    spike.draw();
                }
                Spikes.readyToDraw = false;
            }
            if (Enemy.readyToDraw)
            {
                foreach (Enemy e in enemies)
                {
                    e.draw(map);
                }
                Enemy.readyToDraw = false;
            }
            p.draw();
        }
        private void moveEntities()
        {
            Thread.Sleep(750);
            while (running)
            {
                foreach (Enemy e in enemies)
                {   
                    e.move(map);
                    if (e.didCatchPlayer(p.x, p.y, map.GetLength(0))) running = false;
                }
                Enemy.readyToDraw = true;
                Thread.Sleep(200);
                foreach (Spikes spike in spikes)
                {
                    spike.changeState();
                    if (spike.isPlayerTrapped(p.x, p.y, map.GetLength(0))) running = false;
                }
                Spikes.readyToDraw = true;
                Thread.Sleep(800);
            }
        }
    }
}
