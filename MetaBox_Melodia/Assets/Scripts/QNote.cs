using TMPro;
using UnityEngine;

public class QNote : MonoBehaviour
{

    [SerializeField] int myPitchNum;
    public int MyPitchNum { get { return myPitchNum; } set { myPitchNum = value; } }

    [SerializeField] GameObject hightlightFX = null;

    [SerializeField] TextMeshProUGUI textPitch = null;

    private void OnEnable()
    {
        if (hightlightFX == null) return;
        textPitch.color = Color.gray;
        hightlightFX.SetActive(true);
    }

    public void Setting()
    {
        if (hightlightFX != null) hightlightFX.transform.position = new Vector2(this.transform.position.x, -1.25f);

        textPitch.transform.position = new Vector2(this.transform.position.x, 4.5f);
        switch (myPitchNum)
        {
            case 100:
            case 107:
                textPitch.text = "��";
                break;
            case 101:
                textPitch.text = "��";
                break;
            case 102:
                textPitch.text = "��";
                break;
            case 103:
                textPitch.text = "��";
                break;
            case 104:
                textPitch.text = "��";
                break;
            case 105:
                textPitch.text = "��";
                break;
            case 106:
                textPitch.text = "��";
                break;
        }
    }

    public void Correct()
    {
        textPitch.color = Color.white;
        hightlightFX.SetActive(false);
    }
}
