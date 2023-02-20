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

    [Header("Spawn Setting")]
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

        StartCoroutine(nameof(SpawnerAwake));
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReceiver.GameStart -= SpawnStart;
    }
    //======================================spawn Timing Controll=================================================
    /// <summary>
    /// spawner Initializing routine
    /// </summary>
    /// <returns> until wait spawning routine when data getting done </returns>
    IEnumerator SpawnerAwake()
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
        EventReceiver.GameStart += SpawnStart;
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
            yield return null;

            timer += Time.deltaTime;

            if (timer > spawnTime)
            {
                //refill temp Table
                if (TempTable.Count == 0) TempTable = SpawnTable.ToArray().ToList();

                //random index
                int index = Random.Range(0, TempTable.Count);

                //spawn gameobj
                if(Spawn(index) == null) continue;

                //spawn table controll
                TempTable.RemoveAt(index);

                //timer reset
                timer = 0f;
            }
        }
    }
    private GameObject Spawn(int index)
    {
        try
        {
            GameObject instObj = PoolCp.Inst.BringObjectCp(TempTable[index]);
            instObj.transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);
            return instObj;
        }
        catch
        {
            Debug.Log("##Spawner Error : Cannot Found any spawning target");
            return null;
        }
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
