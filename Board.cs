namespace MemoryGame;
internal class Board
{
    private readonly int r_Height;
    private readonly int r_Width;
    private Card[,] m_CardsMatrix;

    public Board(int height, int width)
    {
        r_Height = height;
        r_Width = width;
        initializeBoard();
    }

    private void initializeBoard()
    {
        m_CardsMatrix = new Card[r_Height, r_Width];
    }

    public int Height
    {
        get { return r_Height; }
    }

    public int Width
    {
        get { return r_Width; }
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

        if ((r_Height * r_Width) % 2 == 0)
        {
            isBoardSizeEven = true;
        }

        return isBoardSizeEven;
    }

    public bool AreAllCardsDisplayed()
    {
        bool allCardsDisplayed = true;

        for (int i = 0; i < r_Height; i++)
        {
            for (int j = 0; j < r_Width; j++)
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
