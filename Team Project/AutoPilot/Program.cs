// TP03 - Team 1 - CS132 - Summer 2025

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AutoPilot
{
    class Program
    {
        static private int width = 55;                                      // define width of animation screen as 55 pixels
        static private int height = 30;                                     // define height of animation screen as 30 pixels
        static private int span = 10;                                       // define distance between left and right road shoulders
        static private char[,] display;                                     // char array representing screen for a single frame of animation
        static private Random random = new Random();                        // random number generator for movement of road     
        static private List<(int left, int right)> roadRows = new List<(int, int)>();           // list containing left & right shoulder positions
        static private int leftShoulder = 5;                                // starting column for left shoulder
        
        static void Main(string[] args)
        // ------------------------------------------------
        // Summary: init road, hide cursor, set text color, enters animation set/draw infinite loop.
        // ------------------------------------------------
        {
            Console.CursorVisible = false;                                  // hide cursor in terminal output
            Console.ForegroundColor = ConsoleColor.Green;                   // set console text color to green

            int left = leftShoulder;                                        // init road as straight path down screen
            for (int i = 0; i < height; i++)
                roadRows.Add((left, left + span));

            while (true)                                                    // init animation loop
            {
                updateRoad();                                               // shifts road left/right
                buildDisplayFromRoad();                                     // fills display array w/ road edges, spaces
                placeCar();                                                 // place car glyph on display
                drawRoadAndCar();                                           // render display array to console
                Thread.Sleep(100);                                          // set delay between frame rendering to control animation speed
            }
        }

        static void updateRoad()
        // ------------------------------------------------
        // Summary: move road by removing top row, adding new bottom row, introducing small random shift left/right. 
        // Maintain road within screen dimensions.
        // ------------------------------------------------
        {
            var lastRow = roadRows[roadRows.Count - 1];                // get left shoulder position from current bottom row
            int left = lastRow.left;                                        // define position as integer

            int delta = random.Next(-1, 2);                                 // random shift road left (-1), stay (0), or right (+1)
            int newLeft = left + delta;                                     // define result of delta as new integer

            int minLeft = 1;                                                // define leftmost boundary 
            int maxLeft = width - span - 2;                                 // define rightmost boundary
            if (newLeft < minLeft) newLeft = minLeft;                       // evaluate if newLeft will go off-screen to left, and if so, override with first column number
            if (newLeft > maxLeft) newLeft = maxLeft;                       // evaluate if newLeft will go off-screen to right, and if so, override with last column number

            roadRows.RemoveAt(0);                                           // remove top row for animation
            roadRows.Add((newLeft, newLeft + span));                        // add bottom row for animation
        }

        static void buildDisplayFromRoad()
        // ------------------------------------------------
        // Summary: derive fresh display from current frame, fill with spaces 
        // and road edge chars based on positions from roadRows
        // ------------------------------------------------
        {
            display = new char[height, width];                              // start with new blank frame

            for (int row = 0; row < height; row++)                          // init loop: for each row in display...
            {
                var (left, right) = roadRows[row];                          // get shoulder positions for this row

                for (int col = 0; col < width; col++)                       // init loop: for each col in current row...
                {
                    if (col == left || col == right)                        // if col is left shoulder or right shoulder...
                        display[row, col] = '|';                            // set road edge
                    else                                                    // otherwise...
                        display[row, col] = ' ';                            // set empty space
                }
            }
        }

        static void placeCar()
        // ------------------------------------------------
        // Summary: draw car glyph in display array, one row above bottom row (configurable), centered between shoulders
        // ------------------------------------------------
        {
            int carRow          = 1;                                        // set car glyph one row above bottom
            var (left, right)   = roadRows[carRow];                         // ref road shoulders
            int carCol          = left + (span / 2);                        // center of road

            display[carRow, carCol] = 'Ö';                                  // place car glyph
        }

        static void drawRoadAndCar()
        // ------------------------------------------------
        // Summary: convert display array into single string, write to console starting at top-left corner.
        // ------------------------------------------------    
        {
            StringBuilder paintAll = new StringBuilder(width * height);     // define a new StringBuilder object with a capacity of width * height

            for (int x = height -1; x >= 0; x--)                                // Loop through available rows while x is >= 0 but less than height.
            {
                for (int y = 0; y < width; y++)                             // Loop through available columns while y is >= 0 but less than width.
                {
                    paintAll.Append(display[x, y]);                         // invoke stringbuilder "paintAll" and append the character at coordinates [x,y] from the display array.
                }
                paintAll.AppendLine();                                      // Append a new line to the StringBuilder after each row is completed.
            }

            Console.SetCursorPosition(0, 0);                                // Set the cursor position to the top-left corner of the console.
            Console.Write(paintAll);                                        // Write the contents of the StringBuilder to the console.
        }
    }
}