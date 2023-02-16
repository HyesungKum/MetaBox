using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum Side
{
    Left,
    Right
}

public class GuestTable : MonoBehaviour
{
    //=====================================Reference Data======================================
    [Header("[Guest Data]")]
    public GuestGroup guestGroup;
    
    [Header("Current Guest")]
    [SerializeField] Guest curGuest = null;

    [Header("Guest Reference")]
    [SerializeField] GameObject guestObj;
    [SerializeField] SpriteRenderer guestImage;
    [SerializeField] GameObject     orderObj;
    [SerializeField] SpriteRenderer orderImage;
    [SerializeField] TextMeshProUGUI guestText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    [Header("Guest Move Production")]
    [SerializeField] AnimationCurve moveCurve;

    [Header("[Require Food List]")]
    public List<FoodData> FoodList;
    private List<FoodData> TempTable;

    [Header("[Current Recipe]")]
    public FoodData requireFood;

    [Header("[TablePostion]")]
    public Side side;
    //===================================inner variables=======================================
    private bool FirstToken { get; set; }

    //=======================================caching===========================================
    [SerializeField] float delayNewGuest;
    private WaitForSeconds waitSec;
    private WaitUntil waitGetData;

    private void Awake()
    {
        //caching
        waitSec = new WaitForSeconds(delayNewGuest);
        waitGetData = new WaitUntil(() => FoodList != null);

        //innver var init
        FirstToken = true;

        StartCoroutine(nameof(GettingData));
    }

    private void OnDestroy()
    {
        //delegate unchain
        if (side == Side.Right)
        {
            EventReceiver.ScoreModiR -= SocreModi;
            EventReceiver.NewGuestR -= NewGuestPord;
            EventReceiver.DoSubmissionR -= DoSubmission;
        }
        else
        {
            EventReceiver.ScoreModiL -= SocreModi;
            EventReceiver.NewGuestL -= NewGuestPord;
            EventReceiver.DoSubmissionL -= DoSubmission;
        }
    }

    //==================================Guest require recipe getting routine=============================
    IEnumerator GettingData()
    {
        yield return waitGetData;

        //Table Copy
        TempTable = FoodList.ToArray().ToList();

        //cur guest data init
        curGuest = null;

        //delegate chain
        if (side == Side.Right)
        {
            EventReceiver.ScoreModiR += SocreModi;
            EventReceiver.NewGuestR += NewGuestPord;
            EventReceiver.DoSubmissionR += DoSubmission;
        }
        else
        {
            EventReceiver.ScoreModiL += SocreModi;
            EventReceiver.NewGuestL += NewGuestPord;
            EventReceiver.DoSubmissionL += DoSubmission;
        }


        if (side == Side.Right) EventReceiver.CallNewGuestR();
        else EventReceiver.CallNewGuestL();
    }

    //============================================Submission=============================================
    void DoSubmission()
    {
        StartCoroutine(nameof(FoodReset));
    }
    IEnumerator FoodReset()
    {
        yield return waitSec;

        //score sum modify
        if (side == Side.Right) EventReceiver.CallScoreModiR(requireFood.Score);
        else EventReceiver.CallScoreModiL(requireFood.Score);

        //call guest eat sfx
        SoundManager.Inst.CallSfx("GuestEat");

        //call new guest event
        if(side == Side.Right) EventReceiver.CallNewGuestR();
        else EventReceiver.CallNewGuestL();
    }

    //====================================Customer Move Production=======================================
    void NewGuestPord()
    {
        StartCoroutine(nameof(GuestMove));
    }
    IEnumerator GuestMove()
    {
        Vector3 tempPos = guestObj.transform.position;
        float timer = 0f;
        bool token = true;
        orderObj.SetActive(false);

        //talk text out
        if (curGuest != null)
        {
            guestText.gameObject.SetActive(true);
            guestText.text = curGuest.textGroup.textData.texts[Random.Range(0, curGuest.textGroup.textData.texts.Length-1)];
        }

        while (true)
        {
            Vector3 fixedPos = tempPos + Vector3.left * moveCurve.Evaluate(timer);
            guestObj.transform.position = fixedPos;

            //Guest Move Out -> point of Guest Leave
            if (token && timer >= 0.5)
            {
                token = false;

                //Guest Data Change
                curGuest = guestGroup.Guests[Random.Range(0,4)];

                //Guest image change
                guestImage.sprite = curGuest.guestImage;

                //Guest Chang Sfx
                if(FirstToken) FirstToken = false;
                else SoundManager.Inst.CallSfx("GuestLeave");
            }

            //Guest Move Finish -> point of Guest arrive
            if (timer > moveCurve.keys[^1].time)
            {
                //recipe pick
                PickRecipe();

                //talk bubble change
                orderObj.SetActive(true);
                
                //Guest Text disable
                guestText.gameObject.SetActive(false);

                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    //========================================picking Controll===========================================
    void PickRecipe()
    {
        //pick index
        int index = Random.Range(0, TempTable.Count);

        //pick require food
        requireFood = TempTable[index];
        TempTable.RemoveAt(index);

        //show combine hint Image
        orderImage.sprite = requireFood.combineImage;

        //call new order
        if (side == Side.Right) EventReceiver.CallNewOrderR();
        else EventReceiver.CallNewOrderL();

        if (TempTable.Count == 0) TempTable = FoodList.ToArray().ToList();
    }

    //========================================Score Modifiy==============================================
    void SocreModi(int value)
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"+{value}";
    }
}
