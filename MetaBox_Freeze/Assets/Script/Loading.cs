using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;

    [SerializeField] GameObject policeCar = null;
    [SerializeField] SpriteRenderer focus = null;
    
    [SerializeField] AnimationCurve PoliceCarCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, -10f), new Keyframe(1.8f, 10f) });
    [SerializeField] AnimationCurve FocusCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f) });

    Vector3 policeCarPos = new Vector3(-10f, -2.16f, 0f);
    Color focusColor = new Color(0, 0, 0, 0);
    bool fade = false;

    public void StartLoading()
    {
        StartCoroutine(nameof(Loop));
    }

    IEnumerator Loop()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Freeze");
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.allowSceneActivation)
        {
            yield return StartCoroutine(nameof(LoadingShow));
        }
    }
    IEnumerator LoadingShow()
    {
        float startTime = 0;
        while (PoliceCarCurve.keys[PoliceCarCurve.keys.Length - 1].time >= startTime)
        {
            policeCarPos.x = PoliceCarCurve.Evaluate(startTime);
            policeCar.transform.position = policeCarPos;

            if (fade)
            {
                focusColor.a = FocusCurve.Evaluate(startTime);
                focus.color = focusColor;
            }
            
            startTime += Time.deltaTime;
            yield return null;
        }
        if (fade) asyncLoad.allowSceneActivation = true;
        if (asyncLoad.progress >= 0.85) fade = true; 
    }
}
