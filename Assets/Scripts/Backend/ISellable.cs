namespace Backend
{
    public interface ISellable
    {
        float CalculatePrice(float spMult);
        bool IsDefective { get; }
    }
}
