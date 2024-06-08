using System;
using System.Collections.Generic;
namespace MemoryGame;

class GameLogicManager
{
    private Board m_Board;
    private List<Player> m_Players;
    private int m_CurrentPlayerIndex;
    private eGameMode m_CurrentGameMode;
    private Dictionary<Card, List<Position>> m_CardMemory;
    private Card m_FirstRevealedCard;

    internal enum eGameMode //לבדוק תקינות קוד
    {
        HumanVsHuman,
        ComputerVsHuman
    }

    public GameLogicManager()
    {
        m_Players = new List<Player>();
        m_CurrentPlayerIndex = 0;
        m_CardMemory = new Dictionary<Card, List<Position>>();
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
        set { 
            if(value >= 2)
                value = 0;
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
        Players[CurrentPlayerIndex].Score += 1;
    }

    public void NoMatch(Card i_FirstCard, Card i_SecondCard)
    {
        i_FirstCard.Displayed = false;
        i_SecondCard.Displayed = false;
        CurrentPlayerIndex += 1;
    }

    // public Card ChooseOneCardComputer()
    // {
    //     foreach(Card card in m_CardMemory.Keys)
    //     {
    //         List<Position> positions = m_CardMemory[card];
    //         if(positions.Count > 1)
    //         {
    //             m_FirstRevealedCard = card;
    //             m_CardMemory[card].Remove(positions[0]);

    //         }
    //     }
        
    // }

    public void AddCardToMemory(Card i_Card, int i_Row, int i_Col)
    {
        Position newPosition = new Position(i_Row, i_Col);

        if (m_CardMemory.ContainsKey(i_Card))
        {
            List<Position> listPositions = m_CardMemory[i_Card];
            listPositions.Add(newPosition);
        }
        else
        {
            List<Position> listPositions = new List<Position>();
            listPositions.Add(newPosition);
            m_CardMemory.Add(i_Card, listPositions);
        }
    }
}

