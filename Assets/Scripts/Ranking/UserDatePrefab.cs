using UnityEngine;
using TMPro;

public class UserDatePrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI showRank = null;
    [SerializeField] TextMeshProUGUI showId = null;
    [SerializeField] TextMeshProUGUI showPoint = null;

    public void ShowRankSet(int rank) => showRank.text = rank.ToString();
    
    public void ShowIDSet(string id) => showId.text = id;

    public void ShowPointSet(int point) => showPoint.text = point.ToString();
}
