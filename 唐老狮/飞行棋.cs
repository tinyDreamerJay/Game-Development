using System.Linq.Expressions;

namespace 飞行棋
{
    enum E_scene
    {
        begin,
        game,
        end,
    }
    enum E_gridType
    {
        normal,
        pause,
        bomb,
        tunnel,
    }
    enum E_playerType
    {
        player,
        computer,
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 游戏初始化
            int w = 50; // 控制台宽度
            int h = 30; // 控制台高度    
            ConsoleInit(w, h);
            #endregion
            E_scene nowSceneType = E_scene.begin;
            while (true)
            {
                switch (nowSceneType)
                {
                    case E_scene.begin:
                        Console.Clear();
                        beginScence(w, h, ref nowSceneType);
                        break;
                    case E_scene.game:
                        Console.Clear();
                        gameScence(w, h,ref nowSceneType);
                        break;
                    case E_scene.end:
                        Console.Clear();
                        endScence(w, h, ref nowSceneType);
                        break;
                    default:
                        break;
                }
            }
        }
        #region 控制台初始化
        static void ConsoleInit(int w, int h)
        {
            Console.CursorVisible = false;// 隐藏光标 
            Console.SetWindowSize(w, h); // 设置窗口大小
            Console.SetBufferSize(w, h);
        }
        #endregion
        #region 开始场景
        static void beginScence(int w, int h, ref E_scene nowScenceType)
        {
            Console.SetCursorPosition(w / 2 - 3, 8);
            Console.Write("飞行棋");
            int nowSelIndex = 0;
            bool isQuitBegin = false;
            while (true)
            {
                Console.SetCursorPosition(w / 2 - 4, 12);
                Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("开始游戏");
                Console.SetCursorPosition(w / 2 - 4, 14);
                Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("结束游戏");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        nowSelIndex--;
                        if (nowSelIndex < 0)
                        {
                            nowSelIndex = 0;
                        }
                        break;
                    case ConsoleKey.S:
                        nowSelIndex++;
                        if (nowSelIndex > 1)
                        {
                            nowSelIndex = 1;
                        }
                        break;
                    case ConsoleKey.J:
                        if (nowSelIndex == 0)
                        {
                            isQuitBegin = true;
                            nowScenceType = E_scene.game;
                        }
                        else if (nowSelIndex == 1)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
                if (isQuitBegin)
                {
                    break;
                }
            }
        }
        #endregion
        #region 游戏场景
        static void gameScence(int w, int h,ref E_scene nowSceneType)
        {
            DrawWall(w, h);
            int row = 14;
            int column = 3;
            int gridNum = 80; //格子数量
            Map map = new Map(row, column, gridNum);
            map.Draw();
            Player player = new Player(0, E_playerType.player);
            Player computer = new Player(0, E_playerType.computer);
            DrawPlayer(player, computer, map);
            randomMove(w, h, ref player,ref computer, map);
            bool isEnd = false;
            while (true)
            {
                Console.ReadKey(true);
                isEnd = randomMove(w, h, ref player, ref computer, map);
                map.Draw();
                DrawPlayer(player, computer, map);
                if (isEnd)
                {
                    Console.ReadKey(true);
                    nowSceneType = E_scene.end;
                    break;
                }
                Console.ReadKey(true);
                isEnd = randomMove(w, h, ref computer, ref player, map);
                map.Draw();
                DrawPlayer(player, computer, map);
                if (isEnd)
                {
                   Console.ReadKey(true);
                    nowSceneType = E_scene.end;
                    break;
                }
            }
        }
        #region 画墙
        static void DrawWall(int w, int h)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < w; i += 2)
            {

                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, h - 1);
                Console.Write("■");
                Console.SetCursorPosition(i, h - 6);
                Console.Write("■");
                Console.SetCursorPosition(i, h - 11);
                Console.Write("■");
            }
            for (int i = 0; i < h; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(w - 2, i);
                Console.Write("■");
            }
            Console.SetCursorPosition(2, h - 10);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("□:普通格子");
            Console.SetCursorPosition(2, h - 9);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("‖:暂停，一回合不懂");
            Console.Write("      ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("●:炸弹，倒退5格");
            Console.SetCursorPosition(2, h - 8);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("¤:时空隧道，随机倒退，暂停，换位置");
            Console.SetCursorPosition(2, h - 7);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("★:玩家");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(12, h - 7);
            Console.Write("▲:电脑");
            Console.SetCursorPosition(22, h - 7);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("◎:玩家和电脑重合");
            Console.SetCursorPosition(2, h - 5);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("按任意键扔色子");
        }
        #endregion
        #region 格子
        struct Vector2
        {
            public int x;
            public int y;
            public Vector2(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        struct Grid
        {
            public Vector2 pos;
            public E_gridType type;
            public Grid(int x, int y, E_gridType type)
            {
                pos.x = x;
                pos.y = y;
                this.type = type;
            }
            public void Draw()
            {
                switch (type)
                {
                    case E_gridType.normal:
                        Console.SetCursorPosition(pos.x, pos.y);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("□");
                        break;
                    case E_gridType.pause:
                        Console.SetCursorPosition(pos.x, pos.y);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("‖");
                        break;
                    case E_gridType.bomb:
                        Console.SetCursorPosition(pos.x, pos.y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("●");
                        break;
                    case E_gridType.tunnel:
                        Console.SetCursorPosition(pos.x, pos.y);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("¤");
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        #region 地图
        struct Map
        {
            public Grid[] grids;
            public Map(int x, int y, int num)
            {
                Random r = new Random();
                grids = new Grid[num];
                int randomNum;
                int indexX = 0;
                int indexY = 0;
                int stepNum = 2;
                for (int i = 0; i < num; i++)
                {
                    randomNum = r.Next(0, 101);
                    if (randomNum < 85 || i == 0 || i == num - 1)
                    {
                        grids[i].type = E_gridType.normal;
                    }
                    else if (randomNum >= 85 && randomNum < 90)
                    {
                        grids[i].type = E_gridType.pause;
                    }
                    else if (randomNum >= 90 && randomNum < 95)
                    {
                        grids[i].type = E_gridType.bomb;
                    }
                    else if (randomNum >= 95 && randomNum < 101)
                    {
                        grids[i].type = E_gridType.tunnel;
                    }
                    grids[i].pos = new Vector2(x, y);
                    if (indexX == 10)
                    {
                        indexY++;
                        y += 1;
                        if (indexY == 2)
                        {
                            indexX = 0;
                            indexY = 0;
                            stepNum = -stepNum;
                        }
                    }
                    else
                    {
                        x += stepNum;
                        indexX++;
                    }
                }
            }
            public void Draw()
            {
                for (int i = 0; i < grids.Length; i++)
                {
                    grids[i].Draw();
                }
            }
        }
        #endregion
        #region 玩家
        struct Player
        {
            public int nowIndex;
            public bool isPause;
            public E_playerType type;
            public Player(int nowIndex, E_playerType type)
            {
                this.nowIndex = nowIndex;
                this.type = type;
                isPause = false;
            }
            public void Draw(Map mapinfo)
            {
                Grid grid = mapinfo.grids[nowIndex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                switch (type)
                {
                    case E_playerType.player:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("★");
                        break;
                    case E_playerType.computer:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("▲");
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        #region 绘制玩家
        static void DrawPlayer(Player player,Player computer,Map map)
        {   

            if (player.nowIndex==computer.nowIndex)
            {
                Grid grid = map.grids[player.nowIndex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("◎");
            }
            else
            {
                player.Draw(map);
                computer.Draw(map);
            }
        }
        #endregion
        #region 扔色子
        static void ClearInfo(int h)
        {
            Console.SetCursorPosition(2, h - 5);
            Console.Write("                                 ");
            Console.SetCursorPosition(2, h - 4);
            Console.Write("                                 ");
            Console.SetCursorPosition(2, h - 3);
            Console.Write("                                 ");
            Console.SetCursorPosition(2, h - 2);
            Console.Write("                                 ");
        }
        static bool randomMove(int w, int h, ref Player p,ref Player player2, Map map)
        {   
            ClearInfo(h);
            Console.ForegroundColor = p.type == E_playerType.player ? ConsoleColor.Cyan : ConsoleColor.Magenta;
            if (p.isPause)
            {   
                Console.SetCursorPosition(2, h - 5);
                Console.WriteLine("{0}处于暂停点，需要暂停一回合", p.type == E_playerType.player?"你":"电脑");
                p.isPause = false;
                return false;
            }
            Random r = new Random();
            int randomNum = r.Next(1, 7);
            p.nowIndex += randomNum;
            Console.SetCursorPosition(2, h - 5);
            Console.WriteLine("{0}扔出了点数为:{1}", p.type == E_playerType.player ? "你" : "电脑", randomNum);
            if (p.nowIndex >= map.grids.Length - 1)
            {   
                p.nowIndex = map.grids.Length - 1;
                Console.SetCursorPosition(2, h - 4);
                Console.Write("{0}走到了终点", p.type == E_playerType.player ? "你" : "电脑");
                Console.SetCursorPosition(2, h - 3);
                Console.Write("请按任意键结束游戏");
                return true;
            }
            else
            {
                Grid grid = map.grids[p.nowIndex];
                switch (grid.type)
                {
                    case E_gridType.normal:
                        Console.SetCursorPosition(2, h - 4);
                        Console.WriteLine("{0}走到了安全位置", p.type == E_playerType.player ? "你" : "电脑");
                        Console.SetCursorPosition(2, h - 3);
                        Console.WriteLine("请按任意键让{0}扔色子", p.type == E_playerType.player ? "电脑" : "你");
                        break;
                    case E_gridType.pause:
                        p.isPause = true;
                        break;
                    case E_gridType.bomb:
                        p.nowIndex -= 5;
                        if (p.nowIndex < 0)
                        {
                            p.nowIndex = 0;
                        }
                        Console.SetCursorPosition(2, h - 4);
                        Console.WriteLine("{0}出发了炸弹,倒退5格", p.type == E_playerType.player ? "你" : "电脑");
                        Console.SetCursorPosition(2, h - 3);
                        Console.WriteLine("请按任意键让{0}扔色子", p.type == E_playerType.player ? "电脑" : "你");
                        break;
                    case E_gridType.tunnel:
                        Random random = new Random();
                        int randomIndex = random.Next(1, 91);
                        Console.SetCursorPosition(2, h - 4);
                        Console.WriteLine("{0}触发了时空隧道", p.type == E_playerType.player ? "你" : "电脑");
                        Console.SetCursorPosition(2, h - 2);
                        Console.WriteLine("请按任意键让{0}扔色子", p.type == E_playerType.player ? "电脑" : "你");
                        if (randomIndex < 30)
                        {
                            p.nowIndex -= 5;
                            if (p.nowIndex<0)
                            {
                                p.nowIndex = 0;
                            }
                            Console.SetCursorPosition(2, h - 3);
                            Console.WriteLine("不豪是炸弹,{0}倒退5格", p.type == E_playerType.player ? "你" : "电脑");
                        }
                        else if (randomIndex>=30&&randomIndex<60)
                        {
                            int tmp = p.nowIndex;
                            p.nowIndex = player2.nowIndex;
                            player2.nowIndex = tmp;
                            Console.SetCursorPosition(2, h - 3);
                            Console.Write("{0}和{1}交换了位置", p.type == E_playerType.player ? "你" : "电脑", player2.type == E_playerType.player ? "你" : "电脑");
                        }
                        else
                        {
                            p.isPause = true;
                            Console.SetCursorPosition(2, h - 3);
                            Console.WriteLine("{0}遇到了东北雨姐,被硬控了一回合", p.type == E_playerType.player ? "你" : "电脑");
                        }

                        break;
                    default:
                        break;
                }
            }
            return false;
        }
        #endregion
        #endregion
        #region 结束场景
        static void endScence(int w,int h, ref E_scene nowSceneType)
        {
            Console.SetCursorPosition(w / 2 - 4, 8);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("游戏结束");
            int nowSelIndex = 0;
            while (true)
            {
                Console.SetCursorPosition(w / 2 - 4, 12);
                Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("重新开始");
                Console.SetCursorPosition(w / 2 - 4, 14);
                Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("结束游戏");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        nowSelIndex--;
                        if (nowSelIndex < 0)
                        {
                            nowSelIndex = 0;
                        }
                        break;
                    case ConsoleKey.S:
                        nowSelIndex++;
                        if (nowSelIndex > 1)
                        {
                            nowSelIndex = 1;
                        }
                        break;
                    case ConsoleKey.J:
                        if (nowSelIndex == 0)
                        {
                            nowSceneType = E_scene.game;
                        }
                        else if (nowSelIndex == 1)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
