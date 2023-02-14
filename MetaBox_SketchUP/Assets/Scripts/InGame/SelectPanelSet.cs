using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate int LevelIndes(int index);

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
    [SerializeField] LevelPanelSet startSceneLevelIndex;
    [SerializeField] AnimalQPrefabData animalQPrefabData = null;

    Vector2 characterPos;
    Image selectImage = null;
    GameObject instQprefab = null;

    Dictionary<int, GameObject> qAnimalPrefabDictionary;

    int levelIndexCheck;
    void Awake()
    {
        if (selectPanelImg == null)
            selectPanelImg = Resources.Load<SelectPanelImgData>("Data/SelectPanelImgData");
        if (animalQPrefabData == null)
            animalQPrefabData = Resources.Load<AnimalQPrefabData>("Data/AnimalQPrefabData");

        qAnimalPrefabDictionary = new Dictionary<int, GameObject>();


        levelIndexCheck = SoundManager.Inst.LevelIndex;
        ImgeSet();

        #region Button Click Event Setting
        oneBrush.onClick.AddListener(delegate
        {
            OnClickQustion(oneBrush, true, false, false);
            CharacterMoves(1);
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(oneBrush.transform.position);
        });

        twoBrush.onClick.AddListener(delegate
        {
            OnClickQustion(twoBrush, false, true, false);
            CharacterMoves(2);
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(twoBrush.transform.position);
        });

        threeBrush.onClick.AddListener(delegate
        {
            OnClickQustion(threeBrush, false, false, true);
            CharacterMoves(3);
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(threeBrush.transform.position);
        });

        CloseSelectPanelBut.onClick.AddListener(delegate
        {
            OnClickInGameGoSelectPanel();
            SoundManager.Inst.ButtonSFXPlay(); SoundManager.Inst.ButtonEffect(CloseSelectPanelBut.transform.position);
        });
        #endregion 
    }

    void AddDictionaryPrefab()
    {
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            qAnimalPrefabDictionary.Add(count, animalQPrefabData.Qprefab[i]);
        }
    }

    void ImgeSet()
    {
        //Debug.Log("animalQPrefabData : " + animalQPrefabData.Qprefab.Length);
        if (levelIndexCheck == 1)
        {
            SelecetImgOne(oneBrush, selectPanelImg.LevelOneSelectPanelImg, 0);
            InstQPrefab(0,objOne);
            SelecetImgOne(twoBrush, selectPanelImg.LevelOneSelectPanelImg, 1);
            InstQPrefab(1, objTwo);
            SelecetImgOne(threeBrush, selectPanelImg.LevelOneSelectPanelImg, 2);
            InstQPrefab(2, objThree);
        }
        else if (levelIndexCheck == 2)
        {
            SelecetImgOne(oneBrush, selectPanelImg.LevelTwoSelectPanelImg, 0);
            InstQPrefab(3, objOne);
            SelecetImgOne(twoBrush, selectPanelImg.LevelTwoSelectPanelImg, 1);
            InstQPrefab(4, objTwo);
            SelecetImgOne(threeBrush, selectPanelImg.LevelTwoSelectPanelImg, 2);
            InstQPrefab(5, objThree);
        }
        else if (levelIndexCheck == 3)
        {
            SelecetImgOne(oneBrush, selectPanelImg.LevelThreeSelectPanelImg, 0);
            InstQPrefab(6, objOne);
            SelecetImgOne(twoBrush, selectPanelImg.LevelThreeSelectPanelImg, 1);
            InstQPrefab(7, objTwo);
            SelecetImgOne(threeBrush, selectPanelImg.LevelThreeSelectPanelImg, 2);
            InstQPrefab(8, objThree);
        }
        else if (levelIndexCheck == 4)
        {
            SelecetImgOne(oneBrush, selectPanelImg.LevelFourSelectPanelImg, 0);
            InstQPrefab(9, objOne);
            SelecetImgOne(twoBrush, selectPanelImg.LevelFourSelectPanelImg, 1);
            InstQPrefab(10, objTwo);
            SelecetImgOne(threeBrush, selectPanelImg.LevelFourSelectPanelImg, 2);
            InstQPrefab(11, objThree);
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
        //InGamePanelSet.Inst.LineColorAndSizeChange(false);
        ButAllSet();
    }
}