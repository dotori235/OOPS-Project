namespace Backend
{
    public class Welder : Machine
    {
        public override StatType GetTargetStat()
        {
            return StatType.Durability;
        }

        public override bool CanCauseDefect()
        {
            return true;
        }
    }
}
