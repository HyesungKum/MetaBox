using System.Collections;
using UnityEngine;

public class ScrollingMap : MonoBehaviour
{
    //==============================camera referance===========================
    [SerializeField] Camera mainCam;

    //===============================Settings==================================
    [Header("Scoll Settings")]
    [SerializeField] SpriteRenderer backRenderer;
    [SerializeField] Transform[] background;
    [SerializeField] float scrollSpeed;

    [Header("[Rock Setting]")]
    [SerializeField] SpriteRenderer rockRenderer;
    [SerializeField] Transform[] Rock;
    [SerializeField] Transform[] RockSpawnTr;
    [SerializeField] float rockSpeed;

    //=============================inner variables==============================
    BoxCollider2D[] rockColl;

    float leftLimit;
    float rightLimit;
    Vector3 newBackPos;
    Vector3[] newRockPos = new Vector3[2];

    private void Awake()
    {
        //initailizing
        rockColl = new BoxCollider2D[Rock.Length];

        for (int i = 0; i < Rock.Length; i++)
        {
            Rock[i].TryGetComponent(out rockColl[i]);
        }

        leftLimit = mainCam.ViewportToWorldPoint(new Vector3(-0.5f, 0f)).x;
        rightLimit = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0f)).x;
        newBackPos = new Vector3(backRenderer.bounds.size.x * 3f, 0f);
        newRockPos[0] = RockSpawnTr[0].position;
        newRockPos[1] = RockSpawnTr[1].position;

        //delegate chain
        EventReciver.PlayerFall += StopScroll;
        EventReciver.PlayerRise += StartScroll;

        //scroll routine
        StartScroll();
    }

    private void OnDisable()
    {
        //delegate unchain
        EventReciver.PlayerFall -= StopScroll;
        EventReciver.PlayerRise -= StartScroll;
    }

    //===========================Scrolling Controll==============================
    private void StartScroll()
    {
        StartCoroutine(nameof(Scrolling));
    }
    private void StopScroll()
    {
        StopCoroutine(nameof(Scrolling));
    }

    //==========================map image scrolling==============================
    IEnumerator Scrolling()
    {
        while (true)
        {
            //background scrolling
            for (int i = 0; i < background.Length; i++)
            {
                background[i].Translate(scrollSpeed * Time.deltaTime * Vector3.left);

                if (background[i].position.x < leftLimit) background[i].position += (newBackPos + Vector3.left * 0.01f);
            }

            //rock scrolling
            for(int i =0; i< Rock.Length; i++)
            {
                Rock[i].Translate(rockSpeed * Time.deltaTime * Vector3.left);
                if (Rock[i].position.x < -11f)
                {
                    rockColl[i].enabled = true;
                    Rock[i].position = newRockPos[Random.Range(0, 2)];
                }
            }
            
            yield return null;
        }
    }
}
