using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        levelone.onClick.AddListener(delegate { OnClickLevel(SceneName.LevelOneScene); SoundManager.Inst.ButtonSFXPlay(); });
        leveltwo.onClick.AddListener(delegate { OnClickLevel(SceneName.LevelTwoScene); SoundManager.Inst.ButtonSFXPlay(); });
        levelthree.onClick.AddListener(delegate { OnClickLevel(SceneName.LevelThreeScene);  SoundManager.Inst.ButtonSFXPlay(); });
        levelfour.onClick.AddListener(delegate { OnClickLevel(SceneName.LevelFourScene); SoundManager.Inst.ButtonSFXPlay(); });

        // === close button set ===
        closeBut.onClick.AddListener(delegate { OnClickCloseBut(); SoundManager.Inst.ButtonSFXPlay(); });
    }

    void OnClickLevel(int sceneIndex)
    {
        StartSceneManager.Inst.MoveScene(sceneIndex);
        SoundManager.Inst.InGameBGMPlay();
    }

    public void OnClickCloseBut()
    {
        this.gameObject.SetActive(false);
        StartSceneManager.Inst.StartPanelSet(true);
    }
}