using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("[Player Data]")]
    [SerializeField] TextMeshProUGUI showRank = null;
    [SerializeField] TextMeshProUGUI showId = null;
    [SerializeField] TextMeshProUGUI showPoint = null;

    public void ShowRankSet(int rank) => showRank.text = rank.ToString();
    public void RankObjSet(bool active) => showRank.gameObject.SetActive(active);
    public void ShowIDSet(string id) => showId.text = id;
}