using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class TrimTable : MonoBehaviour
{
    //================ObjectReferance=======================
    [SerializeField] Ingredient targetIngred = null;
    private Ingredient tempIngred = null;

    [SerializeField] GameObject trimSliderObj = null;
    Slider trimSlider = null;

    private SpriteRenderer spriteRenderer = null;

    //================inner variables=======================
    Vector3 tablePos = Vector3.zero;
    Vector3 sliderPos = Vector3.zero;

    float tableRoundX;
    float tableRoundY;

    private void Awake()
    {
        //
        trimSliderObj.transform.position = Camera.main.ScreenToWorldPoint(this.transform.position);
        trimSliderObj.SetActive(false);

        trimSlider = trimSliderObj.GetComponent<Slider>();

        //
        tableRoundX = Mathf.Round(this.transform.position.x * 10f) / 10f;
        tableRoundY = Mathf.Round(this.transform.position.y * 10f) / 10f;

        tablePos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1f);

        sliderPos = trimSliderObj.transform.position;
        //
        this.transform.tag = "Table";

        //
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        TargetControll();
        TargetMove();
        IngredCheak();
    }

    void IngredCheak()
    {
        if (targetIngred == null) return;

        if(targetIngred.IsTrimed)
        {
            trimSliderObj.SetActive(false);
            tempIngred = null;
            targetIngred = null;
        }
    }
    void TargetControll()
    {
        if (targetIngred == null) return;

        if (tempIngred == null)
        {
            tempIngred = targetIngred;
        }
        else if(tempIngred != targetIngred && !targetIngred.IsCliked)
        {
            Destroy(tempIngred.gameObject);
            tempIngred = targetIngred;
        }
    }
    void TargetMove()
    {
        if (targetIngred == null || targetIngred.transform.position == tablePos) return;

        if (!targetIngred.IsCliked)
        {
            Transform targetTr = targetIngred.transform;

            float fixedX = Mathf.Round(targetTr.position.x * 10f) / 10f;
            float fixedY = Mathf.Round(targetTr.position.y * 10f) / 10f;

            if (tableRoundX == fixedX && tableRoundY == fixedY)
            {
                targetTr.position = tablePos;

                targetIngred.TrimReady = true;

                trimSliderObj.SetActive(true);
                trimSliderObj.transform.position = Camera.main.WorldToScreenPoint( tablePos + Vector3.up * 2f);
            }

            targetTr.position = Vector3.Lerp(targetTr.position, tablePos, Time.deltaTime * 3f);
        }
    }

    public void Pressing()
    {
        if (targetIngred == null) return;
        if (!(targetIngred.TrimReady && targetIngred.TrimType == TrimType.Pressing)) return;

        //누르기 이펙트

        targetIngred.curTask++;
        trimSlider.value = targetIngred.curTask / targetIngred.needTask;
    }
    public void Touching()
    {
        if (targetIngred == null) return;
        if (!(targetIngred.TrimReady && targetIngred.TrimType == TrimType.Touching)) return;

        //터치 이펙트
        
        targetIngred.curTask++;
        trimSlider.value = targetIngred.curTask / targetIngred.needTask;
    }
    public void Slicing()
    {
        if (targetIngred == null) return;
        if (!(targetIngred.TrimReady && targetIngred.TrimType == TrimType.Slicing)) return;

        //자르기 이펙트

        targetIngred.curTask++;
        trimSlider.value = targetIngred.curTask / targetIngred.needTask;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

            if (contactIngred != tempIngred)
            {
                targetIngred = contactIngred;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(nameof(Ingredient)))
        {
            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

            if (targetIngred == null || targetIngred.IsTrimed) return;

            if (!targetIngred.TrimReady && contactIngred == targetIngred)
            {
                targetIngred = null;
                tempIngred = null;
            }
        }
    }
}
