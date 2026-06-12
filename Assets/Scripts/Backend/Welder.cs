namespace Backend
{
    public class Welder : Machine
    {
        protected override void Awake()
        {
            base.Awake();
            SetMachineType(MachineType.Welder);
        }

        public override StatType GetTargetStat()
        {
            return StatType.Durability;
        }

        public override bool CanCauseDefect()
        {
            return false;
        }
    }
}
