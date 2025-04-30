using System.Diagnostics;

namespace my_second_program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int wideth = 50;
            int height = 30;
            Console.SetWindowSize(wideth, height);
            Console.SetBufferSize(wideth, height);
            Console.CursorVisible = false;
            int nowScencID = 0;
            int index = 0;

            while (true)
            {   
                bool scencechange = false;
                switch (nowScencID)
                {
                    #region begin
                    case 0:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(wideth / 2 - 6, 8);
                        Console.WriteLine("王子营救公主");
                        Console.SetCursorPosition(wideth / 2 - 2, 11);
                        Console.ForegroundColor = index == 0 ? ConsoleColor.Red : ConsoleColor.White;
                        Console.Write("begin");
                        Console.SetCursorPosition(wideth / 2 - 1, 12);
                        Console.ForegroundColor = index == 1 ? ConsoleColor.Red : ConsoleColor.White;
                        Console.Write("exit");
                        char action = Console.ReadKey(true).KeyChar;
                        switch (action)
                        {
                            case 'w':
                            case 'W':
                                --index;
                                if (index < 0)
                                {
                                    index = 0;
                                }
                                break;
                            case 's':
                            case 'S':
                                ++index;
                                if (index > 1)
                                {
                                    index = 1;
                                }
                                break;
                            case 'j':
                            case 'J':
                                if (index == 0)
                                {
                                    Console.Clear();
                                    nowScencID = 1;
                                }
                                else
                                {
                                    Environment.Exit(0);
                                }
                                break;
                        }
                        break;
                    #endregion
                    case 1:
                        #region red_Wall
                        for (int i = 0; i < wideth; i+=2)
                        {
                            Console.SetCursorPosition(i, height - 7);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■");
                            Console.SetCursorPosition(i, height - 1);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■");
                            Console.SetCursorPosition(i, 0);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■");
                        }
                        for (int i = 0; i < height; i++)
                        {
                            Console.SetCursorPosition(0, i);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■");
                            Console.SetCursorPosition(wideth - 2, i);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■");
                        }
                        #endregion
                        #region boss princess and player
                        int bosshp = 100;
                        int bossX = 24;
                        int bossY = 14;
                        int bossAtkMin = 5;
                        int bossAtkMax = 7;
                        string bossIcon = "■";
                        ConsoleColor bosscolor = ConsoleColor.Green;
                        int playerHp = 100;
                        int playerX = 10;
                        int playerY = 10;
                        int playerAtkMin = 4;
                        int playerAtkMax = 8; 
                        string playerIcon = "●";
                        char player_move;
                        ConsoleColor playercolor = ConsoleColor.Yellow;
                        int princessX = 24;
                        int princessY = 5;
                        ConsoleColor princessColor = ConsoleColor.Magenta;
                        string princessIcon = "★";
                        #endregion
                        #region game_playing
                        char playerInPut;
                        bool isFight = false;
                        bool scenceChange = false;
                        while (true)
                        {   if (scenceChange) break;
                            if (bosshp > 0)
                            {
                                Console.ForegroundColor = bosscolor;
                                Console.SetCursorPosition(bossX, bossY);
                                Console.Write(bossIcon);
                            }
                            else
                            {   

                                Console.SetCursorPosition(princessX, princessY);
                                Console.ForegroundColor = princessColor;
                                Console.Write(princessIcon);
                            }
                            
                            if (isFight)
                            {   Console.ForegroundColor = playercolor;
                                Console.SetCursorPosition(playerX, playerY);
                                Console.Write(playerIcon);
                                if (bosshp > 0)
                                {
                                    if (playerHp < 0)
                                    {
                                        Console.SetCursorPosition(3, height - 6);
                                        Console.Write("                                        ");
                                        Console.SetCursorPosition(3, height - 5);
                                        Console.Write("                                     ");
                                        Console.SetCursorPosition(3, height - 4);
                                        Console.Write("                                       ");
                                        Console.SetCursorPosition(3, height - 5);
                                        Console.Write("你死亡，死因被boss干死了。");
                                        nowScencID = 2;
                                        scenceChange = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.SetCursorPosition(bossX, bossY);
                                    Console.Write("  ");
                                    Console.SetCursorPosition(3,height - 6);
                                    Console.Write("                                        ");
                                    Console.SetCursorPosition(3, height - 5);
                                    Console.Write("                                     ");
                                    Console.SetCursorPosition(3, height - 4);
                                    Console.Write("                                       ");
                                    Console.SetCursorPosition(3, height - 5);
                                    Console.Write("你战胜了boss，快去营救公主吧。");
                                    isFight = false;
                                    continue;
                                }
                                Console.SetCursorPosition(3, height - 6);
                                Console.Write("开始战斗了！！！按j键继续");
                                playerInPut = Console.ReadKey(true).KeyChar;
                                Console.SetCursorPosition(3, height - 5);
                                Console.Write("                                     ");
                                Console.SetCursorPosition(3, height - 4);
                                Console.Write("                                       ");
                                if (playerInPut == 'j' || playerInPut == 'J')
                                {
                                    Random r = new Random();
                                    int atk = r.Next(playerAtkMin, playerAtkMax);
                                    bosshp -= atk;
                                    Console.SetCursorPosition(3, height - 5);
                                    Console.Write("你对boss造成了{0}伤害,boss剩余血量{1}", atk, bosshp);
                                    atk = r.Next(bossAtkMin, bossAtkMax);
                                    playerHp -= atk;
                                    Console.SetCursorPosition(3, height - 4);
                                    Console.Write("boss对你造成了{0}伤害,你剩余血量{1}", atk, playerHp);
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(playerX, playerY);
                                Console.ForegroundColor = playercolor;
                                Console.Write(playerIcon);
                                player_move = Console.ReadKey(true).KeyChar;
                                Console.SetCursorPosition(playerX, playerY);
                                Console.Write("  ");
                                switch (player_move)
                                {
                                    case 'W':
                                    case 'w':
                                        --playerY;
                                        if (playerY <= 2)
                                        {
                                            playerY = 1;
                                        }
                                        if (playerX == bossX && playerX == bossX)
                                        {
                                            playerX += 1;
                                        }
                                        if (playerX == princessX && playerY == princessY && bosshp < 0)
                                        {
                                            playerY += 1;
                                        }
                                        break;
                                    case 'S':
                                    case 's':
                                        ++playerY;
                                        if (playerY >= height - 7)
                                        {
                                            playerY = height - 8;
                                        }
                                        if (playerY == bossY && playerX == bossX)
                                        {
                                            playerY -= 1;
                                        }
                                        if (playerY == princessY& playerX == princessX && bosshp <= 0)
                                        {
                                            playerY -= 1;
                                        }
                                        break;
                                    case 'A':
                                    case 'a':
                                        playerX -= 2;
                                        if (playerX < 2)
                                        {
                                            playerX = 2;
                                        }
                                        if (playerX == bossX && playerY == bossY)
                                        {
                                            playerX += 2;
                                        }
                                        if (playerX == princessX && playerY == princessY && bosshp <= 0)
                                        {
                                            playerX +=2;
                                        }
                                        break;
                                    case 'D':
                                    case 'd':
                                        playerX += 2;
                                        if (playerX > wideth - 4)
                                        {
                                            playerX = wideth - 4;
                                        }
                                        if (playerX == bossX && playerY == bossY)
                                        {
                                            playerX -= 2;
                                        }
                                        if(playerX == princessX && playerY == princessY && bosshp <=0)
                                        {
                                            playerX -= 2;
                                        }
                                        break;
                                    case 'j':
                                    case 'J':
                                        if ((playerX == bossX && playerY == bossY - 1 ||
                                           playerX == bossX && playerY == bossY + 1 ||
                                           playerX == bossX + 2 && playerY == bossY ||
                                           playerX == bossX - 2 && playerY == bossY) && bosshp > 0)
                                        {
                                            isFight = true;
                                        }
                                        else if((playerX == princessX && playerY == princessY - 1 ||
                                           playerX == princessX && playerY == princessY + 1 ||
                                           playerX == princessX + 2 && playerY == princessY ||
                                           playerX == princessX - 2 && playerY == princessY) && bosshp <= 0)
                                        {
                                            nowScencID = 3;
                                            scenceChange = true;
                                        }
                                        break;
                                }
                            }
                        }
                        #endregion
                        break;
                    case 2:
                        // dead
                        Console.Clear();
                        Console.Write("dead!!!");
                        break;
                    case 3:
                        // win
                        Console.Clear();
                        Console.Write("win!!!");
                        break;
                }
                
            }
        }
    }
}
