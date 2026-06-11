using UnityEngine;
using Backend;
using System.Collections.Generic;
public class BeltBlockManager : MonoBehaviour, IBeltTrackLevelObserver
{
    [SerializeField] private GameObject beltBlockPrefab;
    private List<BeltBlock> beltBlocks= new List<BeltBlock>();
    private BeltTrack beltTrack;
    
    private void Awake()
    {
        beltTrack = GetComponent<BeltTrack>();
    }
    private void Start()
    {
        beltTrack.RegisterObserver(this);
        for(int i = 0; i < beltTrack.GetMachineSpaces(); i++)
        {
            generateBeltBlock();
        }
    }

    public void generateBeltBlock()
    {
        GameObject go = Instantiate(beltBlockPrefab, transform);
        go.transform.localPosition = new Vector3((beltBlocks.Count + 1f), -0.5f, 0);
        beltBlocks.Add(go.GetComponent<BeltBlock>());
    }
    public void OnNotify(ISubject subject)
    {

    }
    public void OnBeltTrackLevelChanged(IBeltTrackLevelSubject subject)
    {
        generateBeltBlock();
    }
}
