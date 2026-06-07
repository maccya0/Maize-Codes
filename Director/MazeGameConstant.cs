
namespace MazeGame
{
    public static class MazeGameConstants
    {
        public static class PlayerConstants
        {
            public const float Height = 1.0f + MazeConstants.rootY;
            public const string Tag = "Player";
            public const string Layer = "Player";
        }
        public static class EnemyConstants
        {
            public const float Height = 0.5f + MazeConstants.rootY;
            public const string Tag = "Enemy";
        }
        public static class InputConstants
        {
            public const float deadInputVal = 0.4f;
        }

        public static class GameConstant
        {
            public const float lightMinVal = 0.3f;
            public const float lightMaxVal = 1.0f;

        }

        public static class MazeConstants
        {
            // 背景との位置調整用
            public const float rootX = 450f;
            public const float rootY = 76f;
            public const float rootZ = 220f;
            public const string wallTag = "Wall";
            public const string indestructibleWallTag = "Indestructible";
            public enum GameState
            {
                None,
                Win,
                Lose,
            }
            public const float AngRange = 0.2f;
            public const float ObjHeight = 1.0f;
            public const float PosOffset = 1.0f;
            public const float StageHeightLimit = PosOffset + 5.0f;

            public enum MazeObjKinds
            {
                EPath,  //通路
                ETrapPath,  //罠の通路
                EUnBreakWall,   //破壊不可の壁
                EBreakWall, //破壊可能の壁
                ETrapWall,  //罠の壁
                EEnemyPos,  //エネミー設置個所
                EStart, //スタート位置
                EChecPoint, //チェックポイント位置
                EGoal,   //ゴール位置
                EItem   //アイテム位置
            }
            public enum Direct
            {
                North,
                East,
                South,
                West,
                Nothing,
                Siege
            };

        }

    }

}
