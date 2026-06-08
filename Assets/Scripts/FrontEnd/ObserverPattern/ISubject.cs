using UnityEngine.Rendering;

public interface ISubject
{
    public void RegisterObserver(IObserver observer);
    public void UnregisterObserver(IObserver observer);
    public void NotifyObservers();
}

public interface IMachineSelectButtonSubject : ISubject{ }

public interface IFactoryStatusSubject : ISubject
{
    public void NotifyFactoryStatus(FactoryStatusType type, UIUpdateArgs arg);
}