using UnityEngine;

public class ObjEnable : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] bool IsTest = false;
    #endif
    private void Awake()
    {
        #if UNITY_EDITOR
        if (IsTest) return;
        #endif

        EventReceiver.unloadScene += DoEnable;
        this.gameObject.SetActive(false);
    }

    void DoEnable()
    {
        this.gameObject.SetActive(true);
    }
}
