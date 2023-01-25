using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeThief();

public class ThiefSpawner : ObjectPool<Thief>
{
    public ChangeThief OpenImage = null;
    public ChangeThief HideImage = null;
    public ChangeThief RemoveThief = null;

    [SerializeField] Thief thiefPref = null;
    public List<ThiefData> ThiefDatas { get; private set; }
    List<int> wantedlist = new List<int>();

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
        wantedlist.Clear();
        OpenImage = null;
        HideImage = null;
        RemoveThief = null;

        int wantedCount = stage.wantedCount;
        Debug.Log($"{stage.wantedCount} // {stage.thiefCount}");
        bool wanted = true;

        for (int i = 0; i < stage.thiefCount; i++)
        {
            if (wantedCount <= 0) wanted = false; 
            int random = Random.Range(0, ThiefDatas.Count);
            Thief thief = Get();
            thief.transform.position = new Vector3(Random.Range(-5f, 8f), Random.Range(-3f, 3f), 0);
            if (thief.transform.parent == null) thief.transform.parent = this.transform;
            thief.Setting(wanted, ThiefDatas[random].id, ThiefDatas[random].moveSpeed, ThiefDatas[random].moveTime);
            if (wanted)
            {
                wantedlist.Add(ThiefDatas[random].id);
            }
            thief.callbackArrest = Release;
            OpenImage += thief.OpenImage;
            HideImage += thief.HideImage;
            RemoveThief += thief.Remove;
            ThiefDatas.RemoveAt(random);
            wantedCount--;
        }
        UIManager.Instance.WantedListSetting(wantedlist);
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
