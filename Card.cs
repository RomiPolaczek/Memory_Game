using System;
namespace MemoryGame;

class Card<T>
{
    private T m_Value;
    private bool m_Flipped;

    public Card(T value)
    {
        m_Value = value;
        m_Flipped = false;
    }

    public T Value
    {
        get {return m_Value;}
        set{m_Value = value;}
    }

    public bool Flipped
    {
        get {return m_Flipped;}
        set{m_Flipped = value;}
    }

}