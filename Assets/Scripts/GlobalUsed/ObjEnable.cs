using UnityEngine;

public class ObjEnable : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] bool IsTest = false;
    #endif
    private void Awake()
    {
        if (IsTest) return;

        EventReceiver.unloadScene += DoEnable;
        this.gameObject.SetActive(false);
    }

    void DoEnable()
    {
        this.gameObject.SetActive(true);
    }
}
