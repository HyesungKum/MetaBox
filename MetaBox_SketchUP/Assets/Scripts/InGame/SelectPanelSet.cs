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

    [Header("[Select Image Data]")]
    [SerializeField] SelectPanelImgData selectPanelImg = null;
    //[SerializeField] LevelPanelSet startSceneLevelIndex;
    Vector2 characterPos;
    Image selectImage = null;

    int levelIndex;

    void Awake()
    {
        if (selectPanelImg == null)
            selectPanelImg = Resources.Load<SelectPanelImgData>("Data/SelectPanelImgData");

        //TryGetComponent<LevelPanelSet>(out startSceneLevelIndex);
        //levelIndex = startSceneLevelIndex.LevelIndex;
        //Debug.Log("levelIndex : " + levelIndex);


        SelecetImgOne(oneBrush, selectPanelImg.LevelOneSelectPanelImg, 0);
        SelecetImgOne(twoBrush, selectPanelImg.LevelOneSelectPanelImg, 1);
        SelecetImgOne(threeBrush, selectPanelImg.LevelOneSelectPanelImg, 2);


        oneBrush.onClick.AddListener(delegate
        {
            OnClickQustion(oneBrush, true, false, false);
            CharacterMoves(1);
            SoundManager.Inst.ButtonSFXPlay();
        });

        twoBrush.onClick.AddListener(delegate
        {
            OnClickQustion(twoBrush, false, true, false);
            CharacterMoves(2);
            SoundManager.Inst.ButtonSFXPlay();
        });

        threeBrush.onClick.AddListener(delegate
        {
            OnClickQustion(threeBrush, false, false, true);
            CharacterMoves(3);
            SoundManager.Inst.ButtonSFXPlay();
        });

        CloseSelectPanelBut.onClick.AddListener(delegate
        {
            OnClickInGameGoSelectPanel();
            SoundManager.Inst.ButtonSFXPlay();
        });
    }

    void SelecetImgOne(Button butNum, Sprite[] SelectPanelImgData, int spriteIndex)
    {
        butNum.TryGetComponent<Image>(out selectImage);
        selectImage.sprite = SelectPanelImgData[spriteIndex];
    }

    void OnClickQustion(Button butNum, bool active, bool twoObjactive, bool threeObjactive)
    {
        PanelChangedSet(butNum, false);
        PlayGameObjSet(active, twoObjactive, threeObjactive);
        InGamePanelSet.Inst.LineColorAndSizeChange(true);

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
        else if (stage == 3)
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