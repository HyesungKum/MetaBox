using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RankingDB;
using MongoDB.Bson;
using UnityEngine.UI;
using System;

[Serializable]
public class playerData
{
    public int level;
    public int rank;
    public string id;
    public long point;
}

[Serializable]
public class levelSet
{
    public RectTransform easyPos;
    public RectTransform normalpos;
    public RectTransform hardpos;
    public RectTransform extremepos;

    public bool easy;
    public bool normal;
    public bool hard;
    public bool extreme;
}

public class TopTenRankMgr : MonoBehaviour
{
    #region
    [Header("[User Data Prefab]")]
    [SerializeField] GameObject userDataPrefab = null;

    [Header("[Player Ranking]")]
    [SerializeField] GameObject playerRanking = null;

    #region Freeze
    [Header("[Freeze]")]
    [SerializeField] public ScrollRect freezeScrollRect = null;
    [SerializeField] public RectTransform[] freezeLevelShowRanking = null;
    [Space]
    [SerializeField] Button[] freezeLevelButs = null;
    #endregion

    #region HeyCook
    [Header("[HeyCook")]
    [SerializeField] public ScrollRect heyCookScrollRect = null;
    [SerializeField] public RectTransform[] heyCookLevelShowPanking = null;
    [Space]
    [SerializeField] Button[] heyCookLevelButton = null;
    #endregion

    #region Melodia
    [Header("[Melodia]")]
    [SerializeField] public ScrollRect melodiaScrollRect = null;
    [SerializeField] public RectTransform[] melodiaShowRanking = null;
    [Space]
    [SerializeField] Button[] melodiaLevelButtons;
    #endregion

    #region SketchUP
    [Header("[SketchUP]")]
    [SerializeField] public ScrollRect sketchUPScrollRect = null;
    [SerializeField] public RectTransform[] sketchUPLevelShowRanking = null;
    [Space]
    [SerializeField] Button[] sketchUPLevelButtons;
    #endregion

    levelSet levelset;
    UserDatePrefab userDataSet = null;

    GameObject instData = null;
    [Space]
    public playerData playerData = null;
    [Space]
    public List<playerData> playerDataList;
    #endregion

    void Awake()
    {
        playerRanking.TryGetComponent<UserDatePrefab>(out userDataSet);
        playerData = new playerData();
        levelset = new levelSet();
        playerDataList = new List<playerData>();

        #region Button Event Setting
        // freeze Button
        FirstSet(freezeLevelShowRanking);
        freezeLevelButs[0].onClick.AddListener(delegate { ContentSetFalse(freezeLevelShowRanking[0]); });
        freezeLevelButs[1].onClick.AddListener(delegate { ContentSetFalse(freezeLevelShowRanking[1]); });
        freezeLevelButs[2].onClick.AddListener(delegate { ContentSetFalse(freezeLevelShowRanking[2]); });
        freezeLevelButs[3].onClick.AddListener(delegate { ContentSetFalse(freezeLevelShowRanking[3]); });

        // sketchUP Button
        FirstSet(sketchUPLevelShowRanking);
        sketchUPLevelButtons[0].onClick.AddListener(delegate { ContentSetFalse(sketchUPLevelShowRanking[0]); });
        sketchUPLevelButtons[1].onClick.AddListener(delegate { ContentSetFalse(sketchUPLevelShowRanking[1]); });
        sketchUPLevelButtons[2].onClick.AddListener(delegate { ContentSetFalse(sketchUPLevelShowRanking[2]); });
        sketchUPLevelButtons[3].onClick.AddListener(delegate { ContentSetFalse(sketchUPLevelShowRanking[3]); });
        #endregion
    }

    private void OnDisable()
    {
        if (sketchUPScrollRect.gameObject.active == false)
            Debug.Log("불림");
    }

    void FirstSet(RectTransform[] transformObj)
    {
        transformObj[0].gameObject.SetActive(true);
        transformObj[1].gameObject.SetActive(false);
        transformObj[2].gameObject.SetActive(false);
        transformObj[3].gameObject.SetActive(false);
    }

    levelSet LevelSetActive(levelSet levelSet)
    {
        levelSet.easyPos.gameObject.SetActive(levelSet.easy);
        levelSet.normalpos.gameObject.SetActive(levelSet.normal);
        levelSet.hardpos.gameObject.SetActive(levelSet.hard);
        levelSet.extremepos.gameObject.SetActive(levelSet.extreme);

        return levelSet;
    }

    // 프로펩 생성
    public void InstUserData(int rank, string id, long point, RectTransform pos)
    {
        userDataPrefab.TryGetComponent<UserDatePrefab>(out userDataSet);
        userDataSet.ShowRankSet(rank);
        userDataSet.ShowIDSet(id);
        userDataSet.ShowPointSet(point);

        instData = ObjectPoolCP.PoolCp.Inst.BringObjectCp(userDataPrefab);
        instData.transform.SetParent(pos, false);
    }

    public List<playerData> PlayerDataAdd(int level, int rank, string id, long point, RectTransform pos)
    {
        playerData newData = new playerData();

        newData.level = level;
        newData.rank = rank;
        newData.id = id;
        newData.point = point;

        playerDataList.Add(newData);
        ContentSetFalse(pos);
        return playerDataList;
    }

    public void ContentSetFalse(RectTransform pos)
    {
        if (pos == sketchUPLevelShowRanking[0])
        {
            ShowPlayerData(playerDataList, 1);
            PosCheck(true, false, false, false);
            sketchUPScrollRect.content = sketchUPLevelShowRanking[0];
        }
        else if (pos == sketchUPLevelShowRanking[1])
        {
            ShowPlayerData(playerDataList, 2);
            PosCheck(false, true, false, false);
            sketchUPScrollRect.content = sketchUPLevelShowRanking[1];
        }
        else if (pos == sketchUPLevelShowRanking[2])
        {
            PosCheck(false, false, true, false);
            sketchUPScrollRect.content = sketchUPLevelShowRanking[2];
            ShowPlayerData(playerDataList, 3);
        }
        else if (pos == sketchUPLevelShowRanking[3])
        {
            PosCheck(false, false, false, true);
            sketchUPScrollRect.content = sketchUPLevelShowRanking[3];
            ShowPlayerData(playerDataList, 4);
        }
    }

    void PosCheck(bool easy, bool normal, bool hard, bool extreme)
    {
        sketchUPLevelShowRanking[0].gameObject.SetActive(easy);
        sketchUPLevelShowRanking[1].gameObject.SetActive(normal);
        sketchUPLevelShowRanking[2].gameObject.SetActive(hard);
        sketchUPLevelShowRanking[3].gameObject.SetActive(extreme);
    }

    public void ShowPlayerData(List<playerData> playerDataList, int levelNum)
    {
        playerRanking.TryGetComponent<UserDatePrefab>(out userDataSet);
        playerData newData = playerDataList.Find(x => x.level == levelNum);

        userDataSet.ShowRankSet(newData.rank);
        userDataSet.ShowIDSet(newData.id);
        userDataSet.ShowPointSet(newData.point);
    }

    public void PlayerNoTopTenRank(string info)
    {
        playerRanking.TryGetComponent<UserDatePrefab>(out userDataSet);
        userDataSet.RankObjSet(false);
        userDataSet.ShowIDSet(info);
        userDataSet.PointObjSet(false);
    }
}