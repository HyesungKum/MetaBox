using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelSet : MonoBehaviour
{
    [Header("[Level Panel]")]
    [SerializeField] Button levelone = null;
    [SerializeField] Button leveltwo = null;
    [SerializeField] Button levelthree = null;
    [SerializeField] Button levelfour = null;
    [SerializeField] Button closeBut = null;

    void Awake()
    {
        levelone.onClick.AddListener(() => OnClickLevelBut());
        //leveltwo.onClick.AddListener(() => );
        //levelthree.onClick.AddListener(() => );
        //levelfour.onClick.AddListener(() => );

        // === close button set ===
        closeBut.onClick.AddListener(() => OnClickCloseBut());
    }

    public void OnClickLevelBut()
    {
        PanelSettingMgr.Inst.InGamePanelSet(true);
        this.gameObject.SetActive(false);
    }

    public void OnClickCloseBut()
    {
        this.gameObject.SetActive(false);
        PanelSettingMgr.Inst.StartPanelSet(true);
    }
}
