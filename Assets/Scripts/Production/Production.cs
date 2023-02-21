using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public abstract class Production : MonoBehaviour
{
    //========================================Production Settings========================================
    [Header("Production Setting")]
    [SerializeField] protected float prodSpeed = 1f;
    [SerializeField] protected float prodTime = 1f;
    [SerializeField] protected bool OnAwake = false;

    //===========================================inner variables=========================================
    protected float timer = 0f;
    protected Vector3 center = Vector3.zero;
    public bool IsEnd = false;

    //===========================================Event Call Back=========================================
    public delegate void ProdEnd();
    public ProdEnd prodEnd = null;

    protected void Awake()
    {
        center = this.transform.position;
    }

    protected void OnEnable()
    {
        if(OnAwake) DoProduction();
    }

    //=========================================Production End Event======================================
    protected void CallProdEnd()
    {
        IsEnd = true;
        prodEnd?.Invoke();
    }

    //============================================Need Implement=========================================
    /// <summary>
    /// Chain or Create Production Logic
    /// </summary>
    public abstract void DoProduction();
    /// <summary>
    /// Chain or Create UndoProduction Logic
    /// </summary>
    public abstract void UndoProduction();
}
