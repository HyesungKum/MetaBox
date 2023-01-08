using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ObjectPoolCP;
using Unity.Burst.CompilerServices;

public class DrawLine : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;
    [SerializeField] Canvas canvas = null;

    [SerializeField] RectTransform objOneLinePos = null;
    [SerializeField] RectTransform objTwoLinePos = null;
    [SerializeField] RectTransform objThreeLinePos = null;
    [SerializeField] GameObject clearImg = null;

    public Transform[] objLinePos = new Transform[5];

    public Vector3 startPos;
    public Vector3 isendPos;
    Touch myTouch;

    public Transform endPos;

    DrawLogic drawLogic = null;

    public int objOneFinishCount = 5;
    int objTwoFinishCount = 5;
    int objThreeFinishCount = 9;

    GameObject instLine = null;
    public Transform firstLine = null;

    void Awake()
    {
        drawLogic = gameObject.GetComponent<DrawLogic>();

    }

    void Update()
    {
        if (Input.touchCount <= 0) return; // 입력이 없으면 return

        myTouch = Input.GetTouch(0);
        Vector3 TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(myTouch.position.x, myTouch.position.y, Camera.main.nearClipPlane));

        RaycastHit2D objCheck = RayCheck(TouchPos);


        if (objCheck)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)  // Have Touch
            {
                //Debug.Log("닿은 이름이 뭐니 ?? :" +objCheck.collider.name);
                if (objCheck.collider.name.Equals("DrowPointOne") && objOneFinishCount == 5)
                {

                    //if (objOneFinishCount == 5)
                    //{
                    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, objCheck.transform.GetChild(0));


                    objOneFinishCount -= 1;
                    startPos = Input.GetTouch(0).position;
                    instLine.transform.position = startPos;

                    #region
                    // }
                    //else if (objOneFinishCount < 5 && objOneFinishCount > 0)
                    //{
                    //    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, endPos.GetChild(0));

                    //    objOneFinishCount -= 1;
                    //    startPos = Input.GetTouch(0).position;
                    //    instLine.transform.position = startPos;
                    //}
                    //else if (objOneFinishCount <= 0) 
                    //{
                    //    clesrImg.ClearImgOne();
                    //    return; 
                    //}
                    //Debug.Log("너는 뭐야 : " + objCheck.collider.name == "DrowPoint");

                    //instLine = ObjectPoolCP.PoolCp.Inst.BringObjectCp(linePrefab);
                    //instLine.transform.SetParent(canvas.transform, false);
                    #endregion
                }
                else if (endPos == EndPos(objCheck))
                {
                    Debug.Log("찍히나요 ? ");

                    //Debug.Log("endPos : " + endPos = EndPos(objCheck));
                    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, EndPos(objCheck).GetChild(0));

                    objOneFinishCount -= 1;
                    startPos = Input.GetTouch(0).position;
                    instLine.transform.position = startPos;

                    if(objOneFinishCount <= 0 && firstLine.transform == endPos)
                    {
                        clearImg.GetComponent<Clear>().ClearImgOne();
                            return;
                    }
                }
                
                
                #region
                ////else if (objCheck.collider.name.Equals("DrowPointOne")&& objOneFinishCount < 5 && objOneFinishCount > 0)
                ////{
                ////    Debug.Log("찍히니 ? ");
                ////    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, endPos.GetChild(0));

                ////    objOneFinishCount -= 1;
                ////    startPos = Input.GetTouch(0).position;
                ////    instLine.transform.position = startPos;
                ////}
                //else if (objCheck.collider.name.Equals("DrowPointOne") && objOneFinishCount <= 0)
                //{
                //    clesrImg.ClearImgOne();
                //    return;
                //}
                //else if (objCheck.collider.name.Equals("DrowPointTwo"))
                //{
                //    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, objTwoLinePos);
                //    startPos = Input.GetTouch(0).position;
                //    instLine.transform.position = startPos;
                //}
                //else if (objCheck.collider.name.Equals("DrowPoint"))
                //{
                //    instLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, objThreeLinePos);
                //    startPos = Input.GetTouch(0).position;
                //    instLine.transform.position = startPos;
                //}
                #endregion

            }
            #region
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)  // Touch Move
            {
                //Debug.Log("objCheck.collider.name : " + objCheck.collider.name);

                //Debug.Log("콜라이더 있니 ? : " + objCheck.collider);
                Vector3 myPos = Input.GetTouch(0).position;
                // 별

                if (instLine != null)
                    ThouchMoves(instLine, myPos);

                // 이동하고 종료 시점을 체크 
                // 이동 
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endPos = objCheck.transform;
                Debug.Log("endPos : " + endPos);
                isendPos = objCheck.transform.position;
                Debug.Log("isendPos : " + isendPos);
            }
            #endregion

        }

    }

    Transform EndPos(RaycastHit2D hit)
    {
        Transform ends = hit.transform;

        for (int i = 0; i < objLinePos.Length; i++)
        {
            Transform check = objLinePos[i];


            if (ends == check)
            {
                return check;
                Debug.Log("check : "+ check);
            }
        }
        return null;
    }


    void ThouchMoves(GameObject InstObj, Vector3 myPos)
    {
        InstObj.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1);
        InstObj.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos));
    }

    // RaycastHit
    RaycastHit2D RayCheck(Vector3 touchPos)
    {
        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 5f);
        return hit;
    }

    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}
