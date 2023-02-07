using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomChoiceKeyWord : MonoBehaviour
{
    [Header("[Button in Text]")]
    [SerializeField] TextMeshProUGUI choiceOneButText = null;
    [SerializeField] TextMeshProUGUI choiceTwoButText = null;
    [SerializeField] TextMeshProUGUI choiceThreeButText = null;
    [SerializeField] TextMeshProUGUI choiceFourButText = null;

    [Header("[AnimaData]")]
    [SerializeField] AnimalDatas animalData = null;

    [Header("[Answer String]")]
    [SerializeField] private string answerKeyWord;
    public string AnswerKeyWord { get { return answerKeyWord; } set { answerKeyWord = value; } }

    [Header("[Other Animal String]")]
    [SerializeField] private string animalOne = null;
    [SerializeField] private string animalTwo = null;
    [SerializeField] private string animalThree = null;

    public string AnimalOne { get { return animalOne; } set { animalOne = value; } }
    public string AnimalTwo { get { return animalTwo; } set { animalTwo = value; } }
    public string AnimalThree { get { return animalThree; } set { animalThree = value; } }

    void Start()
    {
        if (animalData.animalList.Contains(AnswerKeyWord) == true &&
           animalData.animalList.Contains(AnimalOne) == true &&
           animalData.animalList.Contains(AnimalTwo) == true &&
           animalData.animalList.Contains(AnimalThree) == true)
        {
            StagRandomKeyWord(AnswerKeyWord, AnimalOne, AnimalTwo, AnimalThree);
        }
        //Debug.Log("AnswerKeyWord : " + animalData.animalList.Contains(AnswerKeyWord));
        //Debug.Log("AnimalOne : " + animalData.animalList.Contains(AnimalOne));
        //Debug.Log("AnimalTwo : " + animalData.animalList.Contains(AnimalTwo));
        //Debug.Log("AnimalThree : " + animalData.animalList.Contains(AnimalThree));
    }

    void StagRandomKeyWord(string answer, string animalOne, string animalTwo, string animalThree)
    {
        #region
        int random = Random.Range(0, 4);
        int randomtwo = Random.Range(0, 4);
        int randomThree = Random.Range(0, 4);
        int randomFour = Random.Range(0, 4);

        //Debug.Log("$$ random : " + random);
        //Debug.Log("$$ randomtwo : " + randomtwo);
        //Debug.Log("$$ randomThree : " + randomThree);
        //Debug.Log("$$ RandomFour : " + randomFour);

        for (int i = 0; i < 4; i++)
        {
            if (random == randomtwo || random == randomThree || random == randomFour)
            {
                random -= 1;
                if (random == -1) random += 4;
            }
            else if (random == randomtwo && random != randomThree && random != randomFour)
            {
                random -= 2;
                if (random == -1) random += 3;
            }
            else if (randomtwo == random || randomtwo == randomThree || randomtwo == randomFour)
            {
                randomtwo -= 1;
                if (randomtwo == -1) randomtwo += 4;
            }
            else if (randomtwo == randomThree && randomtwo != random && randomtwo != randomFour)
            {
                randomtwo -= 2;
                if (randomtwo == -1) randomtwo += 3;
            }
            else if (randomThree == random || randomThree == randomtwo || randomThree == randomFour)
            {
                randomThree -= 1;
                if (randomThree == -1) randomThree += 4;
            }
            else if (randomFour == random || randomFour == randomtwo || randomFour == randomThree)
            {
                randomFour -= 1;
                if (randomFour == -1) randomFour += 4;
            }
        }

        //Debug.Log("## random : " + random);
        //Debug.Log("## randomtwo : " + randomtwo);
        //Debug.Log("## randomThree : " + randomThree);
        //Debug.Log("## RandomFour : " + randomFour);

        for (int i = 0; i < 4; i++)
        {
            if (random <= 0) choiceOneButText.text = answer;
            else if (random == 1) choiceOneButText.text = animalOne;
            else if (random == 2) choiceOneButText.text = animalTwo;
            else choiceOneButText.text = animalThree;
        }

        for (int i = 0; i < 4; i++)
        {
            if (randomtwo <= 0) choiceTwoButText.text = answer;
            else if (randomtwo == 1) choiceTwoButText.text = animalOne;
            else if (randomtwo == 2) choiceTwoButText.text = animalTwo;
            else choiceTwoButText.text = animalThree;
        }

        for (int i = 0; i < 4; i++)
        {
            if (randomThree <= 0) choiceThreeButText.text = answer;
            else if (randomThree == 1) choiceThreeButText.text = animalOne;
            else if (randomThree == 2) choiceThreeButText.text = animalTwo;
            else choiceThreeButText.text = animalThree;
        }

        for (int i = 0; i < 4; i++)
        {
            if (randomFour <= 0) choiceFourButText.text = answer;
            else if (randomFour == 1) choiceFourButText.text = animalOne;
            else if (randomFour == 2) choiceFourButText.text = animalTwo;
            else choiceFourButText.text = animalThree;
        }
        #endregion
    }
}
