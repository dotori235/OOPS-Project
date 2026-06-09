using UnityEngine.Rendering;
using System.Collections.Generic;
public interface ISubject
{
    public void RegisterObserver(IObserver observer);
    public void UnregisterObserver(IObserver observer);
    public void NotifyObservers();
}
public interface IMachineButtonSubject : ISubject
{

}

public interface IFactoryStatusSubject : ISubject
{
    public void NotifyFactoryStatus(FactoryStatusType type, UIUpdateArgs arg);
}

public interface IBeltBlockSubject : ISubject
{
    public void NotifyBeltBlock();
}

public interface IBeltTrackLevelSubject : ISubject
{
    public void NotifyBeltTrackLevel();
}

public interface IRoundSubject : ISubject
{
    public void NotifyRound();
}