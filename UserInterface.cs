using System;
using System.Security.AccessControl;
using System.Text;
using System.Collections.Generic;
using System.Threading;
namespace MemoryGame;

internal class UserInterface
{
    private const int k_MaxBoardLength = 6;
    private const int k_MinBoardLength = 4;
    private const int k_NumOfPlayers = 2;
    private bool m_EndGame;
    private GameLogicManager m_LogicManager; 

    public UserInterface()
    {
        m_LogicManager = new GameLogicManager();  
        m_EndGame = false;
    }

    public void RunGame()
    {
        bool continueGame = true;

        menu();

        while (continueGame && !m_EndGame)
        {
            setBoard();

            while (!m_LogicManager.Board.AreAllCardsDisplayed() && !m_EndGame)
            {
                playTurn();
            }

            if (m_LogicManager.Board.AreAllCardsDisplayed())
            {
                printScore();
                endGameOrContinue(out continueGame);
            }
        }
    }

    private void menu()
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
            setHumanPlayers();
        }
        else
        {
            m_LogicManager.CurrentGameMode = GameLogicManager.eGameMode.ComputerVsHuman;
            setPlayerName(1, Player.ePlayerType.Human);
            setPlayerName(2, Player.ePlayerType.Computer);
        }
    }

    private void setHumanPlayers()
    {
        for (int i = 0; i < k_NumOfPlayers ; i++)
        {
            setPlayerName(i + 1, Player.ePlayerType.Human); // Pass the player number for display purposes
        }
    }

    private void setPlayerName(int i_PlayerNumber, Player.ePlayerType i_Type)
    {
        string namePlayer;

        if(i_Type == Player.ePlayerType.Human)
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

    private void setBoard()
    {
        Console.Clear();
        bool isBoardSizeEven = false;

        while (!isBoardSizeEven)
        {
            Console.WriteLine("Please enter the board's width (must be between {0} and {1}):", k_MinBoardLength, k_MaxBoardLength);
            setNumberInRange(out int width);

            Console.WriteLine("Please enter the board's height (must be between {0} and {1}):", k_MinBoardLength, k_MaxBoardLength);
            setNumberInRange(out int height);

            m_LogicManager.Board = new Board(height, width);
            isBoardSizeEven = m_LogicManager.Board.IsBoardSizeEven();
            
            if(!isBoardSizeEven)
            {
                Console.WriteLine("Invalid input. Board size must be even.");
                Console.WriteLine();
            }

            char[] letters = generateLettersForBoard(height, width);

            initializeCardsInBoard(letters);
        }
    }

    private void setNumberInRange(out int o_UserInput)
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

        Random rnd = new Random();

        for (int i = letters.Length - 1; i > 0; i--)
        {
            int randomIndex = rnd.Next(i + 1);
            char currentLetter = letters[i];
            letters[i] = letters[randomIndex];
            letters[randomIndex] = currentLetter;
        }

        return letters;
    }

    private void initializeCardsInBoard(char[] i_Letters)
    {
        int letterIndex = 0;

        for (int i = 0; i < m_LogicManager.Board.Height; i++)
        {
            for (int j = 0; j < m_LogicManager.Board.Width; j++)
            {
                m_LogicManager.Board.CardsMatrix[i, j] = new Card(i_Letters[letterIndex++], i, j);
            }
        }
    }

    private void playTurn()
    {
        if (!m_EndGame)
        {
            m_LogicManager.PossibleMatchValue = -1;
            m_LogicManager.FirstSelectedCard = -1;

            drawBoard();
            Console.WriteLine("{0}, it's your turn.", m_LogicManager.Players[m_LogicManager.CurrentPlayerIndex].Name);

            Card firstCard = chooseOneCard();

            printComputerChoice(firstCard);

            if (!m_EndGame)
            {
                m_LogicManager.FirstSelectedCard = firstCard.CardValueToIndex(); 
                Console.Clear();
                drawBoard();

                Card secondCard = chooseOneCard();

                printComputerChoice(secondCard);
                drawBoard();

                if (!m_EndGame)
                {
                    if (firstCard.Value == secondCard.Value)
                    {
                        Console.WriteLine("It's a match");
                        Thread.Sleep(2000);
                        m_LogicManager.UpdateScore();
                        m_LogicManager.DeleteHumanMatchFromMemory(firstCard.CardValueToIndex());
                    }
                    else
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("No match. Try again next turn.");
                        Thread.Sleep(2000);
                        m_LogicManager.NoMatch(firstCard, secondCard);
                        m_LogicManager.AddCardToMemory(firstCard.CardValueToIndex(), firstCard.Position.Row, firstCard.Position.Col); 
                        m_LogicManager.AddCardToMemory(secondCard.CardValueToIndex(), secondCard.Position.Row, secondCard.Position.Col);
                    }

                    drawBoard();
                }
            }
        }
    }

    private void drawBoard()
    {
        int amountOfEqualSigns = (m_LogicManager.Board.Width * 4) + 1;
        string equalLine = string.Format("  {0}", new string('=', amountOfEqualSigns));

        Console.Clear(); ///לשנות !!!
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
            bool isDisplayed = m_LogicManager.Board.GetCardInIndex(i_Index, j).Displayed;
            string cardToPrint = string.Format(" {0} |", isDisplayed ? cardLetter : ' ');
            Console.Write(cardToPrint);
        }

        Console.WriteLine();
    }
    
    private Card chooseOneCard()
    {
        Card newCard;

        if (m_LogicManager.Players[m_LogicManager.CurrentPlayerIndex].PlayerType == Player.ePlayerType.Human)
        {
            newCard = chooseOneCardHuman();
        }
        else
        {
            Console.WriteLine("Please select a card:");
            Thread.Sleep(2000);

            newCard = m_LogicManager.ChooseOneCardComputer();
        }

        return newCard;
    }

    private Card chooseOneCardHuman()
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

    private bool parseInput(string i_InputStr, out int o_Row, out int o_Col)
    {
        o_Row = -1;
        o_Col = -1;
        bool isValidInput = true;

        if (i_InputStr.Length != 2)
        {
            isValidInput = false;
            Console.WriteLine("Invalid lentgh. Please try again."); //TEST
        }
        else
        {
            o_Col = i_InputStr[0] - 'A';

            if (o_Col < 0 || o_Col >= m_LogicManager.Board.Width)
            {
                isValidInput = false;
                Console.WriteLine("Invalid col number. Please try again."); //TEST

            }

            if (!int.TryParse(i_InputStr.Substring(1), out o_Row))
            {
                isValidInput = false;
            }

            o_Row--;

            if (o_Row < 0 || o_Row >= m_LogicManager.Board.Height)
            {
                isValidInput = false;
                Console.WriteLine("Invalid row number. Please try again."); //TEST
            }
        }

        return isValidInput;
    }
    
    private void printComputerChoice(Card i_SelectedCard)
    {
        if (m_LogicManager.Players[m_LogicManager.CurrentPlayerIndex].PlayerType == Player.ePlayerType.Computer)
        {
            Console.WriteLine("{0}{1}", (char)(i_SelectedCard.Position.Col + 'A'), i_SelectedCard.Position.Row + 1);
            Thread.Sleep(1000);
        }
    }  

    private void printScore()
    {
        if (m_LogicManager.Players[0].Score > m_LogicManager.Players[1].Score)
        {
            Console.WriteLine("The winner is {0} with {1} points! ", m_LogicManager.Players[0].Name, m_LogicManager.Players[0].Score);
            Console.WriteLine("{0} with {1} points! ", m_LogicManager.Players[1].Name, m_LogicManager.Players[1].Score);
        }
        else
        {
            Console.WriteLine("The winner is {0} with {1} points! ", m_LogicManager.Players[1].Name, m_LogicManager.Players[1].Score);
            Console.WriteLine("{0} with {1} points! ", m_LogicManager.Players[0].Name, m_LogicManager.Players[0].Score);
        }
    }

    private void endGameOrContinue(out bool o_ContinueGame)
    {
        Thread.Sleep(2000);
        Console.Clear();
        Console.WriteLine("Do you want to play another game? \n(1) Yes \n(2) No");

        string continueGameStr = Console.ReadLine();

        while (continueGameStr != "1" && continueGameStr != "2")
        {
            Console.WriteLine("Invalid input. Please select 1 or 2");
            continueGameStr = Console.ReadLine();
        }

        o_ContinueGame = (continueGameStr == "1");

        if (!o_ContinueGame)
        {
            Console.Clear();
            Console.WriteLine("The game is over! thank you for playing (:");
        }
    }
}