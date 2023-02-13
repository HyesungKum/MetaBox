using UnityEngine;
using UnityEngine.UI;

public class LineSizeChange : MonoBehaviour
{
    [SerializeField] Button thickLineBut = null;
    [SerializeField] Button thinLineBut = null;

    private float lienSize;
    public float LineSize { get { return lienSize; } set { lienSize = value; } }

    void Awake()
    {
        thickLineBut.onClick.AddListener(delegate { GetThickLineSize(); 
            SoundManager.Inst.ChangeLineAndColorSFXPlay(); SoundManager.Inst.ButtonEffect(thickLineBut.transform.position);
        });

        thinLineBut.onClick.AddListener(delegate { GetThinLineButSize(); 
            SoundManager.Inst.ChangeLineAndColorSFXPlay(); SoundManager.Inst.ButtonEffect(thinLineBut.transform.position);
        });
    }

    float GetThickLineSize()
    {
        float lineThickSize = 0.3f;
        LineSize = lineThickSize;
        return LineSize;
    }

    float GetThinLineButSize()
    {
        float lineThinSize = 0.15f;
        LineSize = lineThinSize;
        return LineSize;
    }
}