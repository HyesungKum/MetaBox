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

    private int levelIndex;
    public int LevelIndex { get { return levelIndex; } set { levelIndex = value; } }

    void Awake()
    {
        levelone.onClick.AddListener(delegate { OnClickLevel(1); SoundManager.Inst.ButtonSFXPlay(); });
        leveltwo.onClick.AddListener(delegate { OnClickLevel(2); SoundManager.Inst.ButtonSFXPlay(); });
        levelthree.onClick.AddListener(delegate { OnClickLevel(3);  SoundManager.Inst.ButtonSFXPlay(); });
        levelfour.onClick.AddListener(delegate { OnClickLevel(4); SoundManager.Inst.ButtonSFXPlay(); });

        // === close button set ===
        closeBut.onClick.AddListener(delegate { OnClickCloseBut(); SoundManager.Inst.ButtonSFXPlay(); });
    }

    public int OnClickLevel(int Index)
    {
        LevelIndex = Index;
        //Debug.Log("LevelIndex : " + LevelIndex);
        StartSceneManager.Inst.MoveScene(SceneName.InGameScene);
        SoundManager.Inst.InGameBGMPlay();
        return LevelIndex;
    }

    public void OnClickCloseBut()
    {
        this.gameObject.SetActive(false);
        StartSceneManager.Inst.StartPanelSet(true);
    }
}