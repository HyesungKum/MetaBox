using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelSet : MonoBehaviour
{
    [Header("[Select Panel]")]
    [SerializeField] Button oneBrush = null;
    [SerializeField] Button twoBrush = null;
    [SerializeField] Button threeBrush = null;
    [SerializeField] Button CloseSelectPanelBut = null;

    [Header("[Character Move]")]
    [SerializeField] GameObject character = null;

    [Header("[InGame obj Set]")]
    [SerializeField] GameObject objOne = null;
    [SerializeField] GameObject objTwo = null;
    [SerializeField] GameObject objThree = null;

    Vector2 characterPos;

    void Awake()
    {
        oneBrush.onClick.AddListener(delegate
        {
            OnClickQustion(oneBrush, true, false, false); CharacterMoves(1);
            SoundManager.Inst.ButtonSFXPlay();
        });

        twoBrush.onClick.AddListener(delegate
        {
            OnClickQustion(twoBrush, false, true, false); CharacterMoves(2);
            SoundManager.Inst.ButtonSFXPlay();
        });

        threeBrush.onClick.AddListener(delegate
        {
            OnClickQustion(threeBrush, false, false, true); CharacterMoves(3);
            SoundManager.Inst.ButtonSFXPlay();
        });

        CloseSelectPanelBut.onClick.AddListener(delegate
        {
            OnClickInGameGoSelectPanel();
            SoundManager.Inst.ButtonSFXPlay();
        });
    }

    void OnClickQustion(Button butNum, bool active, bool twoObjactive, bool threeObjactive)
    {
        PanelChangedSet(butNum, false);
        PlayGameObjSet(active, twoObjactive, threeObjactive);
    }



    public void CharacterMoves(int stage)
    {
        characterPos = character.transform.localPosition;
        Vector2 movePos = characterPos;

        if (stage == 1)
        {
            movePos = characterPos;
            character.transform.localPosition = movePos;
        }
        else if (stage == 2)
        {
            movePos = new Vector2(movePos.x, 200);
            character.transform.localPosition = movePos;
        }
        else if(stage == 3)
        {
            movePos = new Vector2(movePos.x, 0);
            character.transform.localPosition = movePos;
        }
    }

    public void PanelChangedSet(Button button, bool butSet)
    {
        this.gameObject.SetActive(false);
        InGamePanelSet.Inst.OneBrushPlayPanelSet(true);
        button.gameObject.SetActive(butSet);
    }

    public void PlayGameObjSet(bool objOneSet, bool objTwoSet, bool objThreeSet)
    {
        InGamePanelSet.Inst.InGameSet(true);
        InGamePanelSet.Inst.LineColorAndSizeChange(true);
        objOne.gameObject.SetActive(objOneSet);
        objTwo.gameObject.SetActive(objTwoSet);
        objThree.gameObject.SetActive(objThreeSet);
    }

    void ButAllSet()
    {
        oneBrush.gameObject.SetActive(true);
        twoBrush.gameObject.SetActive(true);
        threeBrush.gameObject.SetActive(true);
    }

    void OnClickInGameGoSelectPanel()
    {
        InGamePanelSet.Inst.SelectPanelSet(true);
        character.transform.localPosition = characterPos;
        InGamePanelSet.Inst.OneBrushPlayPanelSet(false);
        InGamePanelSet.Inst.InGameSet(false);
        InGamePanelSet.Inst.LineColorAndSizeChange(false);
        ButAllSet();
    }
}