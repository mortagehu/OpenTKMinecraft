namespace OpenTK_Minecraft;

class program
{
    static void Main(string[] args)
    {
        using(Game game = new Game(500, 500))
        {
            game.Run();
        }
    }
    
}