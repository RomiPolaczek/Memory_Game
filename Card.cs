using System;
namespace MemoryGame;

class Card
{
    private char m_Value;
    private bool m_Displayed;
    private int m_Row; //new
    private int m_Col; //new

    public Card(char value, int row, int col)
    {
        m_Value = value;
        m_Displayed = false;
        m_Row = row; //new
        m_Col = col; //new
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

    public int Row //new
    { 
        get { return m_Row; }
        set { m_Row = value; }
    }

    public int Col //new
    {
        get { return m_Col; }
        set { m_Col = value; }
    }
}