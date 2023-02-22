using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RankingDB;
using MongoDB.Bson;
using UnityEngine.UI;

public class TopTenRankMgr : MonoBehaviour
{
    [Header("[User Data Prefab]")]
    [SerializeField] GameObject userDataPrefab = null;

    [Header("[Player Ranking]")]
    [SerializeField] GameObject playerRanking = null;

    [Header("[SketchUP Inst Pos]")]
    [SerializeField] public RectTransform instEasyPos = null;
    [SerializeField] public RectTransform instNormalPos = null;
    [SerializeField] public RectTransform instHardPos = null;
    [SerializeField] public RectTransform instExtremePos = null;

    [Header("[Level Button]")]
    [SerializeField] Button levelOneBut = null;
    [SerializeField] Button levelTwoBut = null;
    [SerializeField] Button levelThreeBut = null;
    [SerializeField] Button levelFourBut = null;

    UserDatePrefab userDataSet = null;
    GameObject instData = null;

    void Awake()
    {
        playerRanking.TryGetComponent<UserDatePrefab>(out userDataSet);
        FirstSet();
        levelOneBut.onClick.AddListener(delegate { ContentSetFalse(instEasyPos); });
        levelTwoBut.onClick.AddListener(delegate { ContentSetFalse(instNormalPos); });
        levelThreeBut.onClick.AddListener(delegate { ContentSetFalse(instHardPos); });
        levelFourBut.onClick.AddListener(delegate { ContentSetFalse(instExtremePos); });
    }

    void FirstSet()
    {
        instEasyPos.gameObject.SetActive(true);
        instNormalPos.gameObject.SetActive(false);
        instHardPos.gameObject.SetActive(false);
        instExtremePos.gameObject.SetActive(false);
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

    public void ContentSetFalse(RectTransform pos)
    {
        if (pos == instEasyPos)
        {
            PosCheck(true, false, false, false);
        }
        else if(pos == instNormalPos)
        {
            PosCheck(false, true, false, false);
        }
        else if(pos == instHardPos)
        {
            PosCheck(false, false, true, false);
        }
        else if(pos == instExtremePos)
        {
            PosCheck(false, false, false, true);
        }
    }

    void PosCheck(bool easy, bool normal, bool hard, bool extreme)
    {
        instEasyPos.gameObject.SetActive(easy);
        instNormalPos.gameObject.SetActive(normal);
        instHardPos.gameObject.SetActive(hard);
        instExtremePos.gameObject.SetActive(extreme);
    }

    public void ShowPlayerData(int rank, string id, long point)
    {
        playerRanking.TryGetComponent<UserDatePrefab>(out userDataSet);
        userDataSet.ShowRankSet(rank);
        userDataSet.ShowIDSet(id);
        userDataSet.ShowPointSet(point);
    }

    public void PlayerNoTopTenRank(string info)
    {
        playerRanking.TryGetComponent<UserDatePrefab>(out userDataSet);
        //userDataSet.ShowNoTonTenInfo();
        userDataSet.RankObjSet(false);
        userDataSet.ShowIDSet(info);
        userDataSet.PointObjSet(false);
    }
}