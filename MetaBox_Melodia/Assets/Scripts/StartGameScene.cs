using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myTestText;
    [SerializeField] GameObject myPanelOption;


    bool isOptionPanelOpen = false;

    private void Awake()
    {
        myPanelOption.SetActive(false);
    }


    public void OnClickReturnToTown()
    { 
        myTestText.text = "������ ������";
    }


    public void OnClickPlay()
    {
        SceneManager.LoadScene("MelodiaLobby");
    }

    public void OnClickRank()
    {
        myTestText.text = "��ŷ�� ������";
    }


    public void OnClickQuit()
    { 
        myTestText.text = "�׸��Ҳ���";
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
