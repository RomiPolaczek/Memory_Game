using System;
using System.Text;
namespace MemoryGame;

class UserInterface
{

    private Board m_Board;
    private const int k_MaxBoardLength = 6;
    private const int k_MinBoardLength = 4;
    //private Player[] m_Players = new Player[2];
    private Player m_player;


    public void SetPlayersName()
    {
        System.Console.WriteLine("Please enter your name:");
        string namePlayer = Console.ReadLine();
        m_player = new Player(namePlayer);
    }

    public void SetBoardSize()
    {
        bool isBoardSizeEven = false;

        while (!isBoardSizeEven)
        {
            
            Console.WriteLine("Please enter the board's width (must be between {0} and {1}):", k_MinBoardLength, k_MaxBoardLength);
            SetNumberInRange(out int width);

            Console.WriteLine("Please enter the board's height (must be between {0} and {1}):", k_MinBoardLength, k_MaxBoardLength);
            SetNumberInRange(out int height);


            m_Board = new Board(height, width);
            isBoardSizeEven = m_Board.IsBoardSizeEven();
            
            if(!isBoardSizeEven)
            {
                Console.WriteLine("Invalid input. Board size must be even.");
                Console.WriteLine();
            }

        }
    }

    public void SetNumberInRange(out int o_UserInput)
    {
        o_UserInput = 0;
        bool isNumber = false;
        bool isNumInRange = false;

        while (!isNumber || !isNumInRange)
        {
            isNumber = int.TryParse(Console.ReadLine(), out o_UserInput);

            if (isNumber)
            {
                isNumInRange = o_UserInput >= k_MinBoardLength && o_UserInput <= k_MaxBoardLength;
            }
            
            if(!isNumber || !isNumInRange)
            {
                Console.WriteLine("Invalid value. Try again.");
            }
        }
    }

    public void DrawBoard()
        {
            int amountOfEqualSigns = (m_Board.Width * 4) + 1;
            string equalLine = string.Format("  {0}", new string('=', amountOfEqualSigns));

            Console.Clear(); ///לשנות !!!
            //drawTurnStatus();
            drawTopLetterRow(m_Board.Width);

            Console.WriteLine(equalLine);

            for (int i = 0; i < m_Board.Height; i++)
            {
                drawRowAtIndex(i);
                Console.WriteLine(equalLine);
            }

            Console.WriteLine();
        }

        private void drawTopLetterRow(int i_LengthOfRow)
        {
            StringBuilder topRowToPrint = new StringBuilder(" ");

            for (int i = 0; i < i_LengthOfRow; i++)
            {
                topRowToPrint.Append(string.Format("   {0}", (char)(i + 'A')));
            }

            Console.WriteLine(topRowToPrint.ToString());
        }

        private void drawRowAtIndex(int i_Index)
        {
            string beginningOfRow = string.Format("{0} |", i_Index + 1);

            Console.Write(beginningOfRow);

            for (int j = 0; j < m_Board.Width; j++)
            {
               // BoardLetter currentBoardLetter = m_GameLogicManager.Letters[i_Index, j];
               // string CellToProint = string.Format(" {0} |", currentBoardLetter.IsHidden ? ' ' : currentBoardLetter.Letter);
                Console.Write("   |");
                //Console.Write(CellToProint);
            }
            Console.WriteLine();
        }
}