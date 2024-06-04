using System;
namespace MemoryGame;
class Board<T>
{
    private int m_Height;
    private int m_Width;
    private Card<T>[,] m_CardsMatrix;
        
    public int Height
    {
        get { return m_Height; }
        set { m_Height = value;}
    }

    public int Width
    {
        get { return m_Width; }
        set { m_Width = value;}
    }

    public Card<T>[,] CardsMatrix
    {
        get { return m_CardsMatrix; }
    }

    public Card<T> GetCardInIndex(int i_row, int i_col)
    {
        return m_CardsMatrix[i_row , i_col];
    }

    public bool IsBoardSizeEven()
    {
        bool isBoardSizeEven = false;
        if ((m_Height * m_Width) % 2 == 0)
        {
            isBoardSizeEven = true;
        }
        return isBoardSizeEven;
    }

     public Board(int height, int width, T[] values)
    {
        m_Height = height;
        m_Width = width;
        InitializeBoard(values);
    }

    public void InitializeBoard(T[] values)
    {
        m_CardsMatrix = new Card<T>[m_Height, m_Width];
        int valueIndex = 0;

        for (int i = 0; i < m_Height; i++)
        {
            for (int j = 0; j < m_Width; j++)
            {
                m_CardsMatrix[i, j] = new Card<T>(values[valueIndex++]);
            }
        }
    }
}
