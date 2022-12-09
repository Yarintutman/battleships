using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleApp22
{
    public class Program
    {
        const int M = 7, N = 7;
        const int BIG_SHIP_SIZE = 4, MEDIUM_SHIP_SIZE = 2, SMALL_SHIP_SIZE = 1;
        const int EMPTY = 0, SHIP_ZONE = 1, SHIP = 2, HIT = 3, DESTROYED = 4, MISS = 5;
        const int NUMBER_OF_BIG_SHIPS = 2, NUMBER_OF_MEDIUM_SHIPS = 3, NUMBER_OF_SMALL_SHIPS = 4;
        const int UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3;
        const int COUNT_TRIES = 60;

        public static int counterShips;
        public static int countBigShips, countMediumShips, countSmallShips;
        public static int countHits;
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי וקולטת נקודות ציון
        //הפעולה בודקת מה יש בנקודות הציון,האם הן תקינות ומדפיסה פלט מתאים
        public static void Target(int[,] board)
        {
            bool hit = false, valid = true;
            int x = -1, y = -1, ship;
            while (!hit)
            {
                valid = true;
                while (valid)
                {
                    Console.Clear();
                    CreateBoard(board);
                    Console.WriteLine("Enter Your Coordinates");
                    try
                    {
                        y = (int)(char.Parse(Console.ReadLine())) - 65;
                        x = int.Parse(Console.ReadLine()) - 1;
                        valid = false;
                    }
                    catch
                    {
                        valid = true;
                        Console.WriteLine("why are you trying to crash my game");
                        Console.ReadLine();
                    }
                    
                }
                if( x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1) && (board[x, y] < HIT))
                {
                    if (board[x, y] == SHIP)
                    {
                        ship = Hit(board, x, y, x, y);
                        if (ship == HIT)
                        {
                            board[x, y] = HIT;
                            Console.Clear();
                            CreateBoard(board);
                            Console.WriteLine("Hit!");
                        }
                        else if (ship == EMPTY)
                        {
                            countHits = 0;
                            DesrtroyShip(board, x, y, x, y);
                            Console.Clear();
                            CreateBoard(board);
                            counterShips--;
                            Console.WriteLine("Destroyed Ship!");
                            if (countHits == BIG_SHIP_SIZE)
                            {
                                countBigShips--;
                            }
                            else if (countHits == MEDIUM_SHIP_SIZE)
                            {
                                countMediumShips--;
                            }
                            else if (countHits == SMALL_SHIP_SIZE)
                            {
                                countSmallShips--;
                            }
                        }
                        hit = true;
                    }
                    else
                    {
                        board[x, y] = MISS;
                        Console.Clear();
                        CreateBoard(board);
                        Console.WriteLine("Miss");
                        hit = true;
                    }
                }
                else
                {
                    if (!(x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1)))
                    {
                        Console.WriteLine("out of borders");
                    }
                    else if((board[x, y] >= HIT))
                    {
                        Console.WriteLine("are you tring to hit the same spot??");
                    }
                    else
                    {
                        Console.WriteLine("U R wrong");
                    }
                    Console.ReadLine();
                }
            }
            Console.WriteLine("There are {0} Big Ships, {1} Medium Ships, and {2} Small Ships", countBigShips, countMediumShips, countSmallShips);
            Console.ReadLine();
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי של מספרים שלמים, את נקודות הציון העדכניות ונקודות הציון המקוריות (בשביל הרקורסיה) וארבע מספרים שלמים
        //הפעולה בודקת האם הספינה נפגעה ומחזירה מספר שלם המייצג פגיעה
        public static int Hit(int[,] board, int x, int y, int mainX, int mainY)
        {
            int counter = 0;
            for (int i = -1; i < 2; i += 2)
            {
                if (((x + i) != mainX || (y != mainY)) && (x + i >= 0 && x + i < board.GetLength(0)))
                {
                    if (board[x + i, y] == SHIP)
                    {
                        return HIT;
                    }
                    else if (board[x + i, y] == HIT)
                    {
                        if (Hit(board, x + i, y, x, y) == EMPTY)
                        {
                            counter++;
                        }
                        else
                        {
                            return HIT;
                        }
                    }
                    else
                    {
                        counter++;
                    }
                }
                else
                {
                    counter++;
                }
                if (((y + i) != mainY || (x != mainX)) && (y + i >= 0 && y + i < board.GetLength(1)))
                {
                    if (board[x, y + i] == SHIP)
                    {
                        return HIT;
                    }
                    else if (board[x, y + i] == HIT)
                    {
                        if (Hit(board, x, y + i, x, y) == EMPTY)
                        {
                            counter++;
                        }
                        else
                        {
                            return HIT;
                        }
                    }
                    else
                    {
                        counter++;
                    }
                }
                else
                {
                    counter++;
                }
            }
            if (counter == 4)
            {
                return EMPTY;
            }
            else
            {
                return HIT;
            }
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי של מספרים שלמים, את נקודות הציון העדכניות ונקודות הציון המקוריות (בשביל הרקורסיה) וארבע מספרים שלמים
        //הפעולה משנה את ערך כל הספינה למושמד
        public static void DesrtroyShip(int[,] board, int x, int y, int mainX, int mainY)
        {
            board[x, y] = DESTROYED;
            countHits++;
            for (int i = -1; i < 2; i += 2)
            {
                if (((x + i) != mainX || (y != mainY)) && (x + i >= 0 && x + i < board.GetLength(0)))
                {
                    if (board[x + i, y] == HIT)
                    {
                        DesrtroyShip(board, x + i, y, x, y);
                    }
                }
                if (((y + i) != mainY || (x != mainX)) && (y + i >= 0 && y + i < board.GetLength(0)))
                {
                    if (board[x, y + i] == HIT)
                    {
                        DesrtroyShip(board, x, y + i, x, y);
                    }
                }

            }
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי
        // הפעולה מדפיסה קווים
        public static void CreateLine(int[,] board)
        {
            Console.Write("    _");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.Write("________");
            }
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי
        //הפעולה מדפיסה את לוח המשחק 
        public static void CreateBoard(int[,] board)
        {
            Console.Clear();
            Console.WriteLine();
            int n = board.GetLength(0), m = board.GetLength(1);

            for (int i = 1; i <= n; i++)
            {
                Console.Write("\t" + i);
            }
            char c;
            Console.WriteLine();
            CreateLine(board);
            Console.WriteLine("\n");

            for (int i = 0; i < m; i++)
            {
                c = (char)('A' + i);
                Console.Write(c);
                for (int j = 0; j < n; j++)
                {
                    if (j == 0)
                    {
                        Console.Write("   |");
                    }
                    Console.Write("\t");
                    if (board[j, i] == MISS)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("#");
                    }
                    else if (board[j, i] == HIT)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("#");
                    }
                    else if (board[j, i] == DESTROYED)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("#");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("#");
                    }
                    Console.ResetColor();
                    Console.Write("   |");
                }
                Console.WriteLine();
                CreateLine(board);
                Console.WriteLine("\n");
            }
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי, נקודות ציון שני מספרים שלמים, כיוון מספר שלם וגודל הספינה מספר שלם
        //הפעולה שמה ספינה ומסביב איזור שבו אי אפשר לשים ספינה אחרת
        public static void SetShip(int[,] board, int x, int y, int diraction, int shipSize)
        {
            for (int i = 0; i < shipSize; i++)
            {
                if (diraction == RIGHT)
                {
                    board[x, y + i] = SHIP;
                    if (x - 1 >= 0 && board[x - 1, y + i] != SHIP)
                    {
                        board[x - 1, y + i] = SHIP_ZONE;
                    }
                    if (x + 1 < N && board[x + 1, y + i] != SHIP)
                    {
                        board[x + 1, y + i] = SHIP_ZONE;
                    }
                    if (i == 0 && y - 1 >= 0 && board[x, y - 1] != SHIP)
                    {
                        board[x, y - 1] = SHIP_ZONE;
                    }
                    if (i == shipSize - 1 && y + 1 + i < M && board[x, y + i + 1] != SHIP)
                    {
                        board[x, y + 1 + i] = SHIP_ZONE;
                    }
                }
                if (diraction == LEFT)
                {
                    board[x, y - i] = SHIP;
                    if (x - 1 >= 0 && board[x - 1, y - i] != SHIP)
                    {
                        board[x - 1, y - i] = SHIP_ZONE;
                    }
                    if (x + 1 < N && board[x + 1, y - i] != SHIP)
                    {
                        board[x + 1, y - i] = SHIP_ZONE;
                    }
                    if (i == 0 && y + 1 < M && board[x, y + 1] != SHIP)
                    {
                        board[x, y + 1] = SHIP_ZONE;
                    }
                    if (i == shipSize - 1 && y - 1 - i >= 0 && board[x, y - 1 - i] != SHIP)
                    {
                        board[x, y - 1 - i] = SHIP_ZONE;
                    }
                }
                if (diraction == UP)
                {
                    board[x - i, y] = SHIP;
                    if (y - 1 >= 0 && board[x - i, y - 1] != SHIP)
                    {
                        board[x - i, y - 1] = SHIP_ZONE;
                    }
                    if (y + 1 < M && board[x - i, y + 1] != SHIP)
                    {
                        board[x - i, y + 1] = SHIP_ZONE;
                    }
                    if (i == 0 && x + 1 < N && board[x + 1, y] != SHIP)
                    {
                        board[x + 1, y] = SHIP_ZONE;
                    }
                    if (i == shipSize - 1 && x - 1 - i >= 0 && board[x - 1 - i, y] != SHIP)
                    {
                        board[x - 1 - i, y] = SHIP_ZONE;
                    }
                }
                if (diraction == DOWN)
                {
                    board[x + i, y] = SHIP;
                    if (y - 1 >= 0 && board[x + i, y - 1] != SHIP)
                        board[x + i, y - 1] = SHIP_ZONE;
                    if (y + 1 < M && board[x + i, y + 1] != SHIP)
                        board[x + i, y + 1] = SHIP_ZONE;
                    if (i == 0 && x - 1 >= 0 && board[x - 1, y] != SHIP)
                    {
                        board[x - 1, y] = SHIP_ZONE;
                    }
                    if (i == shipSize - 1 && x + 1 + i < N && board[x + 1 + i, y] != SHIP)
                    {
                        board[x + i + 1, y] = SHIP_ZONE;
                    }
                }
            }
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי, נקודות ציון  שני מספרים שלמים, כיוון מספר שלם וגודל הספינה מספר שלם
        //הפעולה בודקת ומחזירה האם אפשר לשים ספינה 
        public static bool Check(int[,] board, int x, int y, int ship, int diraction)
        {
            for (int i = 0; i < ship; i++)
            {
                if (x >= 0 && x < N && y >= 0 && y < M)
                {
                    if (board[x,y] == EMPTY)
                    {
                        if (diraction == UP)
                        {
                            x--;
                        }
                        else if (diraction == DOWN)
                        {
                            x++;
                        }
                        else if (diraction == RIGHT)
                        {
                            y++;
                        }
                        else
                        {
                            y--;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        //הפעולה מקבלת את לוח המשחק מערך דו מימדי
        //הפעולה מגרילה כיוונים ונקודות לכל ספינה ומחזירה האם ניתן לשים את כל הספינות 
        public static bool PlaceShips(int[,] board)
        {
            int i = 0, x, y, diraction;
            bool found = true;
            Random rnd = new Random();

            for (int j = 0; j < NUMBER_OF_BIG_SHIPS; j++)
            {
                found = false;
                while (i < COUNT_TRIES && !found)
                {
                    x = rnd.Next(0, N);
                    y = rnd.Next(0, M);
                    diraction = rnd.Next(0, 4);
                    if (Check(board, x, y, BIG_SHIP_SIZE, diraction))
                    {
                        SetShip(board, x, y, diraction, BIG_SHIP_SIZE);
                        found = true;
                    }
                    i++;
                }
            }
            if (i < COUNT_TRIES)
            {
                for (int j = 0; j < NUMBER_OF_MEDIUM_SHIPS; j++)
                {
                    found = false;
                    while (i < COUNT_TRIES && !found)
                    {
                        x = rnd.Next(0, N);
                        y = rnd.Next(0, M);
                        diraction = rnd.Next(0, 4);
                        if (Check(board, x, y, MEDIUM_SHIP_SIZE, diraction))
                        {
                            SetShip(board, x, y, diraction, MEDIUM_SHIP_SIZE);
                            found = true;
                        }
                        i++;
                    }
                }
            }
            if (i < COUNT_TRIES)
            {
                for (int j = 0; j < NUMBER_OF_SMALL_SHIPS; j++)
                {
                    found = false;
                    while (i < COUNT_TRIES && !found)
                    {
                        x = rnd.Next(0, N);
                        y = rnd.Next(0, M);
                        diraction = rnd.Next(0, 4);
                        if (Check(board, x, y, SMALL_SHIP_SIZE, diraction))
                        {
                            SetShip(board, x, y, diraction, SMALL_SHIP_SIZE);
                            found = true;
                        }
                        i++;
                    }
                }
            }
            if (i < COUNT_TRIES)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //הפעולה לא מקבלת ערכים
        //הפעולה מתחילה את המשחק ומפעילה את כל שאר הפעולות
        public static void StartGame()
        {
            int[,] board = new int[N, M];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    board[i, j] = 0;
                }
            }
            if (PlaceShips(board))
            {
                counterShips = NUMBER_OF_BIG_SHIPS + NUMBER_OF_MEDIUM_SHIPS + NUMBER_OF_SMALL_SHIPS;
                countBigShips = NUMBER_OF_BIG_SHIPS;
                countMediumShips = NUMBER_OF_MEDIUM_SHIPS;
                countSmallShips = NUMBER_OF_SMALL_SHIPS;
                int countTurns = 0;
                CreateBoard(board);
                while (counterShips > 0)
                {
                    Target(board);
                    CreateBoard(board);
                    countTurns++;
                }
                Console.WriteLine("You won in {0} turns", countTurns);
            }
            else
            {
                Console.WriteLine("failed to start game...");
            }
        }
        public static void Main(string[] args)
        {
            StartGame();
        }
    }
}
