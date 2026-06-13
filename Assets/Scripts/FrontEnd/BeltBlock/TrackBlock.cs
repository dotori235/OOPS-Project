using Backend;
using UnityEngine;

public class TrackBlock : BlockBase
{
    private BeltTrack _beltTrack;
    private FactoryStatus _factoryStatus;
    public BeltTrack BeltTrack { get => _beltTrack; private set => _beltTrack = value;  }
    private void Awake()
    {
        _beltTrack = GetComponent<BeltTrack>();
        
    }
    private void Start()
    {
        _factoryStatus = FactoryStatus.GetInstance();
    }
    public int TrackLevel { get => _beltTrack.Level; }
    public float TrackLevelUpPrice { get => _beltTrack.Level * 500f; }
    public override BlockUIType UIType()
    {
        return BlockUIType.TrackModify;
    }
    public void TrackLevelUp()
    {
        if (PayMoney(_beltTrack.LevelUpPrice))
        {
            _beltTrack.LevelUp();
        }

    }
    public bool PayMoney(float pay)
    {
        if(_factoryStatus.Money >= pay)
        {
            _factoryStatus.ModifyMoney(-pay);
            return true;
        }
        return false;
    }
}
