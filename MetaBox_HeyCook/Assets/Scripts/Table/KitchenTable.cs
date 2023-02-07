using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class KitchenTable : MonoBehaviour
{
    //==================================Component============================================
    [HideInInspector] public BoxCollider2D collider = null;

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
    [SerializeField] GameObject CookingVfx = null;
    private VfxModule module = null;

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
    /// <summary>
    /// target Postition for Ingred move to KitchenTable
    /// </summary>
    private Vector3 TargetPos { get; set; }

    private float TableRoundX { get; set; }
    private float TableRoundY { get; set; }

    private float SubRoundX { get; set; }
    private float SubRoundY { get; set; }
    
    private float TimeCache { get; set; }
    
    private bool NowCooking { get; set; }

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
        //component
        TryGetComponent(out collider);

        //TablePosition Round
        TableRoundX = Mathf.Round(this.transform.position.x);
        TableRoundY = Mathf.Round(this.transform.position.y);

        //submission Position Round
        SubRoundX = Mathf.Round(guestTable.transform.position.x);
        SubRoundY = Mathf.Round(guestTable.transform.position.y);

        //Ingredient Position setting
        TargetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        //Cook Table Setting
        cookSliderObj.transform.position = TargetPos + Vector3.up * 3f;
        cookSliderObj.SetActive(false);

        NowCooking = false;

        //timer cache
        TimeCache = Time.deltaTime;
    }

    //=================================Cooking Task=====================================
    public void Pressing()
    {
        if (!NowCooking || rawFood == null) return;
        if (!rawFood.IsCookReady) return;
        if (rawFood.FoodData.cookType != CookType.Pressing) return;

        SoundManager.Inst.CallSfx("Cutting");

        TaskContoll();
    }
    public void Touching()
    {
        if (!NowCooking || rawFood == null) return;
        if (!rawFood.IsCookReady) return;
        if (rawFood.FoodData.cookType != CookType.Touching) return;

        SoundManager.Inst.CallSfx("Rowing");

        TaskContoll();
    }
    public void Slicing()
    {
        if (!NowCooking || rawFood == null) return;
        if (!rawFood.IsCookReady) return;
        if (rawFood.FoodData.cookType != CookType.Slicing) return;

        SoundManager.Inst.CallSfx("Steaming");

        TaskContoll();
    }
    private void TaskContoll()
    {
        rawFood.curTask++;
        cookSlider.value = rawFood.curTask / rawFood.FoodData.needTask;
        if(module != null) module.DynamicObj.CallDoDynamic();

        if (cookSlider.value == 1)
        {
            //cooking done
            NowCooking = false;

            //cooking done sfx out
            SoundManager.Inst.CallSfx("CookingDone");

            //Cook process object disable
            cookSliderObj.SetActive(false);
            CookingVfx.SetActive(false);

            //cooking complite vfx enable
            GameObject instObj = PoolCp.Inst.BringObjectCp(foodOrder.foodVfx);
            instObj.transform.position = this.transform.position;

            //food image setting
            rawFood.SetImage(rawFood.FoodData.foodImage, 4);

            //food moving
            StartCoroutine(nameof(FoodMove), rawFood.gameObject);
        }
    }
    
    //===============================Ingredeint In Out==================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (NowCooking) return;

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

            targetTr.position = Vector3.Lerp(targetTr.position, TargetPos, Time.deltaTime * ingredMoveSpeed);

            if (TableRoundX == fixedX && TableRoundY == fixedY)
            {
                targetTr.position = TargetPos;

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

        //judge make a food
        if (correct)
        {
            //Call Correct Ingredient Vfx & Sfx
            EventReciver.CallCorrectIngred(TargetPos);
            SoundManager.Inst.CallSfx("Correct");

            //temp ingred Controll
            if (tempIngred != null) PoolCp.Inst.DestoryObjectCp(tempIngred.gameObject);
            tempIngred = curIngred;

            curNeedIngred.RemoveAt(index);

            //cooking start point
            if (curNeedIngred.Count == 0)
            {
                //rawfood setting
                rawFood = curIngred;
                rawFood.FoodData = foodOrder;

                tempIngred = null;
                curIngred = null;

                //cook table setting
                NowCooking = true;

                cookSliderObj.SetActive(true);
                cookSliderObj.transform.position = TargetPos + Vector3.up * 3f;
                
                cookSlider.value = 0;

                CookingVfx = PoolCp.Inst.BringObjectCp(rawFood.FoodData.cookingVfx);
                CookingVfx.transform.position = TargetPos;
                CookingVfx.TryGetComponent(out module);

                return;
            }
        }
        else
        {

            PoolCp.Inst.DestoryObjectCp(curIngred.gameObject);

            //call wrong vfx & Sfx
            EventReciver.CallWrongIngred(TargetPos);
            SoundManager.Inst.CallSfx("Wrong");
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
    IEnumerator FoodMove(GameObject target)
    {
        while (target.activeSelf)
        {
            if (target == null) yield break;

            float fixedX = Mathf.Round(target.transform.position.x);
            float fixedY = Mathf.Round(target.transform.position.y);

            target.transform.position = Vector3.Lerp(target.transform.position, guestTable.transform.position, TimeCache * servingSpeed);

            if (SubRoundX == fixedX && SubRoundY == fixedY)
            {
                target.transform.position = guestTable.transform.position;

                if (side == Side.Right) EventReciver.CallDoSubmissionR();
                else EventReciver.CallDoSubmissionL();

                rawFood.DoFadeOut();

                yield break;
            }

            yield return null;
        }
    }
}