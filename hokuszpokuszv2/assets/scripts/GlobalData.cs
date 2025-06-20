public class GlobalData
{
    public class GameData()
    {
        public static int _score = 0;
        public static int _hiScore = 0;
        public static GameState _gameState = GameState.Menu;
    }

    public enum GameState { Menu, Playing, GameOver }
}

/*
=== [ GDD ] ===

Layers
    1 - Map
    2 - Player
    3 - Sword
    4 - Enemy

*/