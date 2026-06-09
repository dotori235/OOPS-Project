public interface IObserver
{
    public void OnNotify(ISubject subject);
}
public interface IMachineButtonObserver : IObserver
{
    public void OnButtonSelected(IMachineButtonSubject button);
}


public interface IFactoryStatusObserver : IObserver
{
    public void OnFactoryStatusChanged(FactoryStatusType type, UIUpdateArgs arg);
}

public interface IBeltBlockObserver: IObserver
{
    public void OnBeltBlockChanged(IBeltBlockSubject beltBlock);
}

public interface IBeltTrackLevelObserver: IObserver
{
    public void OnBeltTrackLevelChanged();
}

public interface IRoundObserver: IObserver
{
    public void OnRoundChanged(IRoundSubject subject, UIUpdateArgs arg);
}