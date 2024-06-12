namespace MemoryGame;

internal class Card
{
    private readonly char r_Value;
    private bool m_Displayed;
    private readonly Position r_Position; 
    
    public Card(char value, int row, int col)
    {
        r_Value = value;
        m_Displayed = false;
        r_Position = new Position(row, col);
    }

    public char Value
    {
        get { return r_Value; }
    }

    public bool Displayed
    {
        get { return m_Displayed; }
        set { m_Displayed = value; }
    }

    public Position Position
    {
        get { return r_Position; }
    }

    public int CardValueToIndex()
    {
        return r_Value - 'A';
    }
}