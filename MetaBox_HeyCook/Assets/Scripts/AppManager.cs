using UnityEngine;
using UnityEngine.UI;
using KumTool.AppTransition;

public class AppManager : MonoBehaviour
{
    [Header("Application Setting")]
    [SerializeField] string mainPackName = "com.MetaBox.MetaBox_Main";

    [SerializeField] Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(()=>AppTrans.MoveScene(mainPackName));

        //=================screen setting==================
        Screen.SetResolution(1920, 1080, true);

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }
}
