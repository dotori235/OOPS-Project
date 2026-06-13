using Backend;

public interface IObserver
{
    public void OnNotify(ISubject subject);
}
public interface IUIPanelButtonObserver : IObserver
{
    public void OnButtonSelected(IUIPanelButtonSubject button);
}


public interface IFactoryStatusObserver : IObserver
{
    public void OnFactoryStatusChanged(FactoryStatusType type, UIUpdateArgs arg);
}

public interface IBlockObserver: IObserver
{
    public void OnBlockChanged(IBlockSubject beltBlock);
}

public interface IBeltTrackLevelObserver: IObserver
{
    public void OnBeltTrackLevelChanged(IBeltTrackLevelSubject subject);
}

public interface IRoundObserver: IObserver
{
    public void OnRoundChanged(IRoundSubject subject, UIUpdateArgs arg);
}

public interface ISellBlockObserver : IObserver
{
    public void OnSellBlockReached(ISellable item);
}
public interface IGameStateObserver : IObserver
{
    public void OnGameStateChanged(IGameState state);
}
public interface IMachineObserver : IObserver
{
    public void OnMachineChanged(IMachineSubject machine);
}