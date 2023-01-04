using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("All Recipe List")]
    [SerializeField] List<RecipeData> RecipeList = null; //�� �� �ִ� �丮 ����Ʈ

    [SerializeField] public RecipeData requireRecipe = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    [SerializeField] Ingredient tempIngred = null; //��� ������� ����

    public List<string> customerText;

    //================inner variables=======================
    Vector3 tablePos = Vector3.zero;

    float tableRoundX;
    float tableRoundY;

    private void Awake()
    {
        tableRoundX = Mathf.Round(this.transform.position.x * 10f) / 10f;
        tableRoundY = Mathf.Round(this.transform.position.y * 10f) / 10f;
        tablePos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1f);

        PickRecipe();
    }

    private void Update()
    {
        TargetMove();
    }

    void TargetMove()
    {
        if (tempIngred == null || tempIngred.transform.position == tablePos) return;

        if (!tempIngred.IsCliked)
        {
            Transform targetTr = tempIngred.transform;

            float fixedX = Mathf.Round(targetTr.position.x * 10f) / 10f;
            float fixedY = Mathf.Round(targetTr.position.y * 10f) / 10f;

            if (tableRoundX == fixedX && tableRoundY == fixedY)
            {
                Debug.Log("����Ȯ��");
                targetTr.position = tablePos;

                FoodCompare();
            }

            targetTr.position = Vector3.Lerp(targetTr.position, tablePos, Time.deltaTime * 10f);
        }
    }
    void FoodCompare()
    {
        if (tempIngred.IsCooked)
        {
            if (tempIngred.RecipeData == requireRecipe)
            {
                Debug.Log("���ִ�!");

                EventReciver.CallScoreModi(100);
            }
            else
            {
                Debug.Log("������!");
                EventReciver.CallScoreModi(-10);
            }
        }
        else
        {
            if (tempIngred.IsTrimed)
            {
                Debug.Log("�� �;���!");
                EventReciver.CallScoreModi(-30);
            }
            else
            {
                Debug.Log("ơ!");
                EventReciver.CallScoreModi(-50);
            }
        }
        Destroy(tempIngred.gameObject);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.GetComponent<Ingredient>();

            if (!contactIngred.IsCliked)
            {
                tempIngred = contactIngred;
            }
        }
    }
}
