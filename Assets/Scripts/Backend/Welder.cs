namespace Backend
{
    public class Welder : Machine
    {
        private void Awake()
        {
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
