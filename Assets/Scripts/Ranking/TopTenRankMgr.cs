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

    // ������ ����
    void InstUserData()
    {
        GameObject instData = null;

        // 10 ���� ��������
        for (int i = 1; i < 11; i++)
        {
            instData = ObjectPoolCP.PoolCp.Inst.BringObjectCp(userDataPrefab);
            instData.transform.SetParent(instPos, false); // ���� ������ ����

            userDataSet.ShowRankSet(i);
        }

    }
}
