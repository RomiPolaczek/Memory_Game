using System;
namespace MemoryGame;

class Card
{
    private char m_Value;
    private bool m_Displayed;

    public Card(char value)
    {
        m_Value = value;
        m_Displayed = false;
    }

    public char Value
    {
        get {return m_Value;}
        set{m_Value = value;}
    }

    public bool Displayed
    {
        get {return m_Displayed;}
        set{m_Displayed = value;}
    }
}