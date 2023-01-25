using ObjectPoolCP;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(BoxCollider2D))]
//public class TrimTable : MonoBehaviour
//{
//    //====================================ObjectReferance=============================================
//    [Header("Current Ingredient")]
//    [SerializeField] private Ingredient targetIngred = null;
//    [SerializeField] private Ingredient tempIngred = null;

//    [Header("Trim Slider")]
//    [SerializeField] private GameObject trimSliderObj = null;
//    private Slider trimSlider = null;

//    //=====================================Table Setting==============================================
//    [Header("Table Value")]
//    [SerializeField] float IngredMoveSpeed;

//    //=====================================inner variables============================================
//    private Vector3 targetPos = Vector3.zero;

//    private float tableRoundX;
//    private float tableRoundY;

//    private void Awake()
//    {
//        //Reference components
//        trimSlider = trimSliderObj.GetComponent<Slider>();
        
//        //Tagging
//        this.transform.tag = "Table";

//        //init inner variables
//        Initailizing();
//    }

//    //=====================================Initailize Inner Variables=======================================
//    private void Initailizing()
//    {
//        //TablePosition Round
//        tableRoundX = Mathf.Round(this.transform.position.x);
//        tableRoundY = Mathf.Round(this.transform.position.y);

//        //Ingredient Position Setting
//        targetPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1f);

//        //Trim slider setting
//        trimSliderObj.transform.position = targetPos + Vector3.up * 3f;
//        trimSliderObj.SetActive(false);
//    }

//    //===========================================Trim Task Controll==========================================
//    public void Pressing()
//    {
//        if (targetIngred == null) return;
//        if (!(targetIngred.TrimReady && targetIngred.TrimType == TrimType.Pressing)) return;

//        //누르기 이펙트

//        TaskControll();
//    }
//    public void Touching()
//    {
//        if (targetIngred == null) return;
//        if (!(targetIngred.TrimReady && targetIngred.TrimType == TrimType.Touching)) return;

//        //터치 이펙트

//        TaskControll();
//    }
//    public void Slicing()
//    {
//        if (targetIngred == null) return;
//        if (!(targetIngred.TrimReady && targetIngred.TrimType == TrimType.Slicing)) return;

//        //자르기 이펙트

//        TaskControll();
//    }
//    private void TaskControll()
//    {
//        targetIngred.curTask++;
//        trimSlider.value = targetIngred.curTask / targetIngred.needTask;

//        if (trimSlider.value == 1)
//        {
//            trimSliderObj.SetActive(false);
//            //targetIngred.OnTrim();
//            targetIngred = null;
//        }
//    }

//    //=========================================Ingredeint In Out ============================================
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag(nameof(Ingredient)))
//        {
//            Ingredient contactIngred = collision.transform.GetComponent<Ingredient>();

//            if (contactIngred.IsTrimed) return;
//            if (contactIngred.IsCooked) return;

//            tempIngred = contactIngred;
//        }
//    }
//    void OnTriggerStay2D(Collider2D collision)
//    {
//        if (tempIngred == null) return;

//        if (collision.gameObject == tempIngred.gameObject)
//        {
//            if (!tempIngred.IsCliked)
//            {
//                StartCoroutine(nameof(TargetMove));
//            }
//        }
//    }
//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (tempIngred == null) return;

//        if (collision.CompareTag(nameof(Ingredient)))
//        {
//            if (collision.transform.gameObject == tempIngred.gameObject)
//            {
//                if (tempIngred.IsCliked)
//                {
//                    tempIngred = null;
//                }
//            }
//        }
//    }

//    //======================================Target Obj Move Production=======================================
//    IEnumerator TargetMove()
//    {
//        Transform targetTr = tempIngred.transform;

//        while (true)
//        {
//            if (tempIngred == null) yield break;

//            //lerp and round position
//            float fixedX = Mathf.Round(targetTr.position.x);
//            float fixedY = Mathf.Round(targetTr.position.y);

//            targetTr.position = Vector3.Lerp(targetTr.position, targetPos, Time.deltaTime * IngredMoveSpeed);

//            if (tableRoundX == fixedX && tableRoundY == fixedY)
//            {
//                //position controll
//                targetTr.position = targetPos;

//                tempIngred.TrimReady = true;

//                trimSliderObj.SetActive(true);
//                trimSliderObj.transform.position = targetPos + Vector3.up * 3f;
//                trimSlider.value = 0;

//                //current handling ingredient controll
//                if (targetIngred != null) PoolCp.Inst.DestoryObjectCp(targetIngred.gameObject);

//                targetIngred = tempIngred;
//                //targetIngred.OnTrim();
//                tempIngred = null;

//                yield break;
//            }
            
//            yield return null;
//        }
//    }
//}
