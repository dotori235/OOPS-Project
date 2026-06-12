using Backend;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStatusUI : MonoBehaviour, IFactoryStatusObserver
{
    [SerializeField] private Dictionary<FactoryStatusType, UIView> uiDict = new Dictionary<FactoryStatusType, UIView>();
    [SerializeField] private List<FactoryStatusType> factoryStatusTypes;
    [SerializeField] private List<UIView> uiViews;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        for (int i = 0; i < factoryStatusTypes.Count; i++)
        {
            uiDict.Add(factoryStatusTypes[i], uiViews[i]);
        }
        FactoryStatus.GetInstance()?.RegisterObserver(this);
    }
    private void OnDestroy()
    {
        FactoryStatus.GetInstance()?.UnregisterObserver(this);
    }

    public void OnNotify(ISubject subject)
    {

    }
    public void OnFactoryStatusChanged(FactoryStatusType type, UIUpdateArgs arg)
    {
        uiDict[type].SetValue(arg);
    }

}
