using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void ChangeThief();

public class ThiefSpawner : ObjectPool<Thief>
{
    public ChangeThief OpenImage = null;
    public ChangeThief HideImage = null;
    public ChangeThief RemoveThief = null;

    [SerializeField] Thief thiefPref = null;
    public List<ThiefData> ThiefDatas { get; private set; }

    public override Thief CreatePool()
    {
        if (thiefPref == null) thiefPref = Resources.Load<Thief>(nameof(Thief));
        return thiefPref;
    }

    private void Awake()
    {
        PoolInit();
    }
    
    public void Spawn(StageData stage)
    {
        ThiefDatas = DataManager.Instance.FindThiefDatasByThiefGroup(stage.thiefGroup);
        OpenImage = null;
        HideImage = null;
        RemoveThief = null;

        int wantedCount = stage.wantedCount;
        Debug.Log($"{stage.wantedCount} // {stage.thiefCount}");
        bool wanted = true;

        for (int i = 0; i < stage.thiefCount; i++)
        {
            Debug.Log("µµµÏ¸¸µé±â");
            if (wantedCount <= 0) wanted = false; 
            int random = Random.Range(0, ThiefDatas.Count);
            Thief thief = Get();
            thief.transform.position = new Vector3(Random.Range(-5f, 8f), Random.Range(-3f, 3f), 0);
            if (thief.transform.parent == null) thief.transform.parent = this.transform;
            thief.Setting(wanted, ThiefDatas[random].id, ThiefDatas[random].moveSpeed, ThiefDatas[random].moveTime);
            thief.callbackArrest = Release;
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
