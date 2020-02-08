using System;
using System.Threading.Tasks;

namespace CChat
{
    public class Matrix
    {
        // fields
        static Random rand = new Random();
        public static bool IsMatrixRunning { get; set; }

        // properties
        static char AsciiCharacter
        {
            get
            {
                int t = rand.Next(10);
                if (t <= 2)
                    // returns a number
                    return (char)('0' + rand.Next(10));
                else if (t <= 4)
                    // small letter
                    return (char)('a' + rand.Next(27));
                else if (t <= 6)
                    // capital letter
                    return (char)('A' + rand.Next(27));
                else
                    // any ascii character
                    return (char)(rand.Next(32, 255));
            }
        }

        public static void Run() 
        {
            Task.Run(() => Start());

        }

        private static void Start()
        {
            IsMatrixRunning = true;

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WindowLeft = Console.WindowTop = 0;
                Console.WindowHeight = Console.BufferHeight = Console.LargestWindowHeight - 1;
                Console.WindowWidth = Console.BufferWidth = Console.LargestWindowWidth - 1;
                Console.CursorVisible = false;

            }
            catch 
            {

                
            }


            int width, height;
            // setup array of starting y values
            int[] y;

            // width was 209, height was 81
            // setup the screen and initial conditions of y
            Initialize(out width, out height, out y);

            // do the Matrix effect
            // every loop all y's get incremented by 1
            while (true) 
            {
                UpdateAllColumns(width, height, y);

                if (!IsMatrixRunning) 
                {
                    break;
                }
            }

            Console.Clear();
            Console.CursorVisible = true;
        }


        private static void UpdateAllColumns(int width, int height, int[] y)
        {
            int x;
            // draws 3 characters in each x column each time... 
            // a dark green, light green, and a space

            // y is the position on the screen
            // y[x] increments 1 each time so each loop does the same thing but down 1 y value
            for (x = 0; x < width; ++x)
            {
                // the bright green character
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(x, y[x]);
                Console.Write(AsciiCharacter);

                // the dark green character -  2 positions above the bright green character
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                int temp = y[x] - 2;
                Console.SetCursorPosition(x, inScreenYPosition(temp, height));
                Console.Write(AsciiCharacter);

                // the 'space' - 20 positions above the bright green character
                int temp1 = y[x] - 20;
                Console.SetCursorPosition(x, inScreenYPosition(temp1, height));
                Console.Write(' ');

                // increment y
                y[x] = inScreenYPosition(y[x] + 1, height);
            }
        }

        // Deals with what happens when y position is off screen
        public static int inScreenYPosition(int yPosition, int height)
        {
            if (yPosition < 0)
                return yPosition + height;
            else if (yPosition < height)
                return yPosition;
            else
                return 0;
        }

        // only called once at the start
        private static void Initialize(out int width, out int height, out int[] y)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth - 1;

            // 209 for me.. starting y positions of bright green characters
            y = new int[width];

            Console.Clear();
            // loops 209 times for me
            for (int x = 0; x < width; ++x)
            {
                // gets random number between 0 and 81
                y[x] = rand.Next(height);
            }
        }
    }
}