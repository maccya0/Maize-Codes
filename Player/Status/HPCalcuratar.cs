
namespace MazeGame
{
    public static class HPCalcuratar
    {

        public static short AddDamage(short currentHP, short damage)
        {
            if (damage < 0) return currentHP;
            currentHP -= damage;
            return currentHP;
        }

        public static short HealHp(short currentHP, short heal,short MAXHP)
        {
            currentHP += heal;
            if (currentHP >= MAXHP)
            {
                return MAXHP;
            }
            else
            {
                return currentHP;
            }
        }

    }

}
