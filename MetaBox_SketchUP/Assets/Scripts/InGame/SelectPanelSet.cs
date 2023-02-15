using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate int LevelIndes(int index);

public class SelectPanelSet : MonoBehaviour
{
    [Header("[Select Panel]")]
    [SerializeField] public Button oneBrushBut = null;
    [SerializeField] public Button twoBrushBut = null;
    [SerializeField] public Button threeBrushBut = null;

    [Header("[Character Move]")]
    [SerializeField] public GameObject character = null;

    [Header("[Select Image Data]")]
    [SerializeField] SelectPanelImgData selectPanelImg = null;
    [SerializeField] LevelPanelSet startSceneLevelIndex;
    [SerializeField] AnimalQPrefabData animalQPrefabData = null;

    public Vector2 characterPos;
    Image selectImage = null;
    GameObject instQprefab = null;

    int levelIndexCheck;

    void Awake()
    {
        if (selectPanelImg == null)
            selectPanelImg = Resources.Load<SelectPanelImgData>("Data/SelectPanelImgData");
        if (animalQPrefabData == null)
            animalQPrefabData = Resources.Load<AnimalQPrefabData>("Data/AnimalQPrefabData");

        levelIndexCheck = SoundManager.Inst.LevelIndex;
        //Debug.Log("CharacterPos : " + character.transform.localPosition);
        characterPos = new Vector2(915f, 400f);
        character.transform.localPosition = characterPos;

        ImgeSet();

        #region Button Click Event Setting
        oneBrushBut.onClick.AddListener(delegate
        {
            OnClickQustion(oneBrushBut, true, false, false);
            CharacterMoves(1);
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(oneBrushBut.transform.position);
        });

        twoBrushBut.onClick.AddListener(delegate
        {
            OnClickQustion(twoBrushBut, false, true, false);
            CharacterMoves(2);
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(twoBrushBut.transform.position);
        });

        threeBrushBut.onClick.AddListener(delegate
        {
            OnClickQustion(threeBrushBut, false, false, true);
            CharacterMoves(3);
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(threeBrushBut.transform.position);
        });
        #endregion 
    }

    void ImgeSet()
    {
        if (levelIndexCheck == 1)
        {
            SelecetImgOne(oneBrushBut, selectPanelImg.LevelOneSelectPanelImg, 0);
            InstQPrefab(0,InGamePanelSet.Inst.QOneObj());
            SelecetImgOne(twoBrushBut, selectPanelImg.LevelOneSelectPanelImg, 1);
            InstQPrefab(1, InGamePanelSet.Inst.QTwoObj());
            SelecetImgOne(threeBrushBut, selectPanelImg.LevelOneSelectPanelImg, 2);
            InstQPrefab(2, InGamePanelSet.Inst.QThreeObj());
        }
        else if (levelIndexCheck == 2)
        {
            SelecetImgOne(oneBrushBut, selectPanelImg.LevelTwoSelectPanelImg, 0);
            InstQPrefab(3, InGamePanelSet.Inst.QOneObj());

            SelecetImgOne(twoBrushBut, selectPanelImg.LevelTwoSelectPanelImg, 1);
            InstQPrefab(4, InGamePanelSet.Inst.QTwoObj());

            SelecetImgOne(threeBrushBut, selectPanelImg.LevelTwoSelectPanelImg, 2);
            InstQPrefab(5, InGamePanelSet.Inst.QThreeObj());
        }
        else if (levelIndexCheck == 3)
        {
            SelecetImgOne(oneBrushBut, selectPanelImg.LevelThreeSelectPanelImg, 0);
            InstQPrefab(6, InGamePanelSet.Inst.QOneObj());

            SelecetImgOne(twoBrushBut, selectPanelImg.LevelThreeSelectPanelImg, 1);
            InstQPrefab(7, InGamePanelSet.Inst.QTwoObj());

            SelecetImgOne(threeBrushBut, selectPanelImg.LevelThreeSelectPanelImg, 2);
            InstQPrefab(8, InGamePanelSet.Inst.QThreeObj());
        }
        else if (levelIndexCheck == 4)
        {
            SelecetImgOne(oneBrushBut, selectPanelImg.LevelFourSelectPanelImg, 0);
            InstQPrefab(9, InGamePanelSet.Inst.QOneObj());

            SelecetImgOne(twoBrushBut, selectPanelImg.LevelFourSelectPanelImg, 1);
            InstQPrefab(10, InGamePanelSet.Inst.QTwoObj());

            SelecetImgOne(threeBrushBut, selectPanelImg.LevelFourSelectPanelImg, 2);
            InstQPrefab(11, InGamePanelSet.Inst.QThreeObj());
        }
    }

    void InstQPrefab(int index, GameObject setParent)
    {
        instQprefab = ObjectPoolCP.PoolCp.Inst.BringObjectCp(animalQPrefabData.Qprefab[index]);
        instQprefab.transform.SetParent(setParent.transform);
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
        //button.gameObject.SetActive(butSet);
    }

    public void PlayGameObjSet(bool objOneSet, bool objTwoSet, bool objThreeSet)
    {
        InGamePanelSet.Inst.InGameSet(true);
        InGamePanelSet.Inst.LineColorAndSizeChange(true);

        InGamePanelSet.Inst.QOneSet(objOneSet);
        InGamePanelSet.Inst.QTwoSet(objTwoSet);
        InGamePanelSet.Inst.QThreeSet(objThreeSet);
    }
}