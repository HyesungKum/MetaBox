using UnityEngine;
using UnityEngine.UI;
using ToolKum.AppTransition;


public class QuitPanelSet : MonoBehaviour
{
    [Header("[Quit Panel Button]")]
    [SerializeField] Button quitPanelOkBut = null;
    [SerializeField] Button quitPanelQuitBut = null;


    void Awake()
    {
        quitPanelOkBut.onClick.AddListener(delegate { AppQuit(); 
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(quitPanelOkBut.transform.position);
        });

        quitPanelQuitBut.onClick.AddListener(delegate { OnClickQuitBut();  
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(quitPanelQuitBut.transform.position);
        });
    }

    void OnClickQuitBut()
    {
        StartSceneManager.Inst.StartPanelSet(true);
        this.gameObject.SetActive(false);
    }

    private void AppQuit()
    {
        if (LoadIDMgr.Inst.curUserData.id == "전설의연습생") Application.Quit(); 
        else AppTrans.MoveScene(LoadIDMgr.Inst.mainPackName);
    }
}