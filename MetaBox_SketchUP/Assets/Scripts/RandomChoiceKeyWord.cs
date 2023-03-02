using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomChoiceKeyWord : MonoBehaviour
{
    [Header("[Button in Text]")]
    [SerializeField] TextMeshProUGUI choiceOneButText = null;
    [SerializeField] TextMeshProUGUI choiceTwoButText = null;
    [SerializeField] TextMeshProUGUI choiceThreeButText = null;
    [SerializeField] TextMeshProUGUI choiceFourButText = null;

    [Header("[Answer String]")]
    [SerializeField] private string answerKeyWord;

    public ChoicePanelData AnimalDataContainer = null;
    public List<string> animalNameList = null;

    public string answer { get; set; }

    int answerIndex;

    int answerNum;
    int butNumTwo;
    int butNumThree;
    int butNumFour;

    void Awake()
    {
        if (AnimalDataContainer == null)
            AnimalDataContainer = Resources.Load<ChoicePanelData>("Data/AnimalNameData");

        #region List Add
        animalNameList = new List<string>();
        animalNameList.Add(AnimalDataContainer.Polarbear);
        animalNameList.Add(AnimalDataContainer.Reindeer);
        animalNameList.Add(AnimalDataContainer.Penguin);
        animalNameList.Add(AnimalDataContainer.Orca);
        animalNameList.Add(AnimalDataContainer.Walrus);
        animalNameList.Add(AnimalDataContainer.Dolphin);
        animalNameList.Add(AnimalDataContainer.Giraffe);
        animalNameList.Add(AnimalDataContainer.Elephant);
        animalNameList.Add(AnimalDataContainer.Cheetah);
        animalNameList.Add(AnimalDataContainer.Tiger);
        animalNameList.Add(AnimalDataContainer.Deer);
        animalNameList.Add(AnimalDataContainer.Rabbit);
        #endregion

        int listCount;
        listCount = animalNameList.Count;

        for (int i = 0; i < listCount; ++i)
        {
            if (animalNameList[i].Contains(answerKeyWord))
            {
                answer = animalNameList[i];
                answerIndex = i;
            }
        }

        SetButWord();
    }

    void SetButWord()
    {
        answerNum = Random.Range(0, 4);

        for (int i = 0; i < 4; ++i)
        {
            if (answerNum == 0)
            {
                butNumTwo = 1; butNumThree = 2; butNumFour = 3;
            }
            if (answerNum == 1)
            {
                butNumTwo = 0; butNumThree = 2; butNumFour = 3;
            }
            if (answerNum == 2)
            {
                butNumTwo = 0; butNumThree = 1; butNumFour = 3;
            }
            if (answerNum == 3)
            {
                butNumTwo = 0; butNumThree = 1; butNumFour = 2;
            }
        }

        animalNameList.RemoveAt(answerIndex); // 정답을 리스트에서 뺀다
        int listCount = animalNameList.Count;
        int randomOne = Random.Range(0, listCount);
        int randomTwo = Random.Range(0, listCount);
        int randomThree = Random.Range(0, listCount);

        //Debug.Log("## randomOne : " + randomOne);
        //Debug.Log("## randomTwo : " + randomTwo);
        //Debug.Log("## randomThree : " + randomThree);

        if(randomOne == randomTwo || randomOne == randomThree)
        {
            randomOne += 1;
        }
        else if(randomTwo == randomOne || randomTwo == randomThree)
        {
            randomTwo += 1;
        }
        else if(randomThree == randomOne || randomThree == randomTwo)
        {
            randomThree += 1;
        }

        //Debug.Log("## randomOne : " + randomOne);
        //Debug.Log("## randomTwo : " + randomTwo);
        //Debug.Log("## randomThree : " + randomThree);

        if (answerNum == 0)
        {
            choiceOneButText.text = answer;
            choiceTwoButText.text = animalNameList[randomOne];
            choiceThreeButText.text = animalNameList[randomTwo];
            choiceFourButText.text = animalNameList[randomThree];
        }
        else if (answerNum == 1)
        {
            choiceOneButText.text = animalNameList[randomOne];
            choiceTwoButText.text = answer;
            choiceThreeButText.text = animalNameList[randomTwo];
            choiceFourButText.text = animalNameList[randomThree];
        }
        else if (answerNum == 2)
        {
            choiceOneButText.text = animalNameList[randomTwo];
            choiceTwoButText.text = animalNameList[randomOne];
            choiceThreeButText.text = answer;
            choiceFourButText.text = animalNameList[randomThree];
        }
        else if (answerNum == 3)
        {
            choiceOneButText.text = animalNameList[randomOne];
            choiceTwoButText.text = animalNameList[randomTwo];
            choiceThreeButText.text = animalNameList[randomThree];
            choiceFourButText.text = answer;
        }
    }
}