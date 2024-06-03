using System;
namespace MemoryGame;
class Board
{
    
    private int m_Height;
    private int m_Width;

     public int Height
    {
        get { return m_Height; }
        set 
        { 
            if(value >= 4 && value <= 6)
            {
                m_Height = value;
            }
            else
            {
                throw new ArgumentException("Height must be an even number.");
            }
        }
    }
        

    public int Width
    {
        get { return m_Width; }
        set 
        { 
            if(value >= 4 && value <= 6)
            {
                m_Width = value;
            }
            else
            {
               
            }
         }
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
    }
}