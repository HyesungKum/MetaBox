using System;
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
        #region Button Event Setting
        levelone.onClick.AddListener(delegate { OnClickLevel(1); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(levelone.transform.position);
        });

        leveltwo.onClick.AddListener(delegate { OnClickLevel(2); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(leveltwo.transform.position);
        });

        levelthree.onClick.AddListener(delegate { OnClickLevel(3);  
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(levelthree.transform.position);
        });

        levelfour.onClick.AddListener(delegate { OnClickLevel(4); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(levelfour.transform.position);
        });

        // === close button set ===
        closeBut.onClick.AddListener(delegate { OnClickCloseBut(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(closeBut.transform.position);
        });
        #endregion
    }

    public int OnClickLevel(int index)
    {
        SoundManager.Inst.LevelIndex = index;
        StartSceneManager.Inst.MoveScene(SceneName.InGameScene);
        SoundManager.Inst.InGameBGMPlay();
        //Debug.Log("LevelIndex : " + SoundManager.Inst.LevelIndex);
        return SoundManager.Inst.LevelIndex;
    }

    public void OnClickCloseBut()
    {
        this.gameObject.SetActive(false);
        StartSceneManager.Inst.StartPanelSet(true);
    }
}