using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelSet : MonoBehaviour
{
    [SerializeField] Button closeBut = null;

    void Awake()
    {
        closeBut.onClick.AddListener(delegate { OnClickCloseBut(); });
    }

    public void OnClickCloseBut()
    {
        this.gameObject.SetActive(false);
        PanelSettingMgr.Inst.StartPanelSet(true);
    }
}
