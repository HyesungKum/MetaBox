using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.Audio;
using ObjectPoolCP;
using System.Collections;

public enum SFX
{
    Button,
    Siren,
    HideNPC,
    MovePC,
    Catch,
    Penalty,
    Alarm,
    WaveClear,
    GameClear,
    Fail
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(SoundManager), typeof(SoundManager)).GetComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] ScriptableObj scriptableSound = null;

    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] AudioSource audioBGM = null;
    [SerializeField] AudioSource audioSFX = null;

    [SerializeField] GameObject touchEff = null;

    WaitForSeconds playEff = new WaitForSeconds(2f);

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this.gameObject);
    }

    #region Button
    public void AddButtonListener() //각 씬매니저에서 호출
    {
        GameObject[] rootObj = GetSceneRootObject();

        for (int i = 0; i < rootObj.Length; i++)
        {
            GameObject go = (GameObject)rootObj[i] as GameObject;
            Component[] buttons = go.transform.GetComponentsInChildren(typeof(Button), true);
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(delegate { OnClickButton(button.transform.position); });
                button.onClick.AddListener(() => audioSFX.PlayOneShot(scriptableSound.SFX[(int)SFX.Button]));
            }
        }
    }

    void OnClickButton(Vector3 pos)
    {
        StartCoroutine(TouchEff(pos));
    }

    IEnumerator TouchEff(Vector3 movepoint)
    {
        GameObject effect = PoolCp.Inst.BringObjectCp(touchEff);
        effect.transform.position = movepoint;

        yield return playEff;

        if (effect != null) PoolCp.Inst.DestoryObjectCp(effect);
    }

    GameObject[] GetSceneRootObject()
    {
        Scene curscene = SceneManager.GetActiveScene();
        return curscene.GetRootGameObjects();
    }

    #endregion

    public void AudioVolumeControl(string target, float volume)
    {
        if (volume == -40f) audioMixer.SetFloat(target, -80);
        else audioMixer.SetFloat(target, volume);
    }

    public float GetVolume(string target)
    {
        audioMixer.GetFloat(target, out float volume);
        return volume;
    }

    public void MusicStart(int musicindex) //0-title, 1-main
    {
        if (audioBGM.isPlaying) audioBGM.Stop();
        audioBGM.clip = scriptableSound.BGM[musicindex];
        audioBGM.Play();
    }

    public void PlaySFX(SFX mode)
    {
        audioSFX.PlayOneShot(scriptableSound.SFX[(int)mode]);
    }

    public void StopSFX()
    {
        audioSFX.Stop();
    }
}
