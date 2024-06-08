using System;
using System.Security.AccessControl;
using System.Text;
using System.Collections.Generic;
using System.Threading;
namespace MemoryGame;

class UserInterface
{
    private const int k_MaxBoardLength = 6;
    private const int k_MinBoardLength = 4;
    private const int k_NumOfPlayers = 2;
    private bool m_EndGame = false;
    private GameLogicManager m_LogicManager = new GameLogicManager();
    

    public void SetBoard()
    {
        bool isBoardSizeEven = false;

        while (!isBoardSizeEven)
        {
            
            Console.WriteLine("Please enter the board's width (must be between {0} and {1}):", k_MinBoardLength, k_MaxBoardLength);
            SetNumberInRange(out int width);

            Console.WriteLine("Please enter the board's height (must be between {0} and {1}):", k_MinBoardLength, k_MaxBoardLength);
            SetNumberInRange(out int height);

            m_LogicManager.Board = new Board(height, width);
            isBoardSizeEven = m_LogicManager.Board.IsBoardSizeEven();
            
            if(!isBoardSizeEven)
            {
                Console.WriteLine("Invalid input. Board size must be even.");
                Console.WriteLine();
            }

            char[] letters = generateLettersForBoard(height, width);
            InitializeCardsInBoard(letters);
        }
    }

    public void InitializeCardsInBoard(char[] letters)
    {
        int letterIndex = 0;

        for (int i = 0; i < m_LogicManager.Board.Height; i++)
        {
            for (int j = 0; j < m_LogicManager.Board.Width; j++)
            {
                m_LogicManager.Board.CardsMatrix[i, j] = new Card(letters[letterIndex++]);
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

    private char[] generateLettersForBoard(int i_Height, int i_Width)
    {
        int numPairs = (i_Height * i_Width) / 2;
        char[] letters = new char[i_Height * i_Width];

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
        Menu();
        SetBoard();
    
        while (!m_LogicManager.Board.AreAllCardsDisplayed() && !m_EndGame)
        {
            PlayTurn();
        }
    }

    public void Menu()
    {
        Console.WriteLine("Welcome to Memory Game!");
        Console.WriteLine("Please choose a game mode:");
        Console.WriteLine("(1) Player vs. Player");
        Console.WriteLine("(2) Player vs. Computer");

        string selectedGameMode = Console.ReadLine();

        while(selectedGameMode != "1" && selectedGameMode != "2")
        {
            Console.WriteLine("Invalid game mode. Please choose 1 or 2.");
            selectedGameMode = Console.ReadLine();
        }

        if(selectedGameMode == "1")
        {   
            m_LogicManager.CurrentGameMode = GameLogicManager.eGameMode.HumanVsHuman;
            SetHumanPlayers();
        }
        else
        {
            m_LogicManager.CurrentGameMode = GameLogicManager.eGameMode.ComputerVsHuman;
            SetPlayerName(1, Player.ePlayerTypes.Human);
            SetPlayerName(2, Player.ePlayerTypes.Computer);
        }
    }

    public void PlayTurn()
    {
        if (!m_EndGame)
        {
            DrawBoard();
            Console.WriteLine("{0}, it's your turn.", m_LogicManager.Players[m_LogicManager.CurrentPlayerIndex].Name);
            Card firstCard = ChooseOneCard();

            if (!m_EndGame)
            {
                Console.Clear();
                DrawBoard();
                Card secondCard = ChooseOneCard();
                DrawBoard();

                if (!m_EndGame)
                {
                    if (firstCard.Value == secondCard.Value)
                    {
                        Console.WriteLine("It's a match");
                        m_LogicManager.UpdateScore();
                    }
                    else
                    {
                        Console.WriteLine("No match. Try again next turn.");
                        Thread.Sleep(2000);
                        m_LogicManager.NoMatch(firstCard, secondCard);
                    }

                    DrawBoard();
                }
            }
        }
    }
    
    public Card ChooseOneCard()
    {
        Card newCard;

        if (m_LogicManager.Players[m_LogicManager.CurrentPlayerIndex].PlayerType == Player.ePlayerTypes.Human)
        {
            newCard = ChooseOneCardHuman();
        }
        else
        {
            Console.WriteLine("Please select a card:");
            newCard = m_LogicManager.ChooseOneCardComputer();
        }

        return newCard;
    }

    public Card ChooseOneCardHuman()
    {
        Card selectedCard = null;

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
                    m_LogicManager.AddCardToMemory(selectedCard.Value - 'A', row, col); ///////FIX
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

    public void SetHumanPlayers()
    {
        for (int i = 0; i < k_NumOfPlayers ; i++)
        {
            SetPlayerName(i + 1, Player.ePlayerTypes.Human); // Pass the player number for display purposes
        }
    }

    private void SetPlayerName(int i_PlayerNumber,Player.ePlayerTypes i_Type)
    {
        string namePlayer;

        if(i_Type == Player.ePlayerTypes.Human)
        {
            Console.WriteLine("Please enter the name of player {0}:", i_PlayerNumber);
            namePlayer = Console.ReadLine();
        }
        else
        {
            namePlayer = "Computer";
        }
        Player newPlayer = new Player(namePlayer, i_Type);
        m_LogicManager.AddPlayer(newPlayer);
    }

}