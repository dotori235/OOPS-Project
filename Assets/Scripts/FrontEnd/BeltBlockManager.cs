using NUnit.Framework;
using UnityEngine;
using Backend;
using System.Collections.Generic;
public class BeltBlockManager : MonoBehaviour
{
    [SerializeField] private GameObject beltBlockPrefab;
    private List<BeltBlock> beltBlocks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        beltBlocks = new List<BeltBlock>();
    }
    void Start()
    {
        for(int i = 0; i < GetComponent<BeltTrack>().GetMachineSpaces(); i++)
        {
            generateBeltBlock();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void generateBeltBlock()
    {
        GameObject go = Instantiate(beltBlockPrefab);
        go.transform.position = new Vector3(1, 0, 0) * (beltBlocks.Count+0.5f);
        beltBlocks.Add(go.GetComponent<BeltBlock>());
    }
}
