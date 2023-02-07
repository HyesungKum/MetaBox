using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelSet : MonoBehaviour
{
    [Header("[Select Panel]")]
    [SerializeField] Button oneBrush = null;
    [SerializeField] Button twoBrush = null;
    [SerializeField] Button threeBrush = null;
    [SerializeField] Button CloseSelectPanelBut = null;

    [Header("[InGame obj Set]")]
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;


    void Awake()
    {
        oneBrush.onClick.AddListener(delegate { OnClickOneBrush(); SoundManager.Inst.ButtonSFXPlay(); });
        twoBrush.onClick.AddListener(delegate { OnClickTwoBrush(); SoundManager.Inst.ButtonSFXPlay(); });
        threeBrush.onClick.AddListener(delegate { OnClickThreeBrush(); SoundManager.Inst.ButtonSFXPlay(); });

        CloseSelectPanelBut.onClick.AddListener(delegate { OnClickInGameGoSelectPanel(); SoundManager.Inst.ButtonSFXPlay(); });
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
        InGamePanelSet.Inst.OneBrushPlayPanelSet(true);
        button.gameObject.SetActive(butSet);
    }

    public void PlayGameObjSet(bool objOneSet, bool objTwoSet, bool objThreeSet)
    {
        InGamePanelSet.Inst.InGameSet(true);
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
        InGamePanelSet.Inst.SelectPanelSet(true);
        InGamePanelSet.Inst.OneBrushPlayPanelSet(false);
        InGamePanelSet.Inst.InGameSet(false);
        ButAllSet();
    }
}