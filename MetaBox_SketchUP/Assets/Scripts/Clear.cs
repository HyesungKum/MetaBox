using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clear : MonoBehaviour
{
    [SerializeField] GameObject inGamePanel = null;
    [SerializeField] GameObject clearPanel = null;

    [SerializeField] GameObject clearImgOne = null;
    [SerializeField] GameObject clearImgTwo = null;
    [SerializeField] GameObject clearImgThree = null;


    void Start()
    {
        SetingClearImg(false, false, false);
    }

    public void ClearImgOne()
    {
        clearPanel.gameObject.SetActive(true);
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(true, false, false);
    }

    public void ClearIMgObjTwo()
    {
        clearPanel.gameObject.SetActive(true);
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(false, true, false);
    }

    public void ClearIMgObjThree() 
    {
        clearPanel.gameObject.SetActive(true);
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(false, false, true);
    }

    public void ClearImgTwo()
    {
        clearPanel.gameObject.SetActive(true);
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(true, true, false);
    }

    public void ClearImgThree()
    {
        clearPanel.gameObject.SetActive(true);
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(true, true, true);
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
