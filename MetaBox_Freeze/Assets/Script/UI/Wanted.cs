using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Wanted : MonoBehaviour
{
    [SerializeField] GameObject arrest = null;
    [SerializeField] RectTransform jail = null;
    [SerializeField] Image thief = null;
    [SerializeField] ScriptableObj wantedImages = null;

    void Awake()
    {
        if (arrest == null) arrest = this.transform.GetChild(1).gameObject;
        if (thief == null) thief = this.transform.GetChild(0).GetComponent<Image>();
        arrest.SetActive(false);
    }

    public void Init(int id)
    {
        thief.sprite = wantedImages.Thief[id];
    }

    public void Catch()
    {
        jail.gameObject.SetActive(true);
        StartCoroutine(nameof(JailShow));
    }

    IEnumerator JailShow()
    {
        float time = 0f;
        Vector3 oriScaleY = new Vector3(1, 0, 1);

        while(time < 2.5f)
        {
            time += Time.deltaTime;
            jail.localScale =  Vector3.Lerp(oriScaleY, Vector3.one, time / 2.5f);
            yield return null;
        }
        arrest.SetActive(true);
    }

}
