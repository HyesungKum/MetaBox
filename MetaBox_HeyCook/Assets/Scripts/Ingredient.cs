using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ingredient : MonoBehaviour
{
    //===============data=====================================
    [Header("Data")]
    public RecipeData RecipeData = null;
    public IngredData IngredData = null;

    //===============component================================
    [Header("Component")]
    public Rigidbody2D Rigidbody2D = null;
    public BoxCollider2D BoxCollider2D = null;
    public SpriteRenderer SpriteRenderer = null;

    //===============flag=====================================
    [Header("Flag")]
    public bool IsCliked;

    public bool TrimReady;
    public bool IsTrimed;

    public bool IsCooked;
    public bool IsCookReady;

    //===============trimControll=============================
    [Header("TrimControll")]

    public float needTask;
    public float curTask = 0;
    public TrimType TrimType;

    //=====================cookControll========================
    public CookType CookType;

    private void Awake()
    {
        //bring component
        Rigidbody2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        Initializing();
    }

    private void Update()
    {
        TrimCollCtl();
        CookCollCtl();
    }

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
    }
    
    /// <summary>
    /// movable collider controll when triming
    /// </summary>
    void TrimCollCtl()
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
    /// movable collider controll when Cooking
    /// </summary>
    void CookCollCtl()
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
}
