namespace Backend
{
    public class RoundEndEvent : GameEvent
    {
        public float AvgAP { get; private set; }
        public bool Passed { get; private set; }

        public RoundEndEvent(float avgAP, bool passed)
        {
            AvgAP = avgAP;
            Passed = passed;
        }
    }
}
