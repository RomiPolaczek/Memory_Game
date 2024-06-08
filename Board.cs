using System;
namespace MemoryGame;
class Board
{
    private int m_Height;
    private int m_Width;
    private Card[,] m_CardsMatrix;
        
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

    public Card[,] CardsMatrix
    {
        get { return m_CardsMatrix; }
    }

    public Card GetCardInIndex(int i_row, int i_col)
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

     public Board(int height, int width)
    {
        m_Height = height;
        m_Width = width;
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        m_CardsMatrix = new Card[m_Height, m_Width];
    }

    public bool AreAllCardsDisplayed()
    {
        bool allCardsDisplayed = true;
        for (int i = 0; i < m_Height; i++)
        {
            for (int j = 0; j < m_Width; j++)
            {
                if(!m_CardsMatrix[i,j].Displayed)
                {
                    allCardsDisplayed = false;
                }
            }
        }

        return allCardsDisplayed;
    }
}
