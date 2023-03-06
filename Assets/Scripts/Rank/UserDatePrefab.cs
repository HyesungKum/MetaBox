using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserDatePrefab : MonoBehaviour
{
    [Header("[Player Data]")]
    [SerializeField] TextMeshProUGUI showRank = null;
    [SerializeField] TextMeshProUGUI showId = null;
    [SerializeField] TextMeshProUGUI showPoint = null;

    [Header("[Img Changed]")]
    [SerializeField] Image rankImg = null;
    [SerializeField] Sprite topOneImg = null;
    [SerializeField] Sprite topTwoImg = null;
    [SerializeField] Sprite topThreeImg = null;
    [SerializeField] Image medalImg = null;

    [Header("[Medal Sprite]")]
    [SerializeField] Sprite rankOneMedal = null;
    [SerializeField] Sprite rankTwoMedal = null;
    [SerializeField] Sprite rankThreeMedal = null;

    void Awake() => medalImg.transform.gameObject.SetActive(false);
    public void ShowRankSet(int rank) => showRank.text = rank.ToString();
    public void RankObjSet(bool active) => showRank.gameObject.SetActive(active);
    public void ShowIDSet(string id) => showId.text = id;
    public void IDObjSet(bool active) => showId.gameObject.SetActive(active);
    public void ShowPointSet(long point) => showPoint.text = point.ToString();
    public void PointObjSet(bool active) => showPoint.gameObject.SetActive(active);
    public void RankImgChangeTopOne() => rankImg.sprite = topOneImg;
    public void RankImgChangeTopTwo() => rankImg.sprite = topTwoImg;
    public void RankImgChangeTopThree() => rankImg.sprite = topThreeImg;
    public void MedalImgSetTure() => medalImg.gameObject.SetActive(true);
    public void RankOneSprite() => medalImg.sprite = rankOneMedal;
    public void RankTwoSprite() => medalImg.sprite = rankTwoMedal;
    public void RankThreeSprite() => medalImg.sprite = rankThreeMedal;
}