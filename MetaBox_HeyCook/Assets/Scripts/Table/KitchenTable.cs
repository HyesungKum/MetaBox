using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenTable : MonoBehaviour
{
    //================================Reference Customer=====================================
    [Header("Reference Customer")]
    [SerializeField] GuestTable guestTable = null;

    //==================================ObjectReferance======================================
    [Header("Require Recipe data")]
    [SerializeField] FoodData foodOrder = null;
    [SerializeField] List<IngredData> curNeedIngred = new();

    //================================Current Ingredient in Pot==============================
    [Header("current ingred in pot")]
    [SerializeField] Ingredient curIngred = null;
    [SerializeField] Ingredient tempIngred = null;
    [SerializeField] Ingredient rawFood = null;

    //===================================Table Setting=======================================
    [Header("Table Value")]
    [SerializeField] float servingSpeed;
    [SerializeField] float ingredMoveSpeed;
    [SerializeField] Side side;

    //=======================================Slider==========================================
    [Header("Slider Obj")]
    [SerializeField] GameObject cookSliderObj = null;
    Slider cookSlider = null;

    //===================================Inner Variables=====================================
    Vector3 targetPos = Vector3.zero;

    private float tableRoundX;
    private float tableRoundY;

    private float subRoundX;
    private float subRoundY;

    private float timeCache;

    private bool nowCooking = false;

    private void Awake()
    {
        //Reference componet
        cookSliderObj.TryGetComponent(out cookSlider);

        //Tagging
        this.transform.tag = "Table";

        //delegate chain
        if (side == Side.Right) EventReciver.NewOrderR += NewCustomerOrder;
        else EventReciver.NewOrderL += NewCustomerOrder;

        //init inner variables 
        Initailizing();
    }

    private void OnDisable()
    {
        //delegate unchain
        if (side == Side.Right) EventReciver.NewOrderR -= NewCustomerOrder;
        else EventReciver.NewOrderL -= NewCustomerOrder;
    }

    //=================================Initializing=====================================
    private void Initailizing()
    {
        //TablePosition Round
        tableRoundX = Mathf.Round(this.transform.position.x);
        tableRoundY = Mathf.Round(this.transform.position.y);

        //submission Position Round
        subRoundX = Mathf.Round(guestTable.transform.position.x);
        subRoundY = Mathf.Round(guestTable.transform.position.y);

        //Ingredient Position setting
        targetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        //Cook Table Setting
        cookSliderObj.transform.position = targetPos + Vector3.up * 3f;
        cookSliderObj.SetActive(false);

        nowCooking = false;

        //timer cache
        timeCache = Time.deltaTime;
    }

    //=================================Cooking Task=====================================
    public void Pressing()
    {
        if (!nowCooking || rawFood == null) return;
        if (!(rawFood.IsCookReady && rawFood.CookType == CookType.Pressing)) return;

        //누르기 이펙트

        TaskContoll();
    }
    public void Touching()
    {
        if (!nowCooking || rawFood == null) return;
        if (!(rawFood.IsCookReady && rawFood.CookType == CookType.Touching)) return;

        //만지기 이펙트

        TaskContoll();
    }
    public void Slicing()
    {
        if (!nowCooking || rawFood == null) return;
        if (!(rawFood.IsCookReady && rawFood.CookType == CookType.Slicing)) return;

        //자르기 이펙트

        TaskContoll();
    }
    private void TaskContoll()
    {
        rawFood.curTask++;
        cookSlider.value = rawFood.curTask / rawFood.needTask;

        if (cookSlider.value == 1)
        {
            cookSliderObj.SetActive(false);
            nowCooking = false;

            GameObject instObj = PoolCp.Inst.BringObjectCp(foodOrder.foodVfx);
            instObj.transform.position = this.transform.position;
            StartCoroutine(nameof(ParticleMove), instObj);

            PoolCp.Inst.DestoryObjectCp(rawFood.gameObject);
        }
    }
    
    //===============================Ingredeint In Out==================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nowCooking) return;

        if (collision.CompareTag(nameof(Ingredient)))
        {
            collision.transform.TryGetComponent(out Ingredient contactIngred);

            curIngred = contactIngred;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (curIngred == null) return;

        if (collision.gameObject == curIngred.gameObject)
        {
            if (!curIngred.IsCliked)
            {
                StartCoroutine(nameof(TargetMove));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (curIngred == null) return;

        if (collision.CompareTag(nameof(Ingredient)))
        {
            if (collision.transform.gameObject == curIngred.gameObject)
            {
                if (curIngred.IsCliked)
                {
                    curIngred = null;
                }
            }
        }
    }

    //=============================target obj production================================
    IEnumerator TargetMove()
    {
        curIngred.ReadyCook();
        Transform targetTr = curIngred.transform;

        while (true)
        {
            if (curIngred == null) yield break;

            float fixedX = Mathf.Round(targetTr.position.x);
            float fixedY = Mathf.Round(targetTr.position.y);

            targetTr.position = Vector3.Lerp(targetTr.position, targetPos, Time.deltaTime * ingredMoveSpeed);

            if (tableRoundX == fixedX && tableRoundY == fixedY)
            {
                targetTr.position = targetPos;

                curIngred.IsCookReady = true;

                RecipeCheck();
            }

            yield return null;
        }
    }

    //================================Recipe Controll===================================
    void RecipeCheck()
    {
        bool correct = false;
        int index = 0;

        //judge recipe ingredient
        for (int i = 0; i < curNeedIngred.Count; i++)
        {
            if (curIngred != null && curNeedIngred[i] == curIngred.IngredData)
            {
                correct = true;
                index = i;
            }
        }

        //judge make food
        if (correct)
        {
            if (tempIngred != null) PoolCp.Inst.DestoryObjectCp(tempIngred.gameObject);
            tempIngred = curIngred;

            curNeedIngred.RemoveAt(index);

            if (curNeedIngred.Count == 0)
            {
                //rawfood setting
                rawFood = curIngred;
                rawFood.FoodData = foodOrder;

                tempIngred = null;
                curIngred = null;

                //cook table setting
                nowCooking = true;
                cookSliderObj.SetActive(true);
                cookSliderObj.transform.position = targetPos + Vector3.up * 3f;
                cookSlider.value = 0;

                //call correct vfx
                EventReciver.CallCorrectIngred(targetPos);
                return;
            }
            //call correct vfx
            EventReciver.CallCorrectIngred(targetPos);
        }
        else
        {
            PoolCp.Inst.DestoryObjectCp(curIngred.gameObject);
            //call wrong vfx
            EventReciver.CallWrongIngred(targetPos);
        }

        curIngred = null;
    }
    /// <summary>
    /// bringing recipe by ref customer
    /// check ingredients about target recipe
    /// </summary>
    void NewCustomerOrder()
    {
        //get guest's order
        foodOrder = guestTable.requireFood;

        //recipe data clean
        if (curNeedIngred.Count >= 0) curNeedIngred.Clear();

        //first exception
        if (foodOrder == null) return;

        //apply new order
        for (int i = 0; i < foodOrder.needIngred.Length; i++)
        {
            curNeedIngred.Add(foodOrder.needIngred[i]);
        }
    }

    //=========================================Production=====================================
    /// <summary>
    /// particle move to guestTable's position and tranfer,
    /// Do submission when arriving at last position 
    /// </summary>
    /// <param name="target">target Object</param>
    /// <returns> null </returns>
    IEnumerator ParticleMove(GameObject target)
    {
        while (target.activeSelf)
        {
            if (target == null) yield break;

            float fixedX = Mathf.Round(target.transform.position.x);
            float fixedY = Mathf.Round(target.transform.position.y);

            target.transform.position = Vector3.Lerp(target.transform.position, guestTable.transform.position, timeCache * servingSpeed);

            if (subRoundX == fixedX && subRoundY == fixedY)
            {
                target.transform.position = guestTable.transform.position;

                guestTable.foodParticle = target;

                if (side == Side.Right) EventReciver.CallDoSubmissionR();
                else EventReciver.CallDoSubmissionL();
                
                yield break;
            }

            yield return null;
        }
    }
}