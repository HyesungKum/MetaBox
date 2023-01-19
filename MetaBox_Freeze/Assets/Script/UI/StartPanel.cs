using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] Button start = null;
    [SerializeField] Button option = null;
    [SerializeField] Button town = null;
    [SerializeField] Button exit = null;

    [SerializeField] GameObject levelPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        levelPanel.SetActive(false);
        start.onClick.AddListener(delegate { OnClick_Start(); });
        option.onClick.AddListener(delegate { OnClick_Option(); });
        town.onClick.AddListener(delegate { OnClick_Town(); });
        exit.onClick.AddListener(delegate { OnClick_Exit(); });
    }

    void OnClick_Start()
    {
        UserData.Instance.InGame();
        this.gameObject.SetActive(false);
        levelPanel.SetActive(true);
    }
    void OnClick_Option()
    {

    }
    void OnClick_Town()
    {

    }
    void OnClick_Exit()
    {
        Application.Quit();
    }
}
