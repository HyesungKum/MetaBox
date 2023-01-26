using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;

    [SerializeField] SpriteRenderer focus = null;
    [SerializeField] GameObject policeCar = null;
    [SerializeField] GameObject thief = null;
    
    [SerializeField] AnimationCurve PoliceCarCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 20f) });
    [SerializeField] AnimationCurve FocusCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f) });
    [SerializeField] AnimationCurve ThiefCurve;

    WaitForSeconds fadeShow = new WaitForSeconds(1f);
    WaitForSeconds wait = new WaitForSeconds(2f);

    Vector3 policeCarOriPos = new Vector3(-10f, -3.62f, 0f);
    Vector3 policeCarCurPos;
    Color focusColor = new Color(0, 0, 0, 0);

    void Start()
    {
        focus.color = focusColor;
        policeCar.transform.position = policeCarOriPos;
        StartCoroutine(nameof(Loop));
    }

    IEnumerator Loop()
    {
        yield return fadeShow;
        policeCarCurPos = policeCar.transform.position;
        asyncLoad = SceneManager.LoadSceneAsync("Freeze");
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            StartCoroutine(nameof(LoadingShow));
            yield return wait;
        }
    }
    IEnumerator LoadingShow()
    {
        float startTime = 0;
        while (PoliceCarCurve.keys[PoliceCarCurve.keys.Length - 1].time >= startTime)
        {
            policeCarCurPos.x = policeCarOriPos.x + PoliceCarCurve.Evaluate(startTime);
            policeCar.transform.position = policeCarCurPos;

            focusColor.a = FocusCurve.Evaluate(startTime);
            focus.color = focusColor;

            startTime += Time.deltaTime;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

}
