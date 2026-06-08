public interface IObserver
{
    public void OnNotify(ISubject subject);
}

public interface IMachineSelectButtonObserver : IObserver
{
    public void OnButtonSelected(MachineSelectButton clickedButton);
}

public interface IFactoryStatusObserver : IObserver
{
    public void OnFactoryStatusChanged(FactoryStatusType type, UIUpdateArgs arg);
}