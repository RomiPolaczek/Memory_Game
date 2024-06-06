using System;
using System.Text;
using System.Threading;
namespace MemoryGame;

class UserInterface
{
    private const int k_MaxBoardLength = 6;
    private const int k_MinBoardLength = 4;
    private bool m_EndGame = false;

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
            char cardLetter = m_LogicManager.Board.GetCardInIndex(i_Index, j).Value;
            bool isDisplayed= m_LogicManager.Board.GetCardInIndex(i_Index, j).Displayed;
            string CardToPrint = string.Format(" {0} |", isDisplayed ? cardLetter : ' ');
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

    public void RunGame()
    {
        while (!m_LogicManager.Board.AreAllCardsDisplayed() && !m_EndGame)
        {
            PlayTurn();
        }
    }
    public void PlayTurn()
    {
        Console.WriteLine("{0}, it's your turn.", m_LogicManager.Player.Name);
        Card<char> firstCard = ChooseOneCard();
        if (m_EndGame)
        {
            Console.WriteLine("Good Bye!");
            Environment.Exit(0);
        }

        Console.Clear();
        DrawBoard();
        Card<char> secondCard = ChooseOneCard();
        DrawBoard();
    
        if(firstCard.Value == secondCard.Value)
        {
            Console.WriteLine("It's a match");
            m_LogicManager.Player.Score += 1;
        }
        else
        {
            Console.WriteLine("No match. Try again next turn.");
            Thread.Sleep(2000);
            firstCard.Displayed = false;
            secondCard.Displayed = false;
        }
    
        DrawBoard();
    }

    public Card<char> ChooseOneCard()
    {
        Card<char> selectedCard = null;

        while(selectedCard == null && !m_EndGame)
        {
            Console.WriteLine("Please select a card:");
            string chosenCell = Console.ReadLine();

            if (chosenCell == "Q")
            {
                m_EndGame=true;
            }
            else if (parseInput(chosenCell, out int row, out int col))
            {
                selectedCard = m_LogicManager.Board.GetCardInIndex(row, col);
                if (!selectedCard.Displayed)
                {
                    selectedCard.Displayed = true;
                }
                else
                {
                    Console.WriteLine("Card {0} has already been flipped. Please select another card.", chosenCell);
                    selectedCard = null;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
         }

        return selectedCard;

    }

    private bool parseInput(string input, out int row, out int col)
    {
        row = -1;
        col = -1;
        bool isValidInput = true;

        if (input.Length != 2)
        {
            isValidInput = false;
        }

        col = input[0] - 'A';
        if (col < 0 || col >= m_LogicManager.Board.Width)
        {
            isValidInput = false;
        }

        if (!int.TryParse(input.Substring(1), out row))
        {
            isValidInput = false;
        }

        row--; 
        if (row < 0 || row >= m_LogicManager.Board.Height)
        {
            isValidInput = false;
        }

        return isValidInput;
    }
}