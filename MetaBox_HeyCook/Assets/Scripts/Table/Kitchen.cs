using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum TablePos
{
    Right,
    Left
}

public class Kitchen : MonoBehaviour
{
    //================================Reference Customer=====================================
    [Header("Reference Customer")]
    [SerializeField] Submission submission = null;
    [SerializeField] TablePos CurTablePos;

    //==================================ObjectReferance======================================
    [Header("Require Recipe data")]
    [SerializeField] SpriteRenderer BillboradRenderer;
    [SerializeField] SetData setOrder = null;
    [SerializeField] List<IngredData> curNeedIngred = new();

    //================================Current Ingredient in Pot==============================
    [Header("current ingred in pot")]
    [SerializeField] Ingredient curIngred = null;
    [SerializeField] Ingredient tempIngred = null;
    [SerializeField] Ingredient rawSet = null;

    //===================================Table Setting=======================================
    [Header("Table Value")]
    [SerializeField] float servingSpeed;
    [SerializeField] float ingredMoveSpeed;

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
        cookSlider = cookSliderObj.GetComponent<Slider>();

        //Tagging
        this.transform.tag = "Table";

        //delegate chain
        EventReciver.NewOrder += NewCustomerOrder;

        //init inner variables 
        Initailizing();
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReciver.NewOrder -= NewCustomerOrder;
    }

    //=================================Initializing=====================================
    private void Initailizing()
    {
        //TablePosition Round
        tableRoundX = Mathf.Round(this.transform.position.x);
        tableRoundY = Mathf.Round(this.transform.position.y);

        //submission Position Round
        subRoundX = Mathf.Round(submission.transform.position.x);
        subRoundY = Mathf.Round(submission.transform.position.y);

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
        if (!nowCooking || rawSet == null) return;
        if (!(rawSet.IsCookReady && rawSet.CookType == CookType.Pressing)) return;

        //누르기 이펙트

        TaskContoll();
    }
    public void Touching()
    {
        if (!nowCooking || rawSet == null) return;
        if (!(rawSet.IsCookReady && rawSet.CookType == CookType.Touching)) return;

        //만지기 이펙트

        TaskContoll();
    }
    public void Slicing()
    {
        if (!nowCooking || rawSet == null) return;
        if (!(rawSet.IsCookReady && rawSet.CookType == CookType.Slicing)) return;

        //자르기 이펙트

        TaskContoll();
    }
    private void TaskContoll()
    {
        rawSet.curTask++;
        cookSlider.value = rawSet.curTask / rawSet.needTask;

        if (cookSlider.value == 1)
        {
            EventReciver.CallScoreModi(setOrder.Score);

            cookSliderObj.SetActive(false);
            nowCooking = false;

            GameObject instObj = PoolCp.Inst.BringObjectCp(setOrder.particle);
            instObj.transform.position = this.transform.position;
            StartCoroutine(nameof(ParticleMove), instObj);

            PoolCp.Inst.DestoryObjectCp(rawSet.gameObject);
        }
    }
    
    //===============================Ingredeint In Out==================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nowCooking) return;

        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

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
            tempIngred.ReadyCook();

            curNeedIngred.RemoveAt(index);

            if (curNeedIngred.Count == 0)
            {
                //rawfood setting
                rawSet = curIngred;
                rawSet.setData = setOrder;

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
        //split recipe right and left
        switch (CurTablePos)
        {
            case TablePos.Left: setOrder = submission.SetDataL; break;
            case TablePos.Right: setOrder = submission.SetDataR; break;
        }

        //apply need combine list Image
        BillboradRenderer.sprite = setOrder.CombineImage;

        //recipe data clean
        if (curNeedIngred.Count >= 0)
        {
            curNeedIngred.Clear();
        }

        //apply new order
        for (int i = 0; i < setOrder.needIngred.Length; i++)
        {
            curNeedIngred.Add(setOrder.needIngred[i]);
        }
    }

    //=========================================Production=====================================
    /// <summary>
    /// particle move to submission position and tranfer
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

            target.transform.position = Vector3.Lerp(target.transform.position, submission.transform.position, timeCache * servingSpeed);

            if (subRoundX == fixedX && subRoundY == fixedY)
            {
                target.transform.position = submission.transform.position;

                switch (CurTablePos)
                {
                    case TablePos.Left: submission.particleR = target; break;
                    case TablePos.Right: submission.particleL = target; break;
                }

                EventReciver.CallDoSubmission();

                yield break;
            }

            yield return null;
        }
    }
}