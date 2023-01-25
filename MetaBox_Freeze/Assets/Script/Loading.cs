using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;

    [SerializeField] SpriteRenderer fade = null;
    [SerializeField] GameObject policeCar = null;
    [SerializeField] GameObject thief = null;
    [SerializeField] AnimationCurve FadeCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f) });
    [SerializeField] AnimationCurve PoliceCarCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 20f) });
    [SerializeField] AnimationCurve ThiefCurve;

    WaitForSeconds wait = new WaitForSeconds(2f);

    Color fadeColor = Color.black;
    Vector3 policeCarOriPos;
    Vector3 policeCarCurPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Loop));
    }
    
    IEnumerator Loop()
    {
        policeCarOriPos = policeCar.transform.position;
        policeCarCurPos = policeCar.transform.position;
        asyncLoad = SceneManager.LoadSceneAsync("Freeze");
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            StartCoroutine(nameof(LoadingShow));
            yield return wait;
            asyncLoad.allowSceneActivation = true;
        }
    }
    IEnumerator LoadingShow()
    {
        float startTime = 0;
        while (PoliceCarCurve.keys[PoliceCarCurve.keys.Length - 1].time >= startTime)
        {
            policeCarCurPos.x = policeCarOriPos.x + PoliceCarCurve.Evaluate(startTime);
            policeCar.transform.position = policeCarCurPos;

            fadeColor.a = FadeCurve.Evaluate(startTime);
            fade.color = fadeColor;

            startTime += Time.deltaTime;
            yield return null;
        }
    }

}
