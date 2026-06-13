using UnityEngine;
using Backend;
public class MachineHPSliderUIView : SliderUIView, IMachineObserver
{
    [SerializeField] private Machine _machine;
    protected override void Awake()
    {
        base.Awake();
        _machine.RegisterObserver(this);
    }
    public void OnMachineChanged(IMachineSubject machine)
    {
        Machine m = machine as Machine;
        if (m == null) return;
        base.SetValue(new SliderUpdateArgs(m.Hp, m.MaxHp));
    }
    public void OnNotify(ISubject subject)
    {

    }
}
