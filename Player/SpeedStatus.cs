

namespace MazeGame
{
    public class SpeedStatus
    {
        private float speed;
        private float initSpeed;
        private float minSpeed;
        private float maxSpeed;
        private SpeedModifire SpeedModifire;

        public SpeedStatus(float _initSpeed, float minSpeed, float maxSpeed, SpeedModifire speedModifire)
        {
            initSpeed = _initSpeed;
            speed = _initSpeed;
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
            SpeedModifire = speedModifire;

            // ƒ‰ƒ€ƒ_Ž®‚ÅŒvŽZŒ‹‰Ê‚ÌˆêŠ‡ƒoƒbƒ`ˆ—‚ð“o˜^‚·‚é
            SpeedModifire.OnChanged += () => 
            {
                float sum = SpeedModifire.GetSumData();
                SetSpeed(sum+ _initSpeed);
            };
        }

        public bool IsDebaff()
        {
            return speed < initSpeed;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public float GetInitSpeed()
        {
            return initSpeed;
        }

        public void Resopone()
        {
            speed = initSpeed;
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            if (speed < minSpeed)
            {
                speed = minSpeed;
            }
        }

    }

}
