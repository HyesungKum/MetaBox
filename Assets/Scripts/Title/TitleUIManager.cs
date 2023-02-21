using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [Header("[Current Active UI]")]
    [SerializeField] GameObject curUI;

    [Header("[Title UI]")]
    [SerializeField] GameObject TitleUIGroup;
    [SerializeField] Button touchScreenButton;

    [Header("[First Init UI]")]
    [SerializeField] GameObject FirstInitUIGroup;

    [SerializeField] ChangeCharacter changeCharacter;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [Space]
    [SerializeField] UserData curUserData;

    private void Awake()
    {
        //addlistener
        nextButton.onClick.AddListener(()=> changeCharacter.NextObj());
        prevButton.onClick.AddListener(()=> changeCharacter.PrevObj());

        touchScreenButton.onClick.AddListener(()=> SceneClicked());

        //delegate chain
        EventReceiver.init += InitUISetting;
        EventReceiver.selectDone += SelectDone;
    }


    void InitUISetting()
    {
        ShowUI(FirstInitUIGroup);
    }

    void SelectDone()
    {
        ShowUI(TitleUIGroup);
    }

    //============================UI Object Controll====================================
    void ShowUI(GameObject targetUIObj)
    {
        if (curUI != null) curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }

    void SceneClicked()
    {
        touchScreenButton.interactable = false;
        EventReceiver.CallTouchScreen();
    }
}
