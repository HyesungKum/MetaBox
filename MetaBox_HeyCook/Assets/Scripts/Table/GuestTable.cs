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
    [SerializeField] GameObject guestObj;
    [SerializeField] SpriteRenderer guestImage;
    [SerializeField] SpriteRenderer talkBubble;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] TextMeshProUGUI guestText;

    [Header("Current Guest")]
    [SerializeField] Guest curGuest = null;

    [Header("[Require Food List]")]
    public List<FoodData> FoodList;
    private List<FoodData> TempTable;

    [Header("[Current Recipe]")]
    public FoodData requireFood;
    public List<IngredData> requireIngreds;

    [Header("[TablePostion]")]
    public Side side;

    //=======================================Component=========================================
    [SerializeField] SpriteRenderer spriteRenderer;

    //====================================inner variables======================================
    [SerializeField] int count;

    //=======================================caching===========================================
    [SerializeField] float delayNewGuest;
    private WaitForSeconds waitSec;
    private WaitUntil waitGetData;

    private void Awake()
    {
        //caching
        waitSec = new WaitForSeconds(delayNewGuest);
        waitGetData = new WaitUntil(() => FoodList != null);

        StartCoroutine(nameof(GettingData));
    }

    private void OnDestroy()
    {
        //delegate unchain
        if (side == Side.Right)
        {
            EventReciver.NewGuestR -= NewGuestPord;
            EventReciver.DoSubmissionR -= DoSubmission;
        }
        else
        {
            EventReciver.NewGuestL -= NewGuestPord;
            EventReciver.DoSubmissionL -= DoSubmission;
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
            EventReciver.NewGuestR += NewGuestPord;
            EventReciver.DoSubmissionR += DoSubmission;
        }
        else
        {
            EventReciver.NewGuestL += NewGuestPord;
            EventReciver.DoSubmissionL += DoSubmission;
        }

        if (side == Side.Right) EventReciver.CallNewGuestR();
        else EventReciver.CallNewGuestL();
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
        EventReciver.CallScoreModi(requireFood.Score);

        //call guest eat sfx
        SoundManager.Inst.CallSfx("GuestEat");

        //call new guest event
        if(side == Side.Right) EventReciver.CallNewGuestR();
        else EventReciver.CallNewGuestL();
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
        talkBubble.gameObject.SetActive(false);

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
                SoundManager.Inst.CallSfx("GuestLeave");
            }

            //Guest Move Finish -> point of Guest arrive
            if (timer > moveCurve.keys[^1].time)
            {
                //recipe pick
                PickRecipe();

                //talk bubble change
                talkBubble.gameObject.SetActive(true);
                talkBubble.sprite = curGuest.talkBubbleImage;
                
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
        spriteRenderer.sprite = requireFood.combineImage;

        //call new order
        if (side == Side.Right) EventReciver.CallNewOrderR();
        else EventReciver.CallNewOrderL();

        if (TempTable.Count == 0) TempTable = FoodList.ToArray().ToList();
    }
}
