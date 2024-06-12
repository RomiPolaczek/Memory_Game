namespace MemoryGame;

internal class GameLogicManager
{
    private Board m_Board;
    private List<Player> m_Players;
    private int m_CurrentPlayerIndex;
    private eGameMode m_CurrentGameMode;
    private Dictionary<int, List<Position>> m_CardMemory;
    private int m_PossibleMatchValue;
    private int m_FirstSelectedCard;

    internal enum eGameMode 
    {
        HumanVsHuman,
        ComputerVsHuman
    }

    public GameLogicManager()
    {
        m_Players = new List<Player>();
        m_CurrentPlayerIndex = 0;
        m_CardMemory = new Dictionary<int, List<Position>>();
        m_PossibleMatchValue = -1;
        m_FirstSelectedCard = -1;
    }

    public Board Board
    {
        get { return m_Board; }
        set { m_Board = value; }
    }

    public List<Player> Players
    {
        get { return m_Players; }
        set { m_Players = value; }
    }
    
    public int CurrentPlayerIndex
    {
        get { return m_CurrentPlayerIndex; }
        set { m_CurrentPlayerIndex = value; }
    }

    public eGameMode CurrentGameMode
    {
        get { return m_CurrentGameMode; }
        set { m_CurrentGameMode = value; }
    }

    public int PossibleMatchValue
    {
        get { return m_PossibleMatchValue; }
        set { m_PossibleMatchValue = value; }            
    }

    public int FirstSelectedCard
    {
        get { return m_FirstSelectedCard; }
        set { m_FirstSelectedCard = value; }
    }

    public void AddPlayer(Player player)
    {
        m_Players.Add(player);
    }

    public void UpdateScore()
    {
        m_Players[CurrentPlayerIndex].Score += 1;
    }

    public void NoMatch(Card i_FirstCard, Card i_SecondCard)
    {
        i_FirstCard.Displayed = false;
        i_SecondCard.Displayed = false;
        m_CurrentPlayerIndex = (m_CurrentPlayerIndex + 1) % m_Players.Count;
    }

    public Card ChooseOneCardComputer()
    {
        Card selectedCard = null;

        if (m_PossibleMatchValue != -1)
        {
           findSecondCardOfTheMatch(out selectedCard);
        }
        else
        {
            findMatchingCouple(ref selectedCard);

            if (selectedCard == null)
            {
                List<Position> hiddenPositions = new List<Position>();

                createHiddenPositionList(hiddenPositions);

                if (hiddenPositions.Count > 0)
                {
                    chooseRandomCard(hiddenPositions, out selectedCard);
                }
            }
        }

        return selectedCard;
    }

    private void findSecondCardOfTheMatch(out Card o_SelectedCard)
    {
        List<Position> positions = m_CardMemory[m_PossibleMatchValue];
        Position position = positions[0];

        m_CardMemory.Remove(m_PossibleMatchValue);
        o_SelectedCard = m_Board.GetCardInIndex(position.Row, position.Col);
        o_SelectedCard.Displayed = true;
        m_PossibleMatchValue = -1;
    }

    private void findMatchingCouple(ref Card o_SelectedCard)
    {
        foreach (int value in m_CardMemory.Keys)
        {
            List<Position> positions = m_CardMemory[value];

            if (positions.Count > 1)
            {
                m_PossibleMatchValue = value;
                Position currentPosition = positions[0];
                m_CardMemory[value].Remove(positions[0]);
                o_SelectedCard = m_Board.GetCardInIndex(currentPosition.Row, currentPosition.Col);
                o_SelectedCard.Displayed = true;
                break;
            }
        }
    }

    private void createHiddenPositionList(List<Position> o_ListPosition)
    {
        for (int row = 0; row < m_Board.Height; row++)
        {
            for (int col = 0; col < m_Board.Width; col++)
            {
                Card card = m_Board.GetCardInIndex(row, col); 

                if (!card.Displayed && !isCardInMemory(card))
                {
                    o_ListPosition.Add(new Position(row, col));
                }
            }
        }
    }

    private void chooseRandomCard(List<Position> i_HiddenPositions, out Card o_SelectedCard)
    {
        Random rnd = new Random();
        Position randomPosition = i_HiddenPositions[rnd.Next(i_HiddenPositions.Count)];

        o_SelectedCard = m_Board.GetCardInIndex(randomPosition.Row, randomPosition.Col);
        o_SelectedCard.Displayed = true;

        int cardValue = o_SelectedCard.CardValueToIndex();

        if (m_CardMemory.ContainsKey(cardValue))
        {
            m_PossibleMatchValue = cardValue;
        }
        
        AddCardToMemory(cardValue, randomPosition.Row, randomPosition.Col); 

        if (m_CardMemory[cardValue].Count == 2 && m_FirstSelectedCard == cardValue)
        {
            m_CardMemory.Remove(cardValue);
        }
    }
    
    public void AddCardToMemory(int i_Value, int i_Row, int i_Col)
    {
        Position newPosition = new Position(i_Row, i_Col);

        if (m_CardMemory.ContainsKey(i_Value))
        {
            if (m_CardMemory[i_Value].Count == 1)
            {
                if ((m_CardMemory[i_Value][0].Row != i_Row) || (m_CardMemory[i_Value][0].Col != i_Col))
                {
                    m_CardMemory[i_Value].Add(newPosition);
                }
            }
        }
        else
        {
            List<Position> listPositions = new List<Position>();

            listPositions.Add(newPosition);
            m_CardMemory.Add(i_Value, listPositions);
        }
    }

    private bool isCardInMemory(Card i_Card)
    {
        bool isCardInMemory = false;
        int cardValue = i_Card.CardValueToIndex(); 

        if (m_CardMemory.TryGetValue(cardValue, out List<Position> positions))
        {
            foreach (Position pos in positions)
            {
                if (pos.Row == i_Card.Position.Row && pos.Col == i_Card.Position.Col)
                {
                    isCardInMemory = true;
                }
            }
        }

        return isCardInMemory;
    }

    public void DeleteHumanMatchFromMemory(int i_Value)
    {
        if (m_CardMemory.ContainsKey(i_Value))
        {
            m_CardMemory.Remove(i_Value);
        }
    }
}

