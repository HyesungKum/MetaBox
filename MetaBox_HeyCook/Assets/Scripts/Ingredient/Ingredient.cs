using ObjectPoolCP;
using System.Collections;
using UnityEngine;

//Different Cook Ways Enum
public enum CookType
{
    Pressing,
    Touching,
    Slicing,
    Max
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour
{
    //============================Data=====================================
    [Header("Data")]
    public FoodData FoodData = null;
    public IngredData IngredData = null;

    //============================Component================================
    [Header("Component")]
    [SerializeField] private Rigidbody2D Rigidbody = null;
    [SerializeField] private BoxCollider2D Collider = null;
    [SerializeField] private SpriteRenderer Renderer = null;

    //============================Flag=====================================
    [Header("Setting")]
    [SerializeField] private float Lifetimer = 0;
    [Space]
    public bool IsCliked;
    [Space]
    public bool IsCooked;
    public bool IsCookReady;

    //============================CookControll=============================
    [Header("[CookControll]")]
    public float curTask = 0;

    //============================Caching==================================
    WaitForSeconds waitSec = new(0.2f);

    public void Reset()
    {
        this.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        this.transform.localScale = Vector3.one * 0.3f;
    }

    private void Awake()
    {
        //bring component
        TryGetComponent<Rigidbody2D>(out Rigidbody);
        TryGetComponent<BoxCollider2D>(out Collider);
        TryGetComponent<SpriteRenderer>(out Renderer);

        //Init when this object awake
        InitAwake();
    }

    private void OnEnable()
    {
        InitSpawn();
    }

    //===================================Initailizing component and inner variables==========================
    /// <summary>
    /// Call this function for GameObject First Initailizing
    /// this function just onetime called because Ingredient Object Naver Destory before scene closed
    /// </summary>
    public void InitAwake()
    {
        //tagging
        if (this.gameObject.tag == "Untagged") this.transform.tag = "Ingredient";
    }
    /// <summary>
    /// sprite and collider, tag, flag and value initializing
    /// called when this object enabled
    /// </summary>
    public void InitSpawn()
    {
        //sprite and collider
        Renderer.sprite = IngredData.ingredientImage;
        Renderer.color = Color.white;
        Renderer.sortingOrder = 4;

        Collider.size = Renderer.sprite.bounds.size * IngredData.interDistance;
        Collider.enabled = true;

        curTask = 0;

        //flag
        IsCliked = false;

        IsCookReady = false;
        IsCooked = false;
        
        Lifetimer = 0;

        StartCoroutine(nameof(LifeCycle));
    }

    //============================================Ingredient Controll=========================================
    /// <summary>
    /// Call when Ingredient Ready to cook
    /// </summary>
    public void ReadyCook()
    {
        IsCookReady = true;
        Renderer.sortingOrder = 3;
        Collider.enabled = false;
    }

    //===========================================Ingredient LifeCycle=========================================
    IEnumerator LifeCycle()
    {
        while (this.gameObject != null)
        {
            if (IsCookReady) yield break;

            if (IsCliked) { Lifetimer = 0; }
            else { Lifetimer += Time.deltaTime; }

            if (IngredData != null)
            {
                if (Lifetimer > IngredData.lifeTime)
                {
                    yield return FadeOut();
                }
            }

            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        yield return waitSec;

        float timer = 0f;
        Collider.enabled = false;

        while (Renderer.color.a != 0)
        {
            timer += Time.deltaTime;
            Renderer.color = Color.Lerp(Renderer.color, Color.clear, timer);

            yield return null;
        }

        PoolCp.Inst.DestoryObjectCp(this.gameObject);
    }

    //===========================================public contact===============================================
    public void DoFadeOut()
    {
        StartCoroutine(nameof(FadeOut));
    }
    public void SetImage(Sprite sprite, int order)
    {
        Renderer.sprite = sprite;
        Renderer.sortingOrder = order;
    }
}
