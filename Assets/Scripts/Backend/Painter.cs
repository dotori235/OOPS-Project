namespace Backend
{
    public class Painter : Machine
    {
        protected override void Awake()
        {
            base.Awake();
            SetMachineType(MachineType.Painter);
        }
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
