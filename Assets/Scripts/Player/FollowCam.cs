using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] Transform playerTrans;

    WaitForSeconds waitSec = new(0.2f);

    private void Reset()
    {
        controller = FindObjectOfType<PlayerController>();
        playerTrans = controller.transform;
    }

    private void Awake()
    {
        controller.moveOrder += FollowOrder;
    }

    private void OnDisable()
    {
        controller.moveOrder -= FollowOrder;
    }

    private void FollowOrder()
    {
        StartCoroutine(nameof(Follow));
    }

    IEnumerator Follow()
    {
        yield return waitSec;
        while (true)
        {
            yield return null;
            Vector3 fixedPos = new(playerTrans.transform.position.x,  playerTrans.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, fixedPos, Time.deltaTime * 1f);

            if (Vector3.Distance(this.transform.position, fixedPos) < 0.2f) yield break;
        }
    }
}
