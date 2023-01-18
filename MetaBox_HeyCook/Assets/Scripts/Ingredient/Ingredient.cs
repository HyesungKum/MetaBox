using ObjectPoolCP;
using System.Collections;
using UnityEngine;

//Different Cook and Trimable Ways Enum
public enum TrimType
{
    Pressing,
    Touching,
    Slicing,
    Max
}
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
    public RecipeData RecipeData = null;
    public IngredData IngredData = null;

    //============================Component================================
    [Header("Component")]
    [SerializeField] private Rigidbody2D Rigidbody2D = null;
    [SerializeField] private BoxCollider2D BoxCollider2D = null;
    [SerializeField] private SpriteRenderer SpriteRenderer = null;

    //============================Flag=====================================
    [Header("Flag")]
    [SerializeField] private float Lifetimer = 0;
    [Space]
    public bool IsCliked;
    [Space]
    public bool TrimReady;
    public bool IsTrimed;
    [Space]
    public bool IsCooked;
    public bool IsCookReady;

    //============================trimControll=============================
    [Header("TrimControll")]
    public float needTask;
    public float curTask = 0;
    public TrimType TrimType;

    //==================================cookControll========================
    public CookType CookType;

    private void Awake()
    {
        //bring component
        Rigidbody2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Initializing();
    }

    //===================================Initailizing component and inner variables==========================
    /// <summary>
    /// sprite and collider, tag, flag initializing
    /// </summary>
    public void Initializing()
    {
        //sprite and collider
        if (IngredData != null)
        {
            SpriteRenderer.sprite = IngredData.ingredientImage;
            BoxCollider2D.size = SpriteRenderer.sprite.bounds.size;
        }
        else if (RecipeData != null)
        {
            SpriteRenderer.sprite = RecipeData.raw;
            BoxCollider2D.size = SpriteRenderer.sprite.bounds.size;
        }
        //tagging
        this.transform.tag = "Ingredient";

        //flag
        IsCliked = false;

        TrimReady = false;
        IsTrimed = false;

        IsCookReady = false;
        IsCooked = false;

        Lifetimer = 0;

        StartCoroutine(nameof(LifeCycle));
    }

    //=====================================Trim & Cook Task Controll=========================================
    /// <summary>
    /// check this ingredient can trimable and do right intrecting
    /// </summary>
    public void OnTrim()
    {
        if (IsTrimed || IngredData == null) return;

        if (TrimReady)
        {
            BoxCollider2D.enabled = false;
        }

        if (curTask >= needTask)
        {
            SpriteRenderer.sprite = IngredData.trimedImage;
            BoxCollider2D.enabled = true;
            IsTrimed = true;
            curTask = 0;
        }
    }
    /// <summary>
    /// check this ingredient can cookable and do right intrecting
    /// </summary>
    public void OnCook()
    {
        if (IsCooked || RecipeData == null) return;

        if (IsCookReady)
        {
            BoxCollider2D.enabled = false;
        }

        if (curTask >= needTask)
        {
            SpriteRenderer.sprite = RecipeData.cooked;
            BoxCollider2D.enabled = true;
            IsCooked = true;
            curTask = 0;
        }
    }

    IEnumerator LifeCycle()
    {
        while (this.gameObject != null)
        {
            if (IsTrimed) yield break;

            if (IsCliked) { Lifetimer = 0; }
            else { Lifetimer += Time.deltaTime; }

            if (IngredData != null)
            {
                if (Lifetimer > IngredData.lifeTime)
                {
                    PoolCp.Inst.DestoryObjectCp(this.gameObject);
                }
            }

            yield return null;
        }
    }
}
