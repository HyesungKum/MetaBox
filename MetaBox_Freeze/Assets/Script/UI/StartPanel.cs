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
        start.onClick.AddListener(() => this.gameObject.SetActive(false));
        option.onClick.AddListener(() => optionPanel.gameObject.SetActive(true));
        town.onClick.AddListener(() => ToolKum.AppTransition.AppTrans.MoveScene("com.MetaBox.MetaBox_Main"));
        exit.onClick.AddListener(() => Application.Quit());
    }
}
