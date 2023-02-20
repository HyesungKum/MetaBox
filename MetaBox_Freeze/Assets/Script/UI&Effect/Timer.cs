using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("timer Control")]
    [SerializeField] TextMeshProUGUI timer = null;
    [SerializeField] AnimationCurve RotationCurve; // = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.25f, 8f), new Keyframe(0.5f, -8f), new Keyframe(0.7f, 6f), new Keyframe(0.9f, -6f), new Keyframe(1.05f, 4f), new Keyframe(1.2f, -4f), new Keyframe(1.3f, 2f), new Keyframe(1.4f, -2f), new Keyframe(1.5f, 0f) });


    [Header("alarm Control")]
    [SerializeField] GameObject alarm = null;
    [SerializeField] int hurryUp = 10; //remaining time for the alarm display to start.
    [SerializeField] AnimationCurve AlarmPosCurve; // = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 0.2f), new Keyframe(0.7f, 0f) });
    [SerializeField] AnimationCurve AlarmRotCurve; // = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.1f, 6f), new Keyframe(0.2f, -6f), new Keyframe(0.3f, 4f), new Keyframe(0.4f, -4f), new Keyframe(0.5f, 2f), new Keyframe(0.6f, -2f), new Keyframe(0.7f, 0f) });
    
    
    Vector3 textrotation;
    Vector3 alarmrotation;
    Vector3 alarmOriPos;
    Vector3 alarmCurPos;

    private void Awake()
    {
        GameManager.Instance.playTimerEvent += PlayTimer;
        GameManager.Instance.penaltyEvent += Penalty;
    }
    private void Start()
    {
        alarm.SetActive(false);
        textrotation = new Vector3(0, 0, 0);
    }

    public void PlayTimer()
    {
        timer.text = string.Format("{0:D2} : {1:D2} ", (GameManager.Instance.PlayTime / 60), (GameManager.Instance.PlayTime % 60));
        if (GameManager.Instance.PlayTime == 0)
        {
            timer.color = Color.white;
            alarm.SetActive(false);
        }
        else if (GameManager.Instance.PlayTime <= hurryUp)
        {
            alarm.SetActive(true);
            SoundManager.Instance.PlaySFX(SFX.Alarm);
            timer.color = Color.red;
            StartCoroutine(nameof(AlarmShow));
        }
    }

    IEnumerator AlarmShow()
    {
        float startTime = 0;
        alarmOriPos = alarm.transform.position;
        alarmCurPos = alarm.transform.position;
        alarmrotation.z = 0;
        while (AlarmPosCurve.keys[AlarmPosCurve.keys.Length - 1].time >= startTime)
        {
            alarmrotation.z = AlarmRotCurve.Evaluate(startTime);
            alarm.transform.rotation = Quaternion.Euler(alarmrotation);
            alarmCurPos.y = alarmOriPos.y + AlarmPosCurve.Evaluate(startTime);
            alarm.transform.position = alarmCurPos;

            startTime += Time.deltaTime;
            if (GameManager.Instance.IsGaming == false)
            {
                timer.color = Color.white;
                alarm.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    public void Penalty()
    {
        StartCoroutine(nameof(PenaltyShow));
    }

    IEnumerator PenaltyShow()
    {
        timer.color = Color.red;
        float startTime = 0;
        textrotation.z = 0;
        while (RotationCurve.keys[RotationCurve.keys.Length - 1].time >= startTime)
        {
            textrotation.z = RotationCurve.Evaluate(startTime);
            this.transform.rotation = Quaternion.Euler(textrotation);

            startTime += Time.deltaTime;
            yield return null;
        }
        timer.color = Color.white;
    }

}
