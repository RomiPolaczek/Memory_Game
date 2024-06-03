using System;
namespace MemoryGame;

class Program
{   
    public static void Main()
    {
        UserInterface UI = new UserInterface();
    
        UI.SetBoardSize();
        UI.SetPlayersName();
        UI.DrawBoard();

    }

}