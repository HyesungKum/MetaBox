using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Submission : MonoBehaviour
{
    //=====================================Reference Data======================================
    [Header("Reference Data")]
    [SerializeField] public GuestGroup guestGroup;
    [SerializeField] List<FoodData> FoodList;
    [SerializeField] private List<FoodData> TempTable;

    [Header("Current Recipe")]
    [SerializeField] public FoodData requireFood;

    [SerializeField] public SetData SetDataR;
    [SerializeField] public SetData SetDataL;
    //[SerializeField] TalkData �������

    //=====================================Reference Obj=======================================
    [Header("Customer")]
    [SerializeField] Guest curGuest = null;
    [SerializeField] GameObject guestObj;
    [SerializeField] SpriteRenderer guestImage;//++
    [SerializeField] SpriteRenderer talkBubble;//++
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] TextMeshProUGUI guestText;

    [Header("SetMenu Particle")]
    [SerializeField] public GameObject particleR;
    [SerializeField] public GameObject particleL;

    //=======================================Component=========================================
    [SerializeField] SpriteRenderer spriteRenderer;

    //====================================inner variables======================================
    [SerializeField] int count;

    //=======================================caching===========================================
    [SerializeField] float waitCallSec;
    private WaitForSeconds waitSec;

    private void Awake()
    {
        //caching
        waitSec = new WaitForSeconds(waitCallSec);

        //Table Copy
        TempTable = FoodList.ToArray().ToList();

        //cur guest data init
        curGuest = null;

        //delegate chain
        EventReciver.NewCostomer += NewCostomerPord;
        EventReciver.DoSubmission += DoSubmission;
    }
    private void Start()
    {
        EventReciver.CallNewComstomer();
    }

    private void OnDestroy()
    {
        EventReciver.NewCostomer -= NewCostomerPord;
        EventReciver.DoSubmission -= DoSubmission;
    }

    //=====================================Submission==========================================
    void DoSubmission()
    {
        count++;
        if (count == 2)
        {
            count = 0;
            StartCoroutine(nameof(FoodReset));
        }
    }
    IEnumerator FoodReset()
    {
        yield return waitSec;

        PoolCp.Inst.DestoryObjectCp(particleR);
        PoolCp.Inst.DestoryObjectCp(particleL);

        EventReciver.CallScoreModi(requireFood.Score);
        EventReciver.CallNewComstomer();
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
                talkBubble.sprite = curGuest.talkBubbleImage;
            }

            //recipe pick
            if (timer > moveCurve.keys[^1].time)
            {
                PickRecipe();
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

        spriteRenderer.sprite = requireFood.FoodImage;

        SetDataR = requireFood.needSet[0];
        SetDataL = requireFood.needSet[1];
        
        //call new order
        EventReciver.CallNewOrder();

        if (TempTable.Count == 0) TempTable = FoodList.ToArray().ToList();
    }
}
