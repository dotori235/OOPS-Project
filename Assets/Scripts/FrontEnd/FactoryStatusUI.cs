using Backend;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FactoryStatusUI : MonoBehaviour, IFactoryStatusObserver
{
    [SerializeField] private Dictionary<FactoryStatusType, UIView> uiDict;
    [SerializeField] private List<FactoryStatusType> factoryStatusTypes;
    [SerializeField] private List<UIView> uiViews;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        uiDict = new Dictionary<FactoryStatusType, UIView>();
        for (int i = 0; i < factoryStatusTypes.Count; i++)
        {
            uiDict.Add(factoryStatusTypes[i], uiViews[i]);
        }
        FactoryStatus.GetInstance().RegisterObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNotify(ISubject subject)
    {

    }
    public void OnFactoryStatusChanged(FactoryStatusType type, float value)
    {
        uiDict[type].SetValue(value);
    }
}
