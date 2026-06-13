using UnityEngine.Rendering;
using System.Collections.Generic;
using Backend;
public interface ISubject
{
    public void RegisterObserver(IObserver observer);
    public void UnregisterObserver(IObserver observer);
    public void NotifyObservers();
}
public interface IUIPanelButtonSubject : ISubject
{

}

public interface IFactoryStatusSubject : ISubject
{
    public void NotifyFactoryStatus(FactoryStatusType type, UIUpdateArgs arg);
}

public interface IBlockSubject : ISubject
{
    public void NotifyBlock();
}

public interface IBeltTrackLevelSubject : ISubject
{
    public void NotifyBeltTrackLevel();
}

public interface IRoundSubject : ISubject
{
    public void NotifyRound();
}
public interface ISellBlockSubject : ISubject
{
    public void NotifySellBlock(ISellable item);
}

public interface IGameStateSubject : ISubject
{
    public void NotifyGameState();
}
public interface IMachineSubject : ISubject
{
    public void NotifyMachine();
}

