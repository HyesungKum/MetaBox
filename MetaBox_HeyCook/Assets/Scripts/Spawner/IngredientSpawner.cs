using ObjectPoolCP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    //===============table data========================
    [Header("Spawn Data Table")]
    public SpawnTableData[] TableData;

    [Header("Current Spawn Table")]
    public List<GameObject> SpawnTable;
    public List<GameObject> TempTable;

    //================moving belt=======================
    [Header("Ref Up Belt Scroll")]
    [SerializeField] BeltZone BeltZone;

    public float spawnTime;

    //===============init spawn=========================
    [Header("Init Spawn")]
    [SerializeField] int spawnCount;

    //================inner variables===================
    private float timer = 0f;
    
    private void Awake()
    {
        //apply Level
        switch (gameManager.Level)
        {
            case 1: SpawnTable = TableData[0].SpawnTable;
                break;
            case 2: SpawnTable = TableData[1].SpawnTable;
                break;
            case 3: SpawnTable = TableData[2].SpawnTable;
                break;
            case 4: SpawnTable = TableData[3].SpawnTable;
                break;
        }

        //Init Table
        TempTable = SpawnTable.ToArray().ToList();

        //Init Spawn
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < spawnCount; i++)
        {
            for (int j = 0; j < SpawnTable.Count; j++)
            {
                list.Add(Spawn(j));
            }
        }

        //Init Pool
        for (int i = 0; i < list.Count; i++)
        {
            PoolCp.Inst.DestoryObjectCp(list[i]);
        }

        //Init Time
        timer = spawnTime;

        //delegate chain
        EventReciver.GameStart += SpawnStart;
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReciver.GameStart -= SpawnStart;
    }

    //=====================================Spawn Reference Object using ObjPool===================================
    void SpawnStart()
    {
        StartCoroutine(nameof(TimingSpawn));
    }
    IEnumerator TimingSpawn()
    {
        while (this.gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > spawnTime)
            {
                //random index
                int index = UnityEngine.Random.Range(0, TempTable.Count);

                //spawn gameobj
                Spawn(index);

                //spawn table controll
                TempTable.RemoveAt(index);
                if (TempTable.Count == 0) TempTable = SpawnTable.ToArray().ToList();

                //timer reset
                timer = 0f;
            }

            yield return null;
        }
    }
    private GameObject Spawn(int index)
    {
        GameObject instObj = PoolCp.Inst.BringObjectCp(TempTable[index]);
        instObj.transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);
        return instObj;
    }

    #region Editor Gizmo
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 0f, 1f, 0.4f);
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
#endif
    #endregion
}
