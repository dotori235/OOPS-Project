using NUnit.Framework;
using UnityEngine;
using Backend;
using System.Collections.Generic;
public class BeltBlockManager : MonoBehaviour
{
    [SerializeField] private GameObject beltBlockPrefab;
    private List<BeltBlock> beltBlocks;
    private BeltTrack beltTrack;
    private void Awake()
    {
        beltTrack = GetComponent<BeltTrack>();
        beltBlocks = new List<BeltBlock>();
    }
    private void Start()
    {
        for(int i = 0; i < beltTrack.GetMachineSpaces(); i++)
        {
            generateBeltBlock();
        }
    }

    public void generateBeltBlock()
    {
        GameObject go = Instantiate(beltBlockPrefab);
        go.transform.position = new Vector3(1, 0, 0) * (beltBlocks.Count+0.5f);
        beltBlocks.Add(go.GetComponent<BeltBlock>());
    }
}
