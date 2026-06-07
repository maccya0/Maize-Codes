using UnityEngine;
using UnityEngine.PlayerLoop;
using static MazeGame.MazeEventConstants;
using static MazeGame.MazeGameConstants.MazeConstants;

namespace MazeGame
{
    public class GameManager: Singleton<GameManager>
    {
        private GameState gameState;
        private bool gameSetFlag;

        public void InitGame()
        {
            gameSetFlag = false;
            gameState = GameState.None;
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public void SetGameState(GameState _gameState)
        {
            if(gameSetFlag) return;
            if(_gameState != GameState.None)
            {
                gameState = _gameState;
                gameSetFlag = true;
            }
        }

        public void CheckGameState()
        {
            switch (gameState)
            {
                case GameState.None:
                    break;
                case GameState.Win:
                    // Ÿ—˜‚Ìˆ—
                    break;
                case GameState.Lose:
                    // ”s–k‚Ìˆ—
                    break;
            }
        }
    }

}
