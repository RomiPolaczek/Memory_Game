using System;
namespace MemoryGame;
class Player 
{
    private string m_Name;

    public Player(string i_name)
    {
        m_Name = i_name;
    }
    

    public string Name
    {
        get { return m_Name ; }
        set { m_Name = value; }
    }
}
