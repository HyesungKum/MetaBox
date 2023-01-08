using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Clear : MonoBehaviour
{
    [SerializeField] GameObject inGamePanel = null;

    [SerializeField] GameObject clearImgOne = null;
    [SerializeField] GameObject clearImgTwo = null;
    [SerializeField] GameObject clearImgThree = null;

    

    void Start()
    {
        SetingClearImg(false, false, false);

    }

    public void ClearImgOne()
    {
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(true, false, false);
    }

    public void ClearImgTwo()
    {
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(true, true, false);
    }

    public void ClearImgThree()
    {
        inGamePanel.gameObject.SetActive(false);
        SetingClearImg(true, true, true);
    }


    void SetingClearImg(bool imgOne, bool imgTwo, bool imgThree)
    {
    
        clearImgOne.gameObject.SetActive(imgOne);
        clearImgTwo.gameObject.SetActive(imgTwo);
        clearImgThree.gameObject.SetActive(imgThree);
    }
}
