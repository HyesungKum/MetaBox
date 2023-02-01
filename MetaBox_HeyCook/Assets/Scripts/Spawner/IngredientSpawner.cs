using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    //===============table data========================
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

    //=================caching==========================
    WaitUntil waitDateGet;

    private void Awake()
    {
        waitDateGet = new WaitUntil(()=> SpawnTable != null);

        StartCoroutine(nameof(SpawnerInit));
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReciver.GameStart -= SpawnStart;
    }
    //======================================spawn Timing Controll=================================================
    /// <summary>
    /// spawner Initializing routine
    /// </summary>
    /// <returns> until wait spawning routine when data getting done </returns>
    IEnumerator SpawnerInit()
    {
        yield return waitDateGet;

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
                int index = Random.Range(0, TempTable.Count);

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
        //try
        //{
            GameObject instObj = PoolCp.Inst.BringObjectCp(TempTable[index]);
            instObj.transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);
            return instObj;
        //}
        //catch
        //{
        //    Debug.LogError("##Spawner Error : Cannot Found any spawning target");
        //    return null;
        //}
    }

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 0f, 1f, 0.4f);
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, this.transform.localScale);
    }
#endif
    #endregion
}
