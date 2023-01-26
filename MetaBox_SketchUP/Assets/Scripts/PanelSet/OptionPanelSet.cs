using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelSet : MonoBehaviour
{
    [SerializeField] Button backStartPanelBut = null;

    void Awake()
    {
        backStartPanelBut.onClick.AddListener(() => OnClickBakcButton());
    }

    void OnClickBakcButton()
    {
        this.gameObject.SetActive(false);
        PanelSettingMgr.Inst.StartPanelSet(true);
    }
}
