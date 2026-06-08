namespace Backend
{
    public class Grinder : Machine
    {
        public override StatType GetTargetStat()
        {
            return StatType.AttackPower;
        }

        public override bool CanCauseDefect()
        {
            return true;
        }
    }
}
