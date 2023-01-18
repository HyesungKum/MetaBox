using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelSet : MonoBehaviour
{
    [Header("[Select Panel]")]
    [SerializeField] Button oneBrush = null;
    [SerializeField] Button twoBrush = null;
    [SerializeField] Button threeBrush = null;
    [SerializeField] Button inGameGoSelectPanel = null;

    [Header("[InGame obj Set]")]
    [SerializeField] GameObject oneBrushObj = null;
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;

    [SerializeField] TextMeshProUGUI stageName = null;

    void Awake()
    {
        oneBrush.onClick.AddListener(() => OnClickOneBrush());
        twoBrush.onClick.AddListener(() => OnClickTwoBrush());
        threeBrush.onClick.AddListener(() => OnClickThreeBrush());

        inGameGoSelectPanel.onClick.AddListener(() => OnClickInGameGoSelectPanel());

    }

    void OnClickOneBrush()
    {
        PanelChangedSet(oneBrush, false);
        PlayGameObjSet(true, false, false);
    }

    void OnClickTwoBrush()
    {
        PanelChangedSet(twoBrush, false);
        PlayGameObjSet(false, true, false);
    }

    void OnClickThreeBrush()
    {
        PanelChangedSet(threeBrush, false);
        PlayGameObjSet(false, false, true);
    }

    public void PanelChangedSet(Button button, bool butSet)
    {
        this.gameObject.SetActive(false);
        oneBrushObj.gameObject.SetActive(true);
        button.gameObject.SetActive(butSet);
    }

    public void PlayGameObjSet(bool objOneSet, bool objTwoSet, bool objThreeSet)
    {
        InGamePanelSet.Inst.InGameSet(true);
        InGamePanelSet.Inst.LineCloneTransform(true);
        // ===== 전에 선을 다시 보여주기
        stageName.gameObject.SetActive(false);
        objOne.gameObject.SetActive(objOneSet);
        objTwo.gameObject.SetActive(objTwoSet);
        objThree.gameObject.SetActive(objThreeSet);
    }

    void ButAllSet()
    {
        oneBrush.gameObject.SetActive(true);
        twoBrush.gameObject.SetActive(true);
        threeBrush.gameObject.SetActive(true);
    }

    void OnClickInGameGoSelectPanel()
    {
        InGamePanelSet.Inst.LineCloneTransform(false);
        InGamePanelSet.Inst.OneBrushPlayPanelSet(false);
        InGamePanelSet.Inst.SelectPanelSetting(true);

        ButAllSet();
    }
}