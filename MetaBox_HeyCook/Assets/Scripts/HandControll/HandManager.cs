using ObjectPoolCP;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    //==================================Reference Object============================
    [Header("Reference Main Camera")]
    [SerializeField] Camera mainCam;

    [Header("Touch Vfx Prefabs")]
    [SerializeField] GameObject TouchVfx;

    [Header("Current Handling Ingred")]
    [SerializeField] bool TitleScene;
    [SerializeField] Ingredient[] touchedIngred;

    private void Awake()
    {
        touchedIngred = new Ingredient[10];

        //touch chain
        for (int i = 0; i < TouchEventGenerator.Inst.touchBegan.Length; i++)
        {
            TouchEventGenerator.Inst.touchBegan[i] += OnTouchedScreen;

            if (TitleScene) continue;

            //touched
            TouchEventGenerator.Inst.touchBegan[i] += OnTouchedIngred;
            TouchEventGenerator.Inst.touchBegan[i] += OnTouchedTable;

            //pressing
            TouchEventGenerator.Inst.touchStationary[i] += OnPressTable;

            //moved
            TouchEventGenerator.Inst.touchMoved[i] += OnMovedIngred;
            TouchEventGenerator.Inst.touchMoved[i] += OnSlicingTable;
            
            //ended
            TouchEventGenerator.Inst.touchEnded[i] += OnEndedIngred;

            //cancled
            TouchEventGenerator.Inst.touchCancled[i] += OnEndedIngred;
        }
    }
    private void OnDisable()
    {
        //application quit exception
        if (TouchEventGenerator.Inst == null) return;

        //touch unchain
        for (int i = 0; i < TouchEventGenerator.Inst.touchBegan.Length; i++)
        {
            TouchEventGenerator.Inst.touchBegan[i] -= OnTouchedScreen;

            if (TitleScene) continue;

            //touched
            TouchEventGenerator.Inst.touchBegan[i] -= OnTouchedIngred;
            TouchEventGenerator.Inst.touchBegan[i] -= OnTouchedTable;

            //pressing
            TouchEventGenerator.Inst.touchStationary[i] -= OnPressTable;

            //moved
            TouchEventGenerator.Inst.touchMoved[i] -= OnMovedIngred;
            TouchEventGenerator.Inst.touchMoved[i] -= OnSlicingTable;

            //ended
            TouchEventGenerator.Inst.touchEnded[i] -= OnEndedIngred;

            //cancled
            TouchEventGenerator.Inst.touchCancled[i] -= OnEndedIngred;
        }
    }

    //=============================================Vfx Controll========================================
    private void OnTouchedScreen(int index, Vector3 pos)
    {
        Vector3 fixedPos = mainCam.ScreenToWorldPoint(pos) + Vector3.forward;

        GameObject instVfx = PoolCp.Inst.BringObjectCp(TouchVfx);
        instVfx.transform.position = fixedPos;
    }

    //==========================================Ingredient Controll=====================================
    private void OnTouchedIngred(int index, Vector3 pos)
    {
        if (GameManager.Inst.IsGameOver || GameManager.Inst.IsPause) return;

        Ray ray = mainCam.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);

        if (!hit || !hit.transform.CompareTag(nameof(Ingredient))) return;

        Ingredient ingred = hit.transform.GetComponent<Ingredient>();

        touchedIngred[index] = ingred;
        touchedIngred[index].IsCliked = true;
    }
    private void OnMovedIngred(int index, Vector3 pos)
    {
        if (GameManager.Inst.IsGameOver || GameManager.Inst.IsPause) return;

        if (touchedIngred[index] == null) return;

        Vector3 movePos = mainCam.ScreenToWorldPoint(pos);
        touchedIngred[index].transform.position = movePos + Vector3.forward * 2f;
    }
    private void OnEndedIngred(int index, Vector3 pos)
    {
        if (touchedIngred[index] == null) return;

        touchedIngred[index].IsCliked = false;
        touchedIngred[index] = null;
    }

    //========================================Table Controll======================================
    private void OnTouchedTable(int index, Vector3 pos)
    {
        Ray ray = mainCam.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);

        if (!hit || !hit.transform.CompareTag("Table")) return;

        hit.transform.SendMessage(nameof(TrimType.Touching), SendMessageOptions.DontRequireReceiver);
    }
    private void OnSlicingTable(int index, Vector3 pos)
    {
        Ray ray = mainCam.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);

        if (!hit || !hit.transform.CompareTag("Table")) return;

        hit.transform.SendMessage(nameof(TrimType.Slicing), SendMessageOptions.DontRequireReceiver);
    }
    private void OnPressTable(int index, Vector3 pos)
    {
        Ray ray = mainCam.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);

        if (!hit || !hit.transform.CompareTag("Table")) return;

        hit.transform.SendMessage(nameof(TrimType.Pressing), SendMessageOptions.DontRequireReceiver);
    }
}
