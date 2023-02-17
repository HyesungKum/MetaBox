using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> gameClearEff = null; //for directing fireworks exploding.
    [SerializeField] TextMeshProUGUI clearPlayTime = null; //Time taken to clear the game.
    [SerializeField] Button home = null; //Button to move to start scene
    [SerializeField] bool testMode = false; //for testing animation curves.


    [SerializeField] AnimationCurve ScaleXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(4f, 1f) });
    [SerializeField] AnimationCurve ScaleYCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(4f, 1f) });
    [SerializeField] AnimationCurve PosXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, -5f), new Keyframe(4f, 0f) });
    [SerializeField] AnimationCurve PosYCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 3f), new Keyframe(4f, 0f) });

    Vector3 oriScale = new Vector3(1, 1, 1);
    Vector3 curPos = new Vector3(0, 0, 0);

    void Awake()
    {
        if (testMode == false) home.onClick.AddListener(() => SoundManager.Instance.MusicStart(0));
        if (testMode == false) home.onClick.AddListener(() => SceneManager.LoadScene("Start"));
    }
    

    void Start()
    {
        if(testMode == false) clearPlayTime.text = (GameManager.Instance.PlayTime).ToString();
        for(int i = 0; i < gameClearEff.Count; i++)
        {
            gameClearEff[i].gameObject.SetActive(true);
            gameClearEff[i].Play();
        }

        StartCoroutine(nameof(ClearPanelShow));
    }

    IEnumerator ClearPanelShow()
    {
        float startTime = 0;
        while (ScaleXCurve.keys[ScaleXCurve.keys.Length - 1].time >= startTime)
        {
            this.transform.localScale = new Vector3(oriScale.x * ScaleXCurve.Evaluate(startTime), oriScale.y * ScaleYCurve.Evaluate(startTime), oriScale.z);
            curPos.x = PosXCurve.Evaluate(startTime);
            curPos.y = PosYCurve.Evaluate(startTime);
            this.transform.position = curPos;

            startTime += Time.deltaTime;
            yield return null;
        }
    }
}
