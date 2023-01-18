using UnityEngine;

public class PanelSettingMgr : MonoBehaviour
{
    #region Singleton
    private static PanelSettingMgr instance;
    public static PanelSettingMgr Inst
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PanelSettingMgr>();
                if(instance == null)
                {
                    instance = new GameObject(nameof(PanelSettingMgr), typeof(PanelSettingMgr)).GetComponent<PanelSettingMgr>();
                }
            }
            return instance;
        }
    }
    #endregion

    [SerializeField] GameObject startPanel = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] GameObject levelPanel = null;
    [SerializeField] GameObject inGamePanel = null;

    void Awake()
    {
        FirstPanenSetting();
    }
    
    void FirstPanenSetting()
    {
        StartPanelSet(true);
        OptionPanelSet(false);
        LevelPanelSet(false);
        InGamePanelSet(false);
    }

    public void StartPanelSet(bool active)
    {
        startPanel.gameObject.SetActive(active);
    }
    
    public void OptionPanelSet(bool active)
    {
        optionPanel.gameObject.SetActive(active);
    }

    public void LevelPanelSet(bool active)
    {
        levelPanel.gameObject.SetActive(active);
    }

    public void InGamePanelSet(bool active)
    {
        inGamePanel.gameObject.SetActive(active);
    }
}
