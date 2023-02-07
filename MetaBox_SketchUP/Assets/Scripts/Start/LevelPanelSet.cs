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
        levelone.onClick.AddListener(delegate { OnClickLevelOneBut(); SoundManager.Inst.ButtonSFXPlay(); });
        leveltwo.onClick.AddListener(delegate { OnClickLevelTwoBut(); SoundManager.Inst.ButtonSFXPlay(); });
        levelthree.onClick.AddListener(delegate { OnClickLevelThreeBut();  SoundManager.Inst.ButtonSFXPlay(); });
        levelfour.onClick.AddListener(delegate { OnClickLevelFourBut(); SoundManager.Inst.ButtonSFXPlay(); });

        // === close button set ===
        closeBut.onClick.AddListener(delegate { OnClickCloseBut(); SoundManager.Inst.ButtonSFXPlay(); });
    }

    public void OnClickLevelOneBut()
    {
        SceneManager.LoadScene(SceneName.LevelOneScene);
        this.gameObject.SetActive(false);
    }

    public void OnClickLevelTwoBut()
    {
        SceneManager.LoadScene(SceneName.LevelTwoScene);
        this.gameObject.SetActive(false);
    }

    public void OnClickLevelThreeBut()
    {
        SceneManager.LoadScene(SceneName.LevelThreeScene);
        this.gameObject.SetActive(false);
    }

    public void OnClickLevelFourBut()
    {
        SceneManager.LoadScene(SceneName.LevelFourScene);
        this.gameObject.SetActive(false);
    }

    public void OnClickCloseBut()
    {
        this.gameObject.SetActive(false);
        StartSceneManager.Inst.StartPanelSet(true);
    }
}
