using System;
namespace MemoryGame;
class Player 
{
    private string m_Name;
    private int m_Score;
    private ePlayerTypes m_PlayerType;
    internal enum ePlayerTypes
    {
        Human,
        Computer
    }

    public Player(string i_Name, ePlayerTypes i_PlayerType)
    {
        m_Name = i_Name;
        m_Score = 0;
        m_PlayerType = i_PlayerType;
    }

    public string Name
    {
        get { return m_Name ; }
        set { m_Name = value; }
    }

    public string Score
    {
        get { return m_Name ; }
        set { m_Name = value; }
    }

    public ePlayerTypes PlayerType
    {
        get { return m_PlayerType ; }
        set { m_PlayerType = value; }
    }
}
