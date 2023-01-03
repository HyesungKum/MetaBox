using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookTable : MonoBehaviour
{
    //================ObjectReferance=======================
    [Header("All Recipe List")]
    [SerializeField] List<RecipeData> RecipeList = null; //�� �� �ִ� �丮 ����Ʈ

    [Header("Require Recipe data")]
    [SerializeField] RecipeData requireRecipe = null; //�ؾ��ϴ� �ִ� �丮
    [SerializeField] List<IngredData> curNeedIngred = null; //���� �־���ϴ� ��� ����Ʈ

    [Header("current ingred in pot")]
    [SerializeField] Ingredient tempIngred = null; //��� ������� ����
    [SerializeField] Ingredient rawFood = null;//�������� �������� ����

    //===================component======================================
    private SpriteRenderer spriteRenderer = null;

    //=====================slider=======================================
    [SerializeField] GameObject cookSliderObj = null;
    Slider cookSlider = null;

    //================inner variables=======================
    Vector3 tablePos = Vector3.zero;
    Vector3 sliderPos = Vector3.zero;

    float tableRoundX;
    float tableRoundY;

    [SerializeField] bool nowCooking = false;

    private void Awake()
    {
        //
        cookSliderObj.transform.position = Camera.main.ScreenToWorldPoint(this.transform.position);
        cookSliderObj.SetActive(false);

        cookSlider = cookSliderObj.GetComponent<Slider>();

        //
        tableRoundX = Mathf.Round(this.transform.position.x * 10f) / 10f;
        tableRoundY = Mathf.Round(this.transform.position.y * 10f) / 10f;

        tablePos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1f);

        sliderPos = cookSliderObj.transform.position;
        //
        this.transform.tag = "Table";

        //
        spriteRenderer = GetComponent<SpriteRenderer>();

        //
        PickRecipe();
    }

    void Update()
    {
        TargetMove();
        Cooking();
    }
    void TargetMove()
    {
        if (nowCooking || tempIngred == null || tempIngred.transform.position == tablePos) return;
    
        if (!tempIngred.IsCliked)
        {
            Transform targetTr = tempIngred.transform;
    
            float fixedX = Mathf.Round(targetTr.position.x * 10f) / 10f;
            float fixedY = Mathf.Round(targetTr.position.y * 10f) / 10f;
    
            if (tableRoundX == fixedX && tableRoundY == fixedY)
            {
                targetTr.position = tablePos;

                tempIngred.IsCookReady = true;
                RecipeCheck();
            }
    
            targetTr.position = Vector3.Lerp(targetTr.position, tablePos, Time.deltaTime * 3f);
        }
    }
    void RecipeCheck()
    {
        if (nowCooking) return;

        for (int i = 0; i < curNeedIngred.Count; i++)
        {
            if (tempIngred != null && curNeedIngred[i] == tempIngred.IngredData)
            {
                Debug.Log("���� ���!");

                //Ȯ���� ��� �����
                curNeedIngred.RemoveAt(i);

                if (curNeedIngred.Count == 0)
                {
                    Debug.Log("�丮 �غ�");
                    //food setting
                    Ingredient cookingIngred = tempIngred;

                    cookingIngred.RecipeData = requireRecipe;
                    cookingIngred.IngredData = null;
                    cookingIngred.Initializing();
                    
                    rawFood = tempIngred;
                    rawFood.gameObject.transform.position = tablePos + (Vector3.back * 2f);
                    rawFood.IsCookReady = true;
                    rawFood.gameObject.name = rawFood.RecipeData.recipeName;
                    
                    tempIngred = null;

                    //cooktable setting
                    nowCooking = true;
                    cookSliderObj.SetActive(true);
                    cookSliderObj.transform.position = Camera.main.WorldToScreenPoint(tablePos + Vector3.up * 2f);

                    return;
                }
            }
            else
            {
                Debug.Log("Ʋ�� ���!");
            }
        }

        Destroy(tempIngred.gameObject);
        tempIngred = null;
    }
    void PickRecipe()
    {
        int index = Random.Range(0, RecipeList.Count);
        requireRecipe = RecipeList[index];

        //ingredient list
        for (int i = 0; i < requireRecipe.needIngred.Length; i++)
        {
            curNeedIngred.Add(requireRecipe.needIngred[i]);
        }
    }
    void Cooking()
    {
        if (!nowCooking || rawFood == null) return;

        if (rawFood.IsCooked)
        {
            rawFood = null;
            nowCooking = false;
            cookSliderObj.SetActive(false);
            PickRecipe();
        }
    }

    public void Pressing()
    {
        if (!nowCooking || rawFood == null) return;
        if (!(rawFood.IsCookReady && rawFood.CookType == CookType.Pressing)) return;

        //������ ����Ʈ

        rawFood.curTask++;
        cookSlider.value = rawFood.curTask / rawFood.needTask;
    }
    public void Touching()
    {
        if (!nowCooking || rawFood == null) return;
        if (!(rawFood.IsCookReady && rawFood.CookType == CookType.Touching)) return;

        //������ ����Ʈ

        rawFood.curTask++;
        cookSlider.value = rawFood.curTask / rawFood.needTask;
    }
    public void Slicing()
    {
        if (!nowCooking || rawFood == null) return;
        if (!(rawFood.IsCookReady && rawFood.CookType == CookType.Slicing)) return;

        //�ڸ��� ����Ʈ

        rawFood.curTask++;
        cookSlider.value = rawFood.curTask / rawFood.needTask;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

            if ( contactIngred!= null && contactIngred.IsTrimed)
            {
                tempIngred = contactIngred;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

            if (tempIngred == null || tempIngred.IsCooked) return;

            if (contactIngred == tempIngred)
            {
                tempIngred = null;
            }
        }
    }
}