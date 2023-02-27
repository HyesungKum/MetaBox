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
    [SerializeField] GameObject TitleReadImage;
    [SerializeField] Button touchScreenButton;

    [Header("[First Init UI]")]
    [SerializeField] GameObject SelectUIGroup;

    [SerializeField] ChangeObject changeCharacter;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [Space]
    [SerializeField] UserData curUserData;

    private void Awake()
    {
        //addlistener
        nextButton.onClick.AddListener(()=> changeCharacter.NextObj());
        prevButton.onClick.AddListener(()=> changeCharacter.PrevObj());

        touchScreenButton.onClick.AddListener(()=> ScreenTouched());

        //delegate chain
        EventReceiver.selectEvent += SelectUISetting;
        EventReceiver.loadingDone += LoadingDone;
    }

    void SelectUISetting() => ShowUI(SelectUIGroup);
    void LoadingDone() => ShowUI(TitleUIGroup);

    //============================UI Object Controll====================================
    void ShowUI(GameObject targetUIObj)
    {
        if (curUI != null) curUI.SetActive(false);
        curUI = targetUIObj;
        curUI.SetActive(true);
    }
    void ScreenTouched()
    {
        touchScreenButton.interactable = false;
        TitleReadImage.SetActive(false);
        EventReceiver.CallTouchScreen();
    }
}
