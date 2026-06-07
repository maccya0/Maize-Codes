
namespace MazeGame
{
    public class HPStatus
    {
        private short hp;
        private short MaxHP;

        public HPStatus(short maxHP)
        {
            this.MaxHP = maxHP;
            hp =  maxHP;
        }

        public bool GetAlive()
        {
            return hp > 0;
        }
        public void SetHp(short newHp)
        {
            hp = newHp;
        }

        public short GetHp()
        {
            return hp;
        }
        public short GetMaxHp()
        {
            return MaxHP;
        }
        public void Resopone()
        {
            hp = MaxHP;
        }
    }

}
