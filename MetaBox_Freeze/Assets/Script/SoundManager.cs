using UnityEngine;
using UnityEngine.Audio;

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

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Application.targetFrameRate = 60;

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        DontDestroyOnLoad(this.gameObject);
    }

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

    public void PlayBGM(int musicindex) //0-title, 1-main
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
