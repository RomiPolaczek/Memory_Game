using System;
using System.Text;
namespace MemoryGame;

class UserInterface
{
    private const int k_MaxBoardLength = 6;
    private const int k_MinBoardLength = 4;

    private GameLogicManager<char> m_LogicManager = new GameLogicManager<char>();
    
    public void SetPlayersName()
    {
        System.Console.WriteLine("Please enter your name:");
        string namePlayer = Console.ReadLine();
        m_LogicManager.Player = new Player(namePlayer);
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

        
            char[] letters = generateLettersForBoard(height, width);
            m_LogicManager.Board = new Board<char>(height, width, letters);
            isBoardSizeEven = m_LogicManager.Board.IsBoardSizeEven();
            
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
        int amountOfEqualSigns = (m_LogicManager.Board.Width * 4) + 1;
        string equalLine = string.Format("  {0}", new string('=', amountOfEqualSigns));

        Console.Clear(); ///לשנות !!!
        //drawTurnStatus();
        drawTopLetterRow(m_LogicManager.Board.Width);

        Console.WriteLine(equalLine);

        for (int i = 0; i < m_LogicManager.Board.Height; i++)
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

        for (int j = 0; j < m_LogicManager.Board.Width; j++)
        {
            char c = m_LogicManager.Board.GetCardInIndex(i_Index, j).Value;
            // BoardLetter currentBoardLetter = m_GameLogicManager.Letters[i_Index, j];
            //string CellToProint = string.Format(" {0} |", currentBoardLetter.IsHidden ? ' ' : currentBoardLetter.Letter);
            string CardToPrint = string.Format(" {0} |",c);
            Console.Write(CardToPrint);
        }
        Console.WriteLine();
    }

    private char[] generateLettersForBoard(int height, int width)
    {
        int numPairs = (height * width) / 2;
        char[] letters = new char[height * width];

        for (int i = 0; i < numPairs; i++)
        {
            char letter = (char)('A' + i);
            letters[2 * i] = letter;
            letters[2 * i + 1] = letter;
        }

        // Shuffle letters
        Random rnd = new Random();
        for (int i = letters.Length - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            char temp = letters[i];
            letters[i] = letters[j];
            letters[j] = temp;
        }

        return letters;
    }


//Need to finish this method
    // public void PlayTurn()
    // {
    //     Console.WriteLine("{0}, it's your turn.", m_LogicManager.Player.Name);
    //     Console.WriteLine("Please select a card:");
    //     string indexStr = Console.ReadLine();

    //     int rowIndex = indexStr[0] - 'A';
    //     int colIndex = int.Parse(indexStr.Substring(1)) - 1;

    //     if (rowIndex < 0 || rowIndex >= m_Board.Height || colIndex < 0 || colIndex >= m_Board.Width)
    //     {
    //         Console.WriteLine("Invalid card selection. Please try again.");
    //         return;
    //     }

    //     // Get the card at the selected position
    //     Card<T> selectedCard = m_Board.GetCardInIndex(rowIndex, colIndex);

    //     // Check if the card is already flipped
    //     if (selectedCard.Flipped)
    //     {
    //         Console.WriteLine("Card {0} has already been flipped. Please select another card.", input);
    //         return;
    //     }

    //     // Flip the card
    //     selectedCard.Flipped = true;

    //     // Display the current state of the board
    //     Console.WriteLine("Selected card {0} has value {1}.", input, selectedCard.Value);
    //     DisplayBoardState();

    // }
}