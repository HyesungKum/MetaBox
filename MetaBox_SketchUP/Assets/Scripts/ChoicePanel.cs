using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    [Header("[Button]")]
    [SerializeField] Button choiceOneBut = null;
    [SerializeField] Button choiceTwoBut = null;
    [SerializeField] Button choiceThreeBut = null;
    [SerializeField] Button choiceFourBut = null;

    [Header("[Button Text]")]
    TextMeshProUGUI butOneText = null;
    TextMeshProUGUI butTwoText = null;
    TextMeshProUGUI butThreeText = null;
    TextMeshProUGUI butFourText = null;

    //[Header("[Answer Korean]")]
    private string answers;

    [Header("[Audio Index]")]
    [SerializeField] private int audioIndex = 0;
    public int AudioIndex { get { return audioIndex; } set { audioIndex = value; } }

    public string Answers { get { return answers; } set { answers = value; } }

    RandomChoiceKeyWord choickKeyWord;

    void Awake()
    {
        TryGetComponent<RandomChoiceKeyWord>(out choickKeyWord);
        Answers = choickKeyWord.KoreanAnswerKeyWord;
    }

    void Start()
    {
        choiceOneBut.onClick.AddListener(delegate { CheckString(choiceOneBut, butOneText,AudioIndex ); });
        choiceTwoBut.onClick.AddListener(delegate { CheckString(choiceTwoBut, butTwoText, AudioIndex); });
        choiceThreeBut.onClick.AddListener(delegate { CheckString(choiceThreeBut, butThreeText, AudioIndex); });
        choiceFourBut.onClick.AddListener(delegate { CheckString(choiceFourBut, butFourText, AudioIndex); });
    }

    void CheckString(Button butNum, TextMeshProUGUI butText , int index)
    {
        butNum.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out butText);
        Debug.Log(" ## butText : " + butText.text);

        if (butText.text == Answers)
        {
            Debug.Log("정답을 맞췄습니다 !!");
            SoundManager.Inst.AnimalAudioPlay(index);
            this.gameObject.SetActive(false);
        }
    }
}