using UnityEngine;

public class PanelSettingMgr : MonoBehaviour
{
    [SerializeField] GameObject startPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject levelPanel = null;
    [SerializeField] GameObject inGamePanel = null;

    void Awake()
    {
        PanenSetting(true, false, false, false, false);
    }

    void PanenSetting(bool startPanelset, bool optionPanelset, bool levelPanelset, bool selectPanelset, bool inGamePanelset)
    {
        startPanel.gameObject.SetActive(startPanelset);
        optionPanel.gameObject.SetActive(optionPanelset);
        levelPanel.gameObject.SetActive(levelPanelset);
        inGamePanel.gameObject.SetActive(inGamePanelset);
    }
}
