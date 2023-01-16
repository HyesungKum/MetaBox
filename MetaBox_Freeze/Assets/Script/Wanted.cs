using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wanted : MonoBehaviour
{
    [SerializeField] GameObject arrest = null;
    [SerializeField] Image image = null;
    [SerializeField] ScriptableObj wantedImages = null;

    // Start is called before the first frame update
    void Start()
    {
        if (arrest == null) arrest = this.transform.GetChild(1).gameObject;
        if (image == null) image = this.transform.GetChild(0).GetComponent<Image>();
        arrest.SetActive(false);
    }

    public void Init(int id)
    {
        image.sprite = wantedImages.Thief[id];
    }

    public void Arrest()
    {
        arrest.SetActive(true);
    }
}
