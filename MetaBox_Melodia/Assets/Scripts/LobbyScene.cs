using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myTestText;
    [SerializeField] GameObject myPanelOption;

    bool isOptionPanelOpen = false;

    private void Awake()
    {
        myPanelOption.SetActive(false);
    }


    public void OnClickBack()
    {
        SceneManager.LoadScene("MelodiaStart");
    }

    public void OnClickEasy()
    {
       SceneManager.LoadScene("MelodiaEasy");
    }


    public void OnClickNormal()
    {
        myTestText.text = "보통 난이도";
    }



    public void OnClickDifficult()
    {
        myTestText.text = "난감한 난이도";
    }

    public void OnClickExtreme()
    {
        myTestText.text = "매우 난감한 난이도";
    }


    public void OnClickOption()
    {
        if (isOptionPanelOpen == false)
        {
            myPanelOption.SetActive(true);

            isOptionPanelOpen = true;
            return;
        }

        myPanelOption.SetActive(false);
        isOptionPanelOpen = false;
    }


}
