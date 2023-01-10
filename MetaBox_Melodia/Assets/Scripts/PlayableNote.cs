using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class PlayableNote : MonoBehaviour
{
    public delegate void DelegatePlayableNote(GameObject myPos);
    public static DelegatePlayableNote myDelegatePlayableNote;



    [SerializeField] protected PitchName myPitchName;
    public PitchName MyPitchName { get { return myPitchName; } set { myPitchName = value; } }

    bool isMoving = false;
    float movingSpeed;

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
                isMoving = false;
            }
        }
    }


    public void MoveNote(Vector3 target, float speed)
    {
        movingSpeed = speed;
        isMoving = true;
        targetPos = target;
    }

    public void DestroyNote()
    {
        myInventory.DestoyedPlayableNote(this.gameObject);
    }


    public void UseNote()
    {
        myInventory.UseNote(this.gameObject);
    }

}
