using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    //===============table data========================
    [Header("Spawn Data Table")]
    public SpawnTableData[] TableData;
    public List<GameObject> SpawnTable = new();

    //================moving belt=======================
    [Header("Ref Up Belt Scroll")]
    [SerializeField] BeltZone BeltZone;

    public int maxSpawn = 3;
    public float spawnTime = 1f;

    //================inner variables===================
    private float timer = 0f;
    
    private void Awake()
    {
        switch (GameManager.Inst.Level)
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
    }

    private void Update()
    {
        TimingSpawn();
    }

    private void TimingSpawn()
    {
        timer += Time.deltaTime;

        if (BeltZone.BeltIngred.Count < maxSpawn && timer > spawnTime)
        {
            Spawn();
            timer = 0f;
        }
    }
    private void Spawn()
    {
        int index = Random.Range(0, SpawnTable.Count);

        PoolCp.Inst.BringObjectCp(SpawnTable[index]).transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);
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
