using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RankingDB;
using MongoDB.Bson;

public class TopTenRankMgr : MonoBehaviour
{
    [Header("[User Data Prefab]")]
    [SerializeField] GameObject userDataPrefab = null;


    [Header("[SketchUP Inst Pos]")]
    [SerializeField] RectTransform instPos = null;

    UserDatePrefab userDataSet = null;

    void Awake()
    {
        userDataPrefab.TryGetComponent<UserDatePrefab>(out userDataSet);
        InstUserData();
    }

    // 프로펩 생성
    void InstUserData()
    {
        GameObject instData = null;

        // 10 개를 생성하자
        for (int i = 1; i < 11; i++)
        {
            instData = ObjectPoolCP.PoolCp.Inst.BringObjectCp(userDataPrefab);
            instData.transform.SetParent(instPos, false); // 생성 포지션 지정

            userDataSet.ShowRankSet(i);
        }

    }
}
