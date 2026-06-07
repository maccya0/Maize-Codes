
namespace MazeGame
{
    public class StatminaStatus
    {
        private float stamina;
        private float maxStamina;

        public StatminaStatus(float _maxStamina)
        {
            maxStamina = _maxStamina;
            stamina = _maxStamina;
        }


        public float GetMaxStamina()
        {
            return maxStamina;
        }

        public float GetStamina()
        {
            return stamina;
        }

        public void Resopone()
        {
            stamina = maxStamina;
        }

        public void SetStamina(float newStamina)
        { 
            stamina = newStamina;
        }
    }

}
