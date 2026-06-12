namespace Backend
{
    public class Grinder : Machine
    {
        protected override void Awake()
        {
            base.Awake();
            SetMachineType(MachineType.Grinder);
        }
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
