using System.Collections;
using UnityEngine;

public class LerpMoveProduction : Production
{
    [SerializeField] Transform targetTr;
    private float targetRoundX { get; set; }
    private float targetRoundY { get; set; }

    new void Awake()
    {
        //TablePosition Round
        targetRoundX = Mathf.Round(targetTr.position.x);
        targetRoundY = Mathf.Round(targetTr.position.y);
    }

    public override void DoProduction() => StartCoroutine(nameof(ProdRoutine));
    public override void UndoProduction() => StartCoroutine(nameof(UndoProdRoutine));
    
    IEnumerator ProdRoutine()
    {
        while (this.transform.position != targetTr.position)
        {
            float fixedX = Mathf.Round(this.transform.position.x);
            float fixedY = Mathf.Round(this.transform.position.y);

            this.transform.position = Vector3.Lerp(this.transform.position, targetTr.position, Time.deltaTime * prodSpeed);

            if (targetRoundX == fixedX && targetRoundY == fixedY)
            {
                this.transform.position = targetTr.position;
            }

            yield return null;
        }

        CallProdEnd();

        yield return null;
    }
    IEnumerator UndoProdRoutine()
    {
        yield return null;
    }

}
