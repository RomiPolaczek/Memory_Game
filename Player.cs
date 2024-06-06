using System;
namespace MemoryGame;
class Player 
{
    private string m_Name;
    private int m_Score;

    public Player(string i_Name)
    {
        m_Name = i_Name;
        m_Score = 0;
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
}
