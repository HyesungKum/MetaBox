using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolCP;
using Unity.VisualScripting;

public delegate void ChangeThief();

public class ThiefSpawner : MonoBehaviour
{
    public ChangeThief OpenImage = null;
    public ChangeThief HideImage = null;
    public ChangeThief RemoveThief = null;

    [SerializeField] Thief thiefPref = null;
    public List<ThiefData> ThiefDatas { get; private set; }

    private void Awake()
    {
        //PoolCp.Inst.BringObjectCp(thiefPref);
    }
    
    public void Spawn(StageData stage)
    {
        ThiefDatas = DataManager.Instance.FindThiefDatasByThiefGroup(stage.thiefGroup);

        int wantedCount = stage.wantedCount;
        bool wanted = true;

        for (int i = 0; i < stage.thiefCount; i++)
        {
            if (wantedCount <= 0) wanted = false; 
            int random = Random.Range(0, ThiefDatas.Count);
            Thief thief = Instantiate<Thief>(thiefPref, new Vector3(Random.Range(-5f, 8f), Random.Range(-3f, 3f), 0), Quaternion.identity, this.transform);
            thief.Setting(wanted, ThiefDatas[random].id, ThiefDatas[random].moveSpeed, ThiefDatas[random].moveTime);
            OpenImage += thief.OpenImage;
            HideImage += thief.HideImage;
            RemoveThief += thief.Remove;
            wantedCount--;
        }
    }

    public void Open()
    {
        if(OpenImage != null) OpenImage();
    }
    public void Hide()
    {
        if(HideImage != null) HideImage();
    }
    public void Remove()
    {
        if(RemoveThief != null) RemoveThief();
    }
}
