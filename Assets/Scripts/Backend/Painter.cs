namespace Backend
{
    public class Painter : Machine
    {
        public override StatType GetTargetStat()
        {
            return StatType.Splendor;
        }

        public override bool CanCauseDefect()
        {
            return true;
        }
    }
}
