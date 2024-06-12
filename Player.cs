namespace MemoryGame;
internal class Player 
{
    private readonly string r_Name;
    private int m_Score;
    private readonly ePlayerType r_PlayerType;
    internal enum ePlayerType
    {
        Human,
        Computer
    }

    public Player(string i_Name, ePlayerType i_PlayerType)
    {
        r_Name = i_Name;
        m_Score = 0;
        r_PlayerType = i_PlayerType;
    }

    public string Name
    {
        get { return r_Name; }
    }

    public int Score
    {
        get { return m_Score; }
        set { m_Score = value; }
    }

    public ePlayerType PlayerType
    {
        get { return r_PlayerType; }
    }
}
