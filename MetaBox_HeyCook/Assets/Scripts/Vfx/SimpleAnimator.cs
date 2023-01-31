using Microsoft.Cci;
using ObjectPoolCP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleAnimation
{
    [SerializeField] public bool Loop;
    [SerializeField] public Sprite[] frames;
}

public class SimpleAnimator : MonoBehaviour
{
    //====================ref components================
    [Header("ref componetns")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("2D Animation")]
    public new CustomDictionary<string, SimpleAnimation> animation;
    public float fps;

    //=====================routine controll====================
    private SimpleAnimation curAnimation;
    private Coroutine aniCooutine;

    //====================inner varables=================
    private float timer = 0f;
    private float frameTimer = 0f;
    private int frameIndex = 0;

    private void OnEnable()
    {
        timer = 0f;
        frameTimer = 0f;
        frameIndex = 0;

        try
        {
            if (animation.TryGetValue("Idle", out curAnimation))
            {
                aniCooutine = StartCoroutine(nameof(Play));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("##SimpleAnimation Error : Cannot Found \"Idle\" key animation");
        }
    }

    //==================================animation change=================================
    public void ChangeAnimation(string key)
    {
        //find key and changing
        if (animation.TryGetValue(key, out SimpleAnimation tempAni))
        {
            if (curAnimation != tempAni)
            {
                StopCoroutine(aniCooutine);
                curAnimation = tempAni;
                aniCooutine = StartCoroutine(nameof(Play));
            }
            //overlap order ignore
            else return;
        }
        else
        {
            Debug.LogError($"##SimpleAnimation Error : Cannot Found \"{key}\" key animation");
        }
    }

    //==================================play animation===================================
    IEnumerator Play()
    {
        frameIndex = 0;

        while (true)
        {
            timer += Time.unscaledDeltaTime;
            frameTimer += Time.unscaledDeltaTime;

            //image changing
            if ((1f / fps) < frameTimer)
            {
                NextFrame();
                frameTimer = 0f;
            }

            yield return null;
        }
    }
    void NextFrame()
    {
        frameIndex++;

        if (frameIndex > curAnimation.frames.Length-1)
        {
            if (curAnimation.Loop) frameIndex = 0;
            else return;
        }

        spriteRenderer.sprite = curAnimation.frames[frameIndex];
    }
}
