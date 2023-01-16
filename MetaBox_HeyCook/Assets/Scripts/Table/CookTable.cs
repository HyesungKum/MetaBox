using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookTable : MonoBehaviour
{
    //================================Reference Customer=====================================
    [Header("Reference Customer")]
    [SerializeField] Customer customer = null;

    //==================================ObjectReferance======================================
    [Header("Require Recipe data")]
    [SerializeField] RecipeData requireRecipe = null;
    [SerializeField] List<IngredData> curNeedIngred = new();

    //================================Current Ingredient in Pot==============================
    [Header("current ingred in pot")]
    [SerializeField] Ingredient tempIngred = null;
    [SerializeField] Ingredient rawFood = null;

    //=======================================Slider==========================================
    [Header("Slider Obj")]
    [SerializeField] GameObject cookSliderObj = null;
    Slider cookSlider = null;

    //===================================Inner Variables=====================================
    Vector3 targetPos = Vector3.zero;

    float tableRoundX;
    float tableRoundY;

    [SerializeField] bool nowCooking = false;

    private void Awake()
    {
        //Reference componet
        cookSlider = cookSliderObj.GetComponent<Slider>();

        //Tagging
        this.transform.tag = "Table";

        //delegate chain
        EventReciver.NewCostomer += BringRecipe;

        //init inner variables 
        Initailizing();
    }
    private void OnDisable()
    {
        //delegate unchain
        EventReciver.NewCostomer -= BringRecipe;
    }

    //=================================Initializing=====================================
    private void Initailizing()
    {
        //TablePosition Raound
        tableRoundX = Mathf.Round(this.transform.position.x * 10f) / 10f;
        tableRoundY = Mathf.Round(this.transform.position.y * 10f) / 10f;

        //Ingredient Position setting
        targetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        //Cook Table Setting
        cookSliderObj.transform.position = targetPos + Vector3.up * 3f;
        cookSliderObj.SetActive(false);

        nowCooking = false;
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
            rawFood.OnCook();
            rawFood = null;
        }
    }
    
    //===============================Ingredeint In Out==================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nowCooking) return;

        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

            if (!contactIngred.IsTrimed) return;
            if (contactIngred.IsCooked) return;

            tempIngred = contactIngred;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (tempIngred == null) return;

        if (collision.gameObject == tempIngred.gameObject)
        {
            if (!tempIngred.IsCliked)
            {
                StartCoroutine(nameof(TargetMove));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tempIngred == null) return;

        if (collision.CompareTag(nameof(Ingredient)))
        {
            if (collision.transform.gameObject == tempIngred.gameObject)
            {
                if (tempIngred.IsCliked)
                {
                    tempIngred = null;
                }
            }
        }
    }

    //=============================target obj production================================
    IEnumerator TargetMove()
    {
        Transform targetTr = tempIngred.transform;

        while (true)
        {
            if (tempIngred == null) yield break;

            float fixedX = Mathf.Round(targetTr.position.x * 10f) / 10f;
            float fixedY = Mathf.Round(targetTr.position.y * 10f) / 10f;

            targetTr.position = Vector3.Lerp(targetTr.position, targetPos, Time.deltaTime * 10f);

            if (tableRoundX == fixedX && tableRoundY == fixedY)
            {
                targetTr.position = targetPos;

                tempIngred.IsCookReady = true;

                RecipeCheck();
            }

            yield return null;
        }
    }

    //================================Recipe Controll===================================
    void RecipeCheck()
    {
        for (int i = 0; i < curNeedIngred.Count; i++)
        {
            if (tempIngred != null && curNeedIngred[i] == tempIngred.IngredData)
            {
                Debug.Log("옳은 재료!");

                //확인할 재료 지우기
                curNeedIngred.RemoveAt(i);

                if (curNeedIngred.Count == 0)
                {
                    Debug.Log("요리 준비");

                    //food setting
                    Ingredient cookingIngred = tempIngred;
                    tempIngred = null;

                    cookingIngred.RecipeData = requireRecipe;
                    cookingIngred.IngredData = null;
                    cookingIngred.Initializing();

                    //rawfood setting
                    rawFood = cookingIngred;
                    rawFood.gameObject.transform.position = targetPos + (Vector3.back);
                    rawFood.IsCookReady = true;
                    rawFood.gameObject.name = rawFood.RecipeData.recipeName;
                    rawFood.OnCook();

                    tempIngred = null;

                    //cook table setting
                    nowCooking = true;
                    cookSliderObj.SetActive(true);
                    cookSliderObj.transform.position = targetPos + Vector3.up * 3f;
                    cookSlider.value = 0;

                    return;
                }
            }
            else
            {
                Debug.Log("틀린 재료!");
            }
        }

        PoolCp.Inst.DestoryObjectCp(tempIngred.gameObject);
        tempIngred = null;
    }
    /// <summary>
    /// bringing recipe by ref customer
    /// check ingredients about target recipe
    /// </summary>
    void BringRecipe()
    {
        requireRecipe = customer.requireRecipe;

        if (curNeedIngred.Count >= 0)
        {
            curNeedIngred.Clear();
        }

        for (int i = 0; i < requireRecipe.needIngred.Length; i++)
        {
            curNeedIngred.Add(requireRecipe.needIngred[i]);
        }
    }
}
