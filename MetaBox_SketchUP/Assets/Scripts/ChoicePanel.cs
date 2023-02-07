using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button choiceOneBut = null;
    [SerializeField] Button choiceTwoBut = null;
    [SerializeField] Button choiceThreeBut = null;
    [SerializeField] Button choiceFourBut = null;

    public ChoicePanelData animalData = null;

    private string stageOneAnswer;
    private string stageTwoAnswer;
    private string stageThreeAnswer;

    #region Property
    public string StageOneAnswer { get { return stageOneAnswer; } set { stageOneAnswer = value; } }
    public string StageTwoAnswer { get { return stageTwoAnswer; } set { stageTwoAnswer = value; } }
    public string StageThreeAnswer { get { return stageThreeAnswer; } set { stageThreeAnswer = value; } }
    #endregion



    void Awake()
    { 
        choiceOneBut.onClick.AddListener(delegate { });
        choiceTwoBut.onClick.AddListener(delegate { });
        choiceThreeBut.onClick.AddListener(delegate { });
        choiceFourBut.onClick.AddListener(delegate { });
    }
}