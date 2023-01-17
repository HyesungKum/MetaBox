using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SelectPanelSet : MonoBehaviour
{
    [Header("[Select Panel]")]
    [SerializeField] Button OneBrush = null;
    [SerializeField] Button TwoBrush = null;
    [SerializeField] Button ThreeBrush = null;
    [SerializeField] TextMeshProUGUI stageName = null;

    [Header("[InGame obj Set]")]
    [SerializeField] GameObject oneBrushObj = null;
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;

    void Awake()
    {
        OneBrush.onClick.AddListener(() => OnClickOneBrush());
        TwoBrush.onClick.AddListener(() => OnClickTwoBrush());
        ThreeBrush.onClick.AddListener(() => OnClickThreeBrush());

    }

    void OnClickOneBrush()
    {
        PanelChangedSet(OneBrush, false);
        PlayGameObjSet(true, false, false);
    }

    void OnClickTwoBrush()
    {
        PanelChangedSet(TwoBrush, false);
        PlayGameObjSet(false, true, false);
    }

    void OnClickThreeBrush()
    {
        PanelChangedSet(ThreeBrush, false);
        PlayGameObjSet(false, false, true);
    }

    void PanelChangedSet(Button button, bool butSet)
    {
        this.gameObject.SetActive(false);
        oneBrushObj.gameObject.SetActive(true);
        oneBrushObj.gameObject.SetActive(true);
        button.gameObject.SetActive(butSet);
    }

    void PlayGameObjSet(bool objOneSet, bool objTwoSet, bool objThreeSet)
    {
        InGamePanelSet.Inst.InGameSet(true);
        stageName.gameObject.SetActive(false);
        objOne.gameObject.SetActive(objOneSet);
        objTwo.gameObject.SetActive(objTwoSet);
        objThree.gameObject.SetActive(objThreeSet);
    }
}
