

namespace MazeGame
{
    public static class StaminaCalcurater
    {

        public static float AddStamina(float val, float currentStamina)
        {
            if (val < 0) return currentStamina;
            return currentStamina += val;
        }
        public static float DownStamina(float val, float currentStamina)
        {
            if (val < 0) return currentStamina;
            return currentStamina - val;
        }
    }
}
