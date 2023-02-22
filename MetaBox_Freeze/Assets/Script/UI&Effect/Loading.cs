using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;

    [Header("UI Control")]
    [SerializeField] Slider loadingBar = null;
    [SerializeField] TextMeshProUGUI loadingprogress = null;

    [Header("policecar Control")]
    [SerializeField] Transform policeCar = null;
    [SerializeField] AnimationCurve PoliceCarCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, -10f), new Keyframe(1.8f, 10f) });

    [Header("fade Out Control")]
    [SerializeField] SpriteRenderer focus = null; //fadeout effect of following a police car
    [SerializeField] AnimationCurve FocusCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1.8f, 1f) });

    Vector3 policeCarPos = new Vector3(-10f, -2.16f, 0f);
    Color focusColor = new Color(0, 0, 0, 0);
    bool fade = false; //flag to start fading out when the next scene is ready to load

    public void StartLoading()
    {
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(nameof(Loop));
    }

    IEnumerator Loop()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Freeze");
        asyncLoad.allowSceneActivation = false;
        SoundManager.Instance.PlaySFX(SFX.Siren);

        while (!asyncLoad.allowSceneActivation)
        {
            if (fade)
            {
                loadingBar.value = 1;
                loadingprogress.text = "출동 준비 완료";
                loadingBar.gameObject.SetActive(false);
            }
            
            yield return StartCoroutine(nameof(LoadingShow));
        }
        SoundManager.Instance.StopSFX();
    }

    IEnumerator LoadingShow()
    {
        float startTime = 0;
        while (PoliceCarCurve.keys[PoliceCarCurve.keys.Length - 1].time >= startTime)
        {
            policeCarPos.x = PoliceCarCurve.Evaluate(startTime);
            policeCar.position = policeCarPos;

            if (fade)
            {
                focusColor.a = FocusCurve.Evaluate(startTime);
                focus.color = focusColor;
            }
            else
            {
                loadingBar.value = asyncLoad.progress;
                loadingprogress.text = string.Format("경찰 출동중 {0}%", (int)(asyncLoad.progress * 100));
            }
            
            startTime += Time.deltaTime;
            yield return null;
        }
        if (fade) asyncLoad.allowSceneActivation = true;
        if (asyncLoad.progress >= 0.88) fade = true;
    }
}
