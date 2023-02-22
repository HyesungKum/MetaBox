using UnityEngine;
using TMPro;

public class UserDatePrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI showRank = null;
    [SerializeField] TextMeshProUGUI showId = null;
    [SerializeField] TextMeshProUGUI showPoint = null;

    public void ShowRankSet(int rank) => showRank.text = rank.ToString();
    public void RankObjSet(bool active) => showRank.gameObject.SetActive(active);
    public void ShowIDSet(string id) => showId.text = id;
    public void ShowNoTonTenInfo()
    {
        Vector2 change = new Vector2(85, 10f);
        showId.rectTransform.offsetMax = change;
        showId.transform.localPosition = new Vector3(0,0,0);
    }
    public void IDObjSet(bool active) => showId.gameObject.SetActive(active);
    public void ShowPointSet(long point) => showPoint.text = point.ToString();
    public void PointObjSet(bool active) => showPoint.gameObject.SetActive(active);
}
