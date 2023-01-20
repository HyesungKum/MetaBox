using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] Button start = null;
    [SerializeField] Button option = null;
    [SerializeField] Button town = null;
    [SerializeField] Button exit = null;

    void Awake()
    {
        start.onClick.AddListener(OnClick_Start);
        option.onClick.AddListener(OnClick_Option);
        town.onClick.AddListener(OnClick_Town);
        exit.onClick.AddListener(OnClick_Exit);
    }

    void OnClick_Start()
    {
        this.gameObject.SetActive(false);
    }
    void OnClick_Option()
    {
        optionPanel.gameObject.SetActive(true);
    }
    void OnClick_Town()
    {

    }
    void OnClick_Exit()
    {
        Application.Quit();
    }
}
