using System;
using System.Collections.Generic;
namespace MemoryGame;

class GameLogicManager
{
    private Board m_Board;
    private List<Player> m_Players;
    private int m_CurrentPlayerIndex;
    private eGameMode m_CurrentGameMode;
    private Dictionary<int, List<Position>> m_CardMemory;
    private int m_FirstRevealedCard;

    internal enum eGameMode //לבדוק תקינות קוד
    {
        HumanVsHuman,
        ComputerVsHuman
    }

    public GameLogicManager()
    {
        m_Players = new List<Player>();
        m_CurrentPlayerIndex = 0;
        m_CardMemory = new Dictionary<int, List<Position>>();
        m_FirstRevealedCard = -1;
    }


    public Board Board
    {
        get { return m_Board; }
        set { m_Board = value; }
    }

    public List<Player> Players
    {
        get { return m_Players; }
        set { m_Players = value; }
    }

    public int CurrentPlayerIndex
    {
        get { return m_CurrentPlayerIndex; }
        set
        {
            //if(value >= 2) //FIX!!!!!!!
            //    value = 0;
            m_CurrentPlayerIndex = value;
        }
    }

    public eGameMode CurrentGameMode
    {
        get { return m_CurrentGameMode; }
        set { m_CurrentGameMode = value; }
    }

    public void AddPlayer(Player player)
    {
        m_Players.Add(player);
    }

    public void UpdateScore()
    {
        m_Players[CurrentPlayerIndex].Score += 1;
    }

    public void NoMatch(Card i_FirstCard, Card i_SecondCard)
    {
        i_FirstCard.Displayed = false;
        i_SecondCard.Displayed = false;
        if (m_CurrentPlayerIndex == m_Players.Count - 1) //לתקן לפי תקינות קוד
        {
            m_CurrentPlayerIndex = 0;
        }
        else
        {
            m_CurrentPlayerIndex += 1;
        }
    }

    public Card ChooseOneCardComputer()
    {
        Card selectedCard = null;

        if (m_FirstRevealedCard != -1)
        {
            List<Position> positions = m_CardMemory[m_FirstRevealedCard];
            Position position = positions[0];
            positions.RemoveAt(0);
            if (positions.Count == 0)
            {
                m_CardMemory.Remove(m_FirstRevealedCard);
            }
            selectedCard = m_Board.GetCardInIndex(position.Row, position.Col);
            selectedCard.Displayed = true;
            m_FirstRevealedCard = -1;
        }

        else
        {
            foreach (int value in m_CardMemory.Keys)
            {
                List<Position> positions = m_CardMemory[value];
                if (positions.Count > 1)
                {
                    m_FirstRevealedCard = value;
                    Position currentPosition = positions[0];
                    m_CardMemory[value].Remove(positions[0]);
                    selectedCard = m_Board.GetCardInIndex(currentPosition.Row, currentPosition.Col);
                    selectedCard.Displayed = true;
                    break;
                }
            }

            if (selectedCard == null)
            {
                Random rnd = new Random();
                List<Position> hiddenPositions = new List<Position>();

                for (int row = 0; row < m_Board.Height; row++)
                {
                    for (int col = 0; col < m_Board.Width; col++)
                    {
                        Card card = m_Board.GetCardInIndex(row, col); //new
                        if (!card.Displayed && !IsCardInMemory(card)) //new
                        {
                            hiddenPositions.Add(new Position(row, col));
                        }
                    }
                }

                if (hiddenPositions.Count > 0)
                {
                    Position randomPosition = hiddenPositions[rnd.Next(hiddenPositions.Count)];
                    selectedCard = m_Board.GetCardInIndex(randomPosition.Row, randomPosition.Col);
                    selectedCard.Displayed = true;

                    if (m_CardMemory.ContainsKey(selectedCard.Value - 'A'))
                    {
                        if (selectedCard.Row != m_CardMemory[selectedCard.Value - 'A'][0].Row || selectedCard.Col != m_CardMemory[selectedCard.Value - 'A'][0].Col)
                        {
                            m_FirstRevealedCard = selectedCard.Value - 'A';   /////לתקןןןןןןןן!
                        }
                        AddCardToMemory(selectedCard.Value - 'A', randomPosition.Row, randomPosition.Col); //לתקןןןןןןןןן
                    }
                    else
                    {
                        AddCardToMemory(selectedCard.Value - 'A', randomPosition.Row, randomPosition.Col); //לתקןןןןןןןן
                    }
                }
            }
        }

        return selectedCard;
    }

    //public void AddCardToMemory(int i_Value, int i_Row, int i_Col)
    //{
    //    Position newPosition = new Position(i_Row, i_Col);

    //    if (m_CardMemory.ContainsKey(i_Value) && )
    //    {
    //        if ((m_CardMemory[i_Value][0].Row != i_Row || m_CardMemory[i_Value][0].Col != i_Col) && m_CardMemory[i_Value].Count != 2)
    //        { 
    //            List<Position> listPositions = m_CardMemory[i_Value];
    //            listPositions.Add(newPosition);
    //        }
    //    }
    //    else if (!m_CardMemory.ContainsKey(i_Value))
    //    {
    //        List<Position> listPositions = new List<Position>();
    //        listPositions.Add(newPosition);
    //        m_CardMemory.Add(i_Value, listPositions);
    //    }
    //}

    public void AddCardToMemory(int i_Value, int i_Row, int i_Col)
    {
        Position newPosition = new Position(i_Row, i_Col);

        if (m_CardMemory.ContainsKey(i_Value))
        {
            if (m_CardMemory[i_Value].Count == 1)
            {
                if ((m_CardMemory[i_Value][0].Row != i_Row) || (m_CardMemory[i_Value][0].Col != i_Col))
                {
                    m_CardMemory[i_Value].Add(newPosition);
                }
            }
        }
        else
        {
            List<Position> listPositions = new List<Position>();
            listPositions.Add(newPosition);
            m_CardMemory.Add(i_Value, listPositions);
        }
    }

    private bool IsCardInMemory(Card card)
    {
        bool isCardInMemory = false;
        int cardValue = card.Value - 'A'; //////לתקן 

        if (m_CardMemory.TryGetValue(cardValue, out List<Position> positions))
        {
            foreach (Position pos in positions)
            {
                if (pos.Row == card.Row && pos.Col == card.Col)
                {
                    isCardInMemory = true;
                }
            }
        }

        return isCardInMemory;
    }
}

