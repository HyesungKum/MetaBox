using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Clear : MonoBehaviour
{
    [SerializeField] GameObject inGamePanel = null;
    [SerializeField] GameObject clearPanel = null;
    [SerializeField] GameObject clearTextPanel = null;
    [SerializeField] TextMeshProUGUI clearText = null;

    [SerializeField] GameObject clearImgOne = null;
    [SerializeField] GameObject clearImgTwo = null;
    [SerializeField] GameObject clearImgThree = null;

    void Awake()
    {
        clearPanel.gameObject.SetActive(false);
        SetingClearImg(false, false, false);
    }

    public void ClearImgOne()
    {
        PanelSetting(true, false);
        SetingClearImg(true, false, false);
    }

    public void ClearImgObjTwo()
    {
        PanelSetting(true, false);
        SetingClearImg(false, true, false);
    }

    public void ClearImgObjThree()
    {
        PanelSetting(true, false);
        SetingClearImg(false, false, true);
    }

    public void ClearImgTwo()
    {
        PanelSetting(true, false);
        SetingClearImg(true, true, false);
    }

    public void ClearAll()
    {
        PanelSetting(true, false);
        SetingClearImg(true, true, true);
        StartCoroutine(TimeDelayImgOne());
    }

    IEnumerator TimeDelayImgOne()
    {
        yield return new WaitForSeconds(2f);

        clearTextPanel.gameObject.SetActive(true);
        clearText.text = " Clear !! ";
    }

    void PanelSetting(bool clearPanelSet, bool inGamePanelSet)
    {
        clearPanel.gameObject.SetActive(clearPanelSet);
        inGamePanel.gameObject.SetActive(inGamePanelSet);
    }

    /// <summary>
    /// imgOne, imgTwo, imgThree True and False Setting
    /// </summary>
    /// <param name="imgOne"></param>
    /// <param name="imgTwo"></param>
    /// <param name="imgThree"></param>
    void SetingClearImg(bool imgOne, bool imgTwo, bool imgThree)
    {
        clearImgOne.gameObject.SetActive(imgOne);
        clearImgTwo.gameObject.SetActive(imgTwo);
        clearImgThree.gameObject.SetActive(imgThree);
    }
}
