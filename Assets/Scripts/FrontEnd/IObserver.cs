public interface IObserver
{
    public void OnNotify(ISubject subject);
}

public interface IMachineSelectButtonObserver : IObserver
{
    public void OnButtonSelected(MachineSelectButton clickedButton);
}