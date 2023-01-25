using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class PlayableNote : MonoBehaviour
{
    [SerializeField] protected PitchName myPitchName;
    public PitchName MyPitchName { get { return myPitchName; } set { myPitchName = value; } }

    bool isMoving = false;
    float movingSpeed;

    Vector2 originPos;
    Vector2 targetPos;

    Inventory myInventory;



    private void Awake()
    {
        myInventory = this.GetComponentInParent<Inventory>();
    }


    private void Update()
    {
        if (isMoving)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, Time.deltaTime * movingSpeed);

            if (Vector2.Distance(this.transform.position, targetPos) <= 0f)
            {
                Debug.Log("µµÂø!");

                this.GetComponent<Collider2D>().enabled = false;

                isMoving = false;
            }
        }


    }





    // raycast where touched 
    Collider2D[] shootCircleRay(Vector3 targetT)
    {
        Ray2D ray;
        ray = new Ray2D(targetT, Vector2.zero);

        Collider2D[] hits = Physics2D.OverlapCircleAll(targetT, 0.5f);

        return hits;
    }



    public void Landed()
    {
        Collider2D[] hits = shootCircleRay(this.transform.position);


        if (hits.Length > 0)
        {
            for (int i = 1; i < hits.Length; ++i)
            {
                Debug.Log("Touched somthing!!" + hits[i].name);


                if (hits[i].name == "QNote")
                {
                    movingSpeed = 3f;
                    targetPos = hits[i].transform.position;

                    hits[i].gameObject.SetActive(false);

                    this.transform.parent = null;

                    isMoving = true;

                    Debug.Log($"Touched QNote!");
                    return;
                }
            }


            for (int i = 1; i < hits.Length; ++i)
            {
                if (hits[i].name == "SheetMusic")
                {
                    Debug.Log("Touched SheetMusic!");
                    Invoke("goodbye", 0.25f);
                    return;
                }
            }
        }

        Debug.Log("touched nothing");

        isMoving = true;
        this.transform.position = originPos;
    }


    void goodbye()
    {
        myInventory.DestoyedPlayableNote(this.gameObject);
        ObjectPoolCP.PoolCp.Inst.DestoryObjectCp(this.gameObject);
    }

    public void SetOriginPos()
    {
        originPos = transform.position;
    
    }

}
