using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTutorials
{
    class Program
    {
        static string[,] Board = { { "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-" }, { "-", "-", "-", "-", "-", "-" } };
        static int totalTurn = 0;
        static int[] p1Position = new int[] { 0, 0 };
        static int[] p2Position = new int[] { 5, 5 };
        static int p1Gemcount = 0;
        static int p2Gemcount = 0;

        static void displayBoard(string[,] Board)
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                Console.WriteLine("");
                for(int j = 0; j < Board.GetLength(1); j++)
                {
                    Console.Write(Board[i, j] + "  ");
                }
            }
        }
        static void Main(string[] args)
        {
            displayBoard(Board);
        }
    }
}