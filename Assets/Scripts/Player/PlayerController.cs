using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerController;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    private NavMesh2Agent agent;
    private int charIndex;
    public Animator animator;
    [SerializeField] GameObject[] playerModels;
    [SerializeField] TextMeshProUGUI IdText;

    //플레이어 이동 명령 이벤트
    public delegate void MoveOrder();

    public MoveOrder moveOrder;

    public void CallMoveOrder() => moveOrder?.Invoke();

    Vector3 LookRight;
    Vector3 LookLeft;

    private void Reset()
    {
        mainCam = FindObjectOfType<Camera>();
    }

    private void Awake()
    {
        //init
        charIndex = DataCheckManager.Inst.GetCharIndex();
        IdText.text = DataCheckManager.Inst.GetID();
        playerModels[charIndex].SetActive(true);

        LookRight = playerModels[charIndex].transform.localScale;
        LookLeft = new Vector3(LookRight.x * -1, LookRight.y, LookRight.z);
        this.TryGetComponent(out agent);


        //delegate chain
        TouchEventGenerator.Inst.touchBegan[0] += PlayerMove;
        agent.moveEvent += PlayerMoveAct;
        agent.stopEvent += PlayerStopAct;
    }

    void OnDisable()
    {
        //delegate unchain
        if (TouchEventGenerator.Inst != null)
        {
            TouchEventGenerator.Inst.touchBegan[0] -= PlayerMove;
            agent.moveEvent -= PlayerMoveAct;
            agent.stopEvent -= PlayerStopAct;
        }
    }


    //=================================Player Movement Animator & Sfx===========================================
    void PlayerMoveAct() => StartCoroutine(nameof(MoveRoutine));
    void PlayerStopAct() => StartCoroutine(nameof(StopRoutine));

    IEnumerator MoveRoutine()
    {
        animator.SetBool("IsMove", agent.IsMove);
        SoundManager.Inst.LoopSfx("WalkingSfx");

        while (true)
        {
            if (!agent.IsLookRight) playerModels[charIndex].transform.localScale = LookRight;
            else playerModels[charIndex].transform.localScale = LookLeft;

            yield return null;
        }
    }
    IEnumerator StopRoutine()
    {
        StopCoroutine(nameof(MoveRoutine));

        animator.SetBool("IsMove", agent.IsMove);
        SoundManager.Inst.StopLoopSfx();

        yield return null;  
    }

    //==========================================player position move=============================================
    private void PlayerMove(int id, Vector3 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject(id)) return;

        Vector3 worldPos = mainCam.ScreenToWorldPoint(pos);
        agent.SetDestination(worldPos);
        CallMoveOrder();
    }
}