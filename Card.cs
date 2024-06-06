using System;
namespace MemoryGame;

class Card<T>
{
    private T m_Value;
    private bool m_Displayed;

    public Card(T value)
    {
        m_Value = value;
        m_Displayed = false;
    }

    public T Value
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