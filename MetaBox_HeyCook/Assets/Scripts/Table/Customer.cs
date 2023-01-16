using ObjectPoolCP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //=====================================Reference Data======================================
    [Header("Reference Data")]
    [SerializeField] List<RecipeData> RecipeList = null; //할 수 있는 요리 리스트 SO로 뭉탱이로 바꾸기
    //[SerializeField] TalkData 대사정보

    //=====================================Reference Obj=======================================
    [Header("Current Recipe")]
    [SerializeField] public RecipeData requireRecipe = null;

    [Header("Current Ingredient")]
    [SerializeField] Ingredient tempIngred = null;

    //=======================================Component==========================================
    [SerializeField] SpriteRenderer spriteRenderer = null;

    //===================================inner variables========================================
    Vector3 targetPos = Vector3.zero;

    float tableRoundX;
    float tableRoundY;

    private void Awake()
    {
        Initializing();
        PickRecipe();
    }

    //==================================Init Inner Variables=====================================
    private void Initializing()
    {
        tableRoundX = Mathf.Round(this.transform.position.x * 10f) / 10f;
        tableRoundY = Mathf.Round(this.transform.position.y * 10f) / 10f;
        targetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1f);
    }

    //=======================================Food In Out==========================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.GetComponent<Ingredient>();
            tempIngred = contactIngred;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            if (!tempIngred.IsCliked)
            {
                StartCoroutine(nameof(TargetMove));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            if (tempIngred.gameObject == collision.gameObject)
            {
                tempIngred = null;
            }
        }
    }

    //===================================Ingredient Move Production===============================
    IEnumerator TargetMove()
    {
        while (true)
        {
            if (tempIngred == null) yield break;

            Transform targetTr = tempIngred.transform;

            float fixedX = Mathf.Round(targetTr.position.x * 10f) / 10f;
            float fixedY = Mathf.Round(targetTr.position.y * 10f) / 10f;

            if (tableRoundX == fixedX && tableRoundY == fixedY)
            {
                targetTr.position = targetPos;

                FoodCompare();
                yield break;
            }

            targetTr.position = Vector3.Lerp(targetTr.position, targetPos, Time.deltaTime * 10f);

            yield return null;
        }
    }

    //=====================================Ingredient Data Compare================================
    void FoodCompare()
    {
        if (tempIngred.IsCooked)
        {
            if (tempIngred.RecipeData == requireRecipe)
            {
                Debug.Log("맛있다!");
                EventReciver.CallScoreModi(100);
            }
            else
            {
                Debug.Log("맛없다!");
                EventReciver.CallScoreModi(-10);
            }
        }
        else
        {
            if (tempIngred.IsTrimed)
            {
                Debug.Log("안 익었다!");
                EventReciver.CallScoreModi(-30);
            }
            else
            {
                Debug.Log("퉤!");
                EventReciver.CallScoreModi(-50);
            }
        }
        PoolCp.Inst.DestoryObjectCp(tempIngred.gameObject);
        tempIngred = null;

        PickRecipe();
    }
    void PickRecipe()
    {
        int index = Random.Range(0, RecipeList.Count);
        requireRecipe = RecipeList[index];
        spriteRenderer.sprite = requireRecipe.cooked;

        EventReciver.CallNewComstomer();
    }
}
