using System;
namespace MemoryGame;

class GameLogicManager<T>
{
    private Board<T> m_Board;
    private Player m_Player;

    public Board<T> Board
    {
        get { return m_Board; }
        set { m_Board = value; }
    }

    public Player Player
    {
        get { return m_Player; }
        set { m_Player = value; }
    }
   
}

