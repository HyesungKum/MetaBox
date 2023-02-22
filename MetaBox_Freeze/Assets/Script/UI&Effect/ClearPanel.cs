using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> gameClearEff = null; //for directing fireworks exploding.
    [SerializeField] TextMeshProUGUI clearPlayTime = null; //Time taken to clear the game.

    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f) });

    Vector3 oriScale = new Vector3(1, 1, 1);

    void Start()
    {
        StartCoroutine(nameof(PanelShow));

        if (clearPlayTime == null) return;

        clearPlayTime.text = string.Format("{0:D2} : {1:D2} ", (GameManager.Instance.PlayTime / 60), (GameManager.Instance.PlayTime % 60));
        for (int i = 0; i < gameClearEff.Count; i++)
        {
            gameClearEff[i].gameObject.SetActive(true);
            gameClearEff[i].Play();
        }
    }

    IEnumerator PanelShow()
    {
        float startTime = 0;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            this.transform.localScale = oriScale * ScaleCurve.Evaluate(startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
    }
}
