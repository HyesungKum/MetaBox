using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCloseButton : MonoBehaviour
{
    [SerializeField] Button inGameCloseButton = null;

    [SerializeField] GameObject inGamePanel = null;
    [SerializeField] GameObject seleckPanel = null;
    [SerializeField] GameObject InGameTimer = null;

    void Start()
    {
        inGameCloseButton.onClick.AddListener(() => OnClickCloseButton());
    }

    void OnClickCloseButton()
    {
        inGamePanel.gameObject.SetActive(false);
        InGameTimer.gameObject.SetActive(false);
        seleckPanel.gameObject.SetActive(true);
    }
}
