using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RankingDB;
using MongoDB.Bson;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

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
    [SerializeField] GameObject freezePlayerRank = null;
    [SerializeField] GameObject heyCookPlayerRank = null;
    [SerializeField] GameObject melodiaPlayerRank = null;
    [SerializeField] GameObject sketchUpPlayerRank = null;

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
    [SerializeField] public ScrollRect melodiaSongOneScrollRect = null;
    [SerializeField] public ScrollRect melodiaSongTwoScrollRect = null;
    [SerializeField] public ScrollRect melodiaSongThreeScrollRect = null;
    [SerializeField] public ScrollRect melodiaSongFourScrollRect = null;
    [Space]
    [SerializeField] public RectTransform[] melodiaSongOneRanking = null;
    [SerializeField] public RectTransform[] melodiaSongTwoRanking = null;
    [SerializeField] public RectTransform[] melodiaSongThreeRanking = null;
    [SerializeField] public RectTransform[] melodiaSongFourRanking = null;
    [Space]
    [SerializeField] Button[] melodiaSongNumButton = null;
    [SerializeField] Button[] melodiaSongOneLevelButtons = null;
    [SerializeField] Button[] melodiaSongTwoLevelButtons = null;
    [SerializeField] Button[] melodiaSongThreeLevelButtons = null;
    [SerializeField] Button[] melodiaSongFourLevelButtons = null;
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
    #endregion

    #region List
    [Space]
    public List<playerData> playerDataFreezeList;
    public List<playerData> playerDataHeyCookList;
    public List<playerData> playerMelodiaSongOneDataList;
    public List<playerData> playerMelodiaSongTwoDataList;
    public List<playerData> playerMelodiaSongThreeDataList;
    public List<playerData> playerMelodiaSongFourDataList;
    public List<playerData> playerDataSketchUPList;
    #endregion

    void Awake()
    {
        sketchUpPlayerRank.TryGetComponent<UserDatePrefab>(out userDataSet);
        playerData = new playerData();
        levelset = new levelSet();


        #region List ReSet
        playerDataFreezeList = new List<playerData>();
        playerDataHeyCookList = new List<playerData>();
        playerMelodiaSongOneDataList = new List<playerData>();
        playerMelodiaSongTwoDataList = new List<playerData>();
        playerMelodiaSongThreeDataList = new List<playerData>();
        playerMelodiaSongFourDataList = new List<playerData>();
        playerDataSketchUPList = new List<playerData>();
        #endregion

        #region Button Event Setting
        // freeze Button
        FirstSet(freezeLevelShowRanking);
        freezeLevelButs[0].onClick.AddListener(delegate { CheckTruePos(freezeScrollRect, freezeLevelShowRanking, freezePlayerRank, playerDataFreezeList, 0); });
        freezeLevelButs[1].onClick.AddListener(delegate { CheckTruePos(freezeScrollRect, freezeLevelShowRanking, freezePlayerRank, playerDataFreezeList, 1); });
        freezeLevelButs[2].onClick.AddListener(delegate { CheckTruePos(freezeScrollRect, freezeLevelShowRanking, freezePlayerRank, playerDataFreezeList, 2); });
        freezeLevelButs[3].onClick.AddListener(delegate { CheckTruePos(freezeScrollRect, freezeLevelShowRanking, freezePlayerRank, playerDataFreezeList, 3); });
        #endregion

        #region heyCook Button
        heyCookLevelButton[0].onClick.AddListener(delegate { CheckTruePos(heyCookScrollRect, heyCookLevelShowPanking, heyCookPlayerRank, playerDataHeyCookList, 0); });
        heyCookLevelButton[1].onClick.AddListener(delegate { CheckTruePos(heyCookScrollRect, heyCookLevelShowPanking, heyCookPlayerRank, playerDataHeyCookList, 1); });
        heyCookLevelButton[2].onClick.AddListener(delegate { CheckTruePos(heyCookScrollRect, heyCookLevelShowPanking, heyCookPlayerRank, playerDataHeyCookList, 2); });
        heyCookLevelButton[3].onClick.AddListener(delegate { CheckTruePos(heyCookScrollRect, heyCookLevelShowPanking, heyCookPlayerRank, playerDataHeyCookList, 3); });
        #endregion

        #region MelodiaData Button
        MelodiaFirstSet();
        melodiaSongNumButton[0].onClick.AddListener(delegate { OnClickMoelidiaSongButton(true, false, false, false); MelidiaContentFirstSet(melodiaSongOneRanking); });
        melodiaSongNumButton[1].onClick.AddListener(delegate { OnClickMoelidiaSongButton(false, true, false, false); MelidiaContentFirstSet(melodiaSongTwoRanking); });
        melodiaSongNumButton[2].onClick.AddListener(delegate { OnClickMoelidiaSongButton(false, false, true, false); MelidiaContentFirstSet(melodiaSongThreeRanking); });
        melodiaSongNumButton[3].onClick.AddListener(delegate { OnClickMoelidiaSongButton(false, false, false, true); MelidiaContentFirstSet(melodiaSongFourRanking); });

        melodiaSongOneLevelButtons[0].onClick.AddListener(delegate { CheckTruePos(melodiaSongOneScrollRect, melodiaSongOneRanking, melodiaPlayerRank, playerMelodiaSongOneDataList, 0);});
        melodiaSongOneLevelButtons[1].onClick.AddListener(delegate { CheckTruePos(melodiaSongOneScrollRect, melodiaSongOneRanking, melodiaPlayerRank, playerMelodiaSongOneDataList, 1); });
        melodiaSongOneLevelButtons[2].onClick.AddListener(delegate { CheckTruePos(melodiaSongOneScrollRect, melodiaSongOneRanking, melodiaPlayerRank, playerMelodiaSongOneDataList, 2); });
        melodiaSongOneLevelButtons[3].onClick.AddListener(delegate { CheckTruePos(melodiaSongOneScrollRect, melodiaSongOneRanking, melodiaPlayerRank, playerMelodiaSongOneDataList, 3); });

        melodiaSongTwoLevelButtons[0].onClick.AddListener(delegate { CheckTruePos(melodiaSongTwoScrollRect, melodiaSongTwoRanking, melodiaPlayerRank, playerMelodiaSongTwoDataList, 0); });
        melodiaSongTwoLevelButtons[1].onClick.AddListener(delegate { CheckTruePos(melodiaSongTwoScrollRect, melodiaSongTwoRanking, melodiaPlayerRank, playerMelodiaSongTwoDataList, 1); });
        melodiaSongTwoLevelButtons[2].onClick.AddListener(delegate { CheckTruePos(melodiaSongTwoScrollRect, melodiaSongTwoRanking, melodiaPlayerRank, playerMelodiaSongTwoDataList, 2); });
        melodiaSongTwoLevelButtons[3].onClick.AddListener(delegate { CheckTruePos(melodiaSongTwoScrollRect, melodiaSongTwoRanking, melodiaPlayerRank, playerMelodiaSongTwoDataList, 3); });
         
        melodiaSongThreeLevelButtons[0].onClick.AddListener(delegate { CheckTruePos(melodiaSongThreeScrollRect, melodiaSongThreeRanking, melodiaPlayerRank, playerMelodiaSongThreeDataList, 0); });
        melodiaSongThreeLevelButtons[1].onClick.AddListener(delegate { CheckTruePos(melodiaSongThreeScrollRect, melodiaSongThreeRanking, melodiaPlayerRank, playerMelodiaSongThreeDataList, 1); });
        melodiaSongThreeLevelButtons[2].onClick.AddListener(delegate { CheckTruePos(melodiaSongThreeScrollRect, melodiaSongThreeRanking, melodiaPlayerRank, playerMelodiaSongThreeDataList, 2); });
        melodiaSongThreeLevelButtons[3].onClick.AddListener(delegate { CheckTruePos(melodiaSongThreeScrollRect, melodiaSongThreeRanking, melodiaPlayerRank, playerMelodiaSongThreeDataList, 3); });

        melodiaSongFourLevelButtons[0].onClick.AddListener(delegate { CheckTruePos(melodiaSongFourScrollRect, melodiaSongFourRanking, melodiaPlayerRank, playerMelodiaSongFourDataList, 0); });
        melodiaSongFourLevelButtons[1].onClick.AddListener(delegate { CheckTruePos(melodiaSongFourScrollRect, melodiaSongFourRanking, melodiaPlayerRank, playerMelodiaSongFourDataList, 1); });
        melodiaSongFourLevelButtons[2].onClick.AddListener(delegate { CheckTruePos(melodiaSongFourScrollRect, melodiaSongFourRanking, melodiaPlayerRank, playerMelodiaSongFourDataList, 2); });
        melodiaSongFourLevelButtons[3].onClick.AddListener(delegate { CheckTruePos(melodiaSongFourScrollRect, melodiaSongFourRanking, melodiaPlayerRank, playerMelodiaSongFourDataList, 3); });
        #endregion

        #region sketchUP Button
        FirstSet(sketchUPLevelShowRanking);
        sketchUPLevelButtons[0].onClick.AddListener(delegate { CheckTruePos(sketchUPScrollRect, sketchUPLevelShowRanking, sketchUpPlayerRank, playerDataSketchUPList, 0); });
        sketchUPLevelButtons[1].onClick.AddListener(delegate { CheckTruePos(sketchUPScrollRect, sketchUPLevelShowRanking, sketchUpPlayerRank, playerDataSketchUPList, 1); });
        sketchUPLevelButtons[2].onClick.AddListener(delegate { CheckTruePos(sketchUPScrollRect, sketchUPLevelShowRanking, sketchUpPlayerRank, playerDataSketchUPList, 2); });
        sketchUPLevelButtons[3].onClick.AddListener(delegate { CheckTruePos(sketchUPScrollRect, sketchUPLevelShowRanking, sketchUpPlayerRank, playerDataSketchUPList, 3); });
        #endregion
    }


    void FirstSet(RectTransform[] transformObj)
    {
        transformObj[0].gameObject.SetActive(true);

        for (int i = 1; i < 4; ++i)
        {
            transformObj[i].gameObject.SetActive(false);
        }
    }

    void MelodiaFirstSet()
    {
        OnClickMoelidiaSongButton(true, false, false, false);
        MelidiaContentFirstSet(melodiaSongOneRanking);
    }

    void OnClickMoelidiaSongButton(bool songOne, bool songTwo, bool songThree, bool songFour)
    {
        melodiaSongOneScrollRect.gameObject.SetActive(songOne);
        melodiaSongTwoScrollRect.gameObject.SetActive(songTwo);
        melodiaSongThreeScrollRect.gameObject.SetActive(songThree);
        melodiaSongFourScrollRect.gameObject.SetActive(songFour);
    }

    void MelidiaContentFirstSet(RectTransform[] songRank)
    {
        songRank[0].gameObject.SetActive(true);
        for(int i = 1; i <4; ++i)
        {
            songRank[i].gameObject.SetActive(false);
        }
    }

    // ÇÁ·ÎÆé »ý¼º
    public void InstUserData(int rank, string id, long point, RectTransform pos)
    {
        userDataPrefab.TryGetComponent<UserDatePrefab>(out userDataSet);
        userDataSet.ShowRankSet(rank);
        userDataSet.ShowIDSet(id);
        userDataSet.ShowPointSet(point);

        instData = ObjectPoolCP.PoolCp.Inst.BringObjectCp(userDataPrefab);
        instData.transform.SetParent(pos, false);
    }

    public List<playerData> PlayerDataAdd(List<playerData> list, int level, int rank, string id, long point, RectTransform pos)
    {
        playerData newData = new playerData();

        newData.level = level;
        newData.rank = rank;
        newData.id = id;
        newData.point = point;

        list.Add(newData);
        return list;
    }

    void CheckTruePos(ScrollRect scroll, RectTransform[] pos, GameObject playerData, List<playerData> list, int index)
    {
        if (index == 0)
        {
            pos[index].gameObject.SetActive(true);
            pos[1].gameObject.SetActive(false);
            pos[2].gameObject.SetActive(false);
            pos[3].gameObject.SetActive(false);
            scroll.content = pos[index];
            ShowPlayerData(playerData, list, 1);
        }
        else if (index == 1)
        {
            pos[0].gameObject.SetActive(false);
            pos[index].gameObject.SetActive(true);
            pos[2].gameObject.SetActive(false);
            pos[3].gameObject.SetActive(false);
            scroll.content = pos[index];
            ShowPlayerData(playerData, list, 2);
        }
        else if (index == 2)
        {
            pos[0].gameObject.SetActive(false);
            pos[1].gameObject.SetActive(false);
            pos[index].gameObject.SetActive(true);
            pos[3].gameObject.SetActive(false);
            scroll.content = pos[index];
            ShowPlayerData(playerData, list, 3);
        }
        else if (index == 3)
        {
            pos[0].gameObject.SetActive(false);
            pos[1].gameObject.SetActive(false);
            pos[2].gameObject.SetActive(false);
            pos[index].gameObject.SetActive(true);
            scroll.content = pos[index];
            ShowPlayerData(playerData, list, 4);
        }
    }

    public void ShowPlayerData(GameObject playerData, List<playerData> playerDataList, int levelNum)
    {
        playerData.TryGetComponent<UserDatePrefab>(out userDataSet);
        playerData newData = playerDataList.Find(x => x.level == levelNum);

        userDataSet.ShowRankSet(newData.rank);
        userDataSet.ShowIDSet(newData.id);
        userDataSet.ShowPointSet(newData.point);
    }

    public void PlayerNoTopTenRank(string info)
    {
        sketchUpPlayerRank.TryGetComponent<UserDatePrefab>(out userDataSet);
        userDataSet.RankObjSet(false);
        userDataSet.ShowIDSet(info);
        userDataSet.PointObjSet(false);
    }
}