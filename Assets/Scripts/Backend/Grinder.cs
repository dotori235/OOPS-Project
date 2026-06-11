namespace Backend
{
    public class Grinder : Machine
    {
        private void Awake()
        {
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
