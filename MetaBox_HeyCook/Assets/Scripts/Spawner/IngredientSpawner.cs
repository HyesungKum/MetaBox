using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] CountingZone CountingZone;

    public SpawnTableData[] TableData;
    public List<GameObject> SpawnTable = new List<GameObject>();

    public float spawnTime = 1f;

    [field:SerializeField] int Level = 0;

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
        SpawnIngredient();
    }

    private void SpawnIngredient()
    {
        timer += Time.deltaTime;

        if (CountingZone.Counting < 3 && timer > spawnTime)
        {
            Spawn();
            timer = 0f;
        }
    }

    private void Spawn()
    {
        int index = Random.Range(0, SpawnTable.Count);

        GameObject instObj = Instantiate(SpawnTable[index],this.transform.position, Quaternion.identity);
        instObj.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 2f,ForceMode2D.Impulse);

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(this.transform.position, this.transform.localScale);
    }
#endif
}
