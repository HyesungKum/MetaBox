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

    [SerializeField] GameObject nextPanel = null;

    void Awake()
    {
        levelone.onClick.AddListener(() => OnClickLevelBut());
        //leveltwo.onClick.AddListener(() => );
        //levelthree.onClick.AddListener(() => );
        //levelfour.onClick.AddListener(() => );
    }

    public void OnClickLevelBut()
    {
        nextPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
