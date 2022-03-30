namespace FightGameInterface {
    public enum GameState {
        PlayerChoice,
        InGame,
        Win,
        Defeat,
        Draw
    }

    public enum InGameState {
        PlayerAction,
        ExecuteAction
    }
}