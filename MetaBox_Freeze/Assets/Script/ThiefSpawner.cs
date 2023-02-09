using System.Collections.Generic;
using UnityEngine;

public class ThiefSpawner : ObjectPool<Thief>
{
    [SerializeField] ScriptableObj scriptableNPC = null;

    List<ThiefData> ThiefDatas = null;

    public override Thief CreatePool()
    {
        return scriptableNPC.NPC[0];
    }

    private void Awake()
    {
        GameManager.Instance.spawnThief = Spawn;
        PoolInit();
    }
    
    public void Spawn()
    {
        StageData CurStage = GameManager.Instance.StageDatas[GameManager.Instance.CurStage];
        ThiefDatas = DataManager.Instance.FindThiefDatasByThiefGroup(CurStage.thiefGroup);

        int wantedCount = CurStage.wantedCount;
        bool wanted = true;

        for (int i = 0; i < CurStage.thiefCount; i++)
        {
            if (wantedCount <= 0) wanted = false; 
            int random = Random.Range(0, ThiefDatas.Count);

            Thief thief = Get();
            thief.transform.position = new Vector3(Random.Range(-7.5f, 4f), Random.Range(-3.5f, 1.8f), 0);
            thief.Setting(wanted, ThiefDatas[random].id, ThiefDatas[random].moveSpeed, ThiefDatas[random].moveTime);
            thief.callbackArrest = Release;

            ThiefDatas.RemoveAt(random);
            wantedCount--;
        }
    }
}
