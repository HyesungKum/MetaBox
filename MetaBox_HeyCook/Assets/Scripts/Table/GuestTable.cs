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
    [Header("Reference Data")]
    public GuestGroup guestGroup;
    public List<FoodData> FoodList;
    private List<FoodData> TempTable;

    [Header("Current Recipe")]
    public FoodData requireFood;
    public List<IngredData> requireIngreds;

    public Side side;

    //=====================================Reference Obj=======================================
    [Header("Guest")]
    [SerializeField] Guest curGuest = null;
    [SerializeField] GameObject guestObj;
    [SerializeField] SpriteRenderer guestImage;
    [SerializeField] SpriteRenderer talkBubble;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] TextMeshProUGUI guestText;

    [Header("Food Particle")]
    [SerializeField] public GameObject foodParticle;

    //=======================================Component=========================================
    [SerializeField] SpriteRenderer spriteRenderer;

    //====================================inner variables======================================
    [SerializeField] int count;

    //=======================================caching===========================================
    [SerializeField] float waitCallSec;
    private WaitForSeconds waitSec;
    private WaitUntil waitGetData;

    private void Awake()
    {
        //caching
        waitSec = new WaitForSeconds(waitCallSec);
        waitGetData = new WaitUntil(() => FoodList != null);

        StartCoroutine(nameof(GettingData));
    }

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
            EventReciver.NewCostomerR += NewCostomerPord;
            EventReciver.DoSubmissionR += DoSubmission;
        }
        else
        {
            EventReciver.NewCostomerL += NewCostomerPord;
            EventReciver.DoSubmissionL += DoSubmission;
        }

        if (side == Side.Right) EventReciver.CallNewComstomerR();
        else EventReciver.CallNewComstomerL();
    }

    private void OnDestroy()
    {
        //delegate unchain
        if (side == Side.Right)
        {
            EventReciver.NewCostomerR -= NewCostomerPord;
            EventReciver.DoSubmissionR -= DoSubmission;
        }
        else
        {
            EventReciver.NewCostomerL -= NewCostomerPord;
            EventReciver.DoSubmissionL -= DoSubmission;
        }
    }

    //=====================================Submission==========================================
    void DoSubmission()
    {
        StartCoroutine(nameof(FoodReset));
    }
    IEnumerator FoodReset()
    {
        yield return waitSec;

        PoolCp.Inst.DestoryObjectCp(foodParticle);

        EventReciver.CallScoreModi(requireFood.Score);

        if(side == Side.Right) EventReciver.CallNewComstomerR();
        else EventReciver.CallNewComstomerL();
    }

    //==============================Customer Move Production===================================
    void NewCostomerPord()
    {
        StartCoroutine(nameof(CustomerMove));
    }
    IEnumerator CustomerMove()
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

            //customer image change
            if (token && timer >= 0.5)
            {
                token = false;
                curGuest = guestGroup.Guests[Random.Range(0,4)];
                guestImage.sprite = curGuest.guestImage;
            }

            //recipe pick
            if (timer > moveCurve.keys[^1].time)
            {
                PickRecipe();
                talkBubble.gameObject.SetActive(true);
                talkBubble.sprite = curGuest.talkBubbleImage;
                guestText.gameObject.SetActive(false);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
    //=================================picking Controll=========================================
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
