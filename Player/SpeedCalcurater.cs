
namespace MazeGame
{
    public static class SpeedCalcurate
    {
        public static float AddSpeed(float _addSpeed, float currentSpeed)
        {
            return _addSpeed + currentSpeed;
        }
        public static float DownSpeed(float _addSpeed, float currentSpeed)
        {
            return currentSpeed - _addSpeed;
        }
    }

}
