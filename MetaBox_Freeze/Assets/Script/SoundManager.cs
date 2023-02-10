using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.Audio;

public enum SFX
{
    Button = 0,
    Catch = 1,
    Penalty = 2,
    WaveClear = 3,
    WaveFail = 4
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

    public bool BGMMute { get; private set; } = false;
    public bool SFXMute { get; private set; } = false;

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
        AddButtonListener();
    }

    #region Button
    public void AddButtonListener() //�� ���Ŵ������� ȣ��
    {
        GameObject[] rootObj = GetSceneRootObject();

        for (int i = 0; i < rootObj.Length; i++)
        {
            GameObject go = (GameObject)rootObj[i] as GameObject;
            Component[] buttons = go.transform.GetComponentsInChildren(typeof(Button), true);
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => audioSFX.PlayOneShot(scriptableSound.SFX[(int)SFX.Button]));
            }
        }
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

    public void AudioMute(string target, float value)
    {
        audioMixer.GetFloat(target, out float volume);
        volume = volume == -80 ? value : -80;
        if(target.Equals("BGM")) BGMMute = volume == -80 ? true : false;
        else SFXMute = volume == -80 ? true : false;
        audioMixer.SetFloat(target, volume);
    }

    public void MusicStart(int musicindex)
    {
        if (audioBGM.isPlaying) audioBGM.Stop();
        audioBGM.clip = scriptableSound.BGM[0];
        audioBGM.Play();
    }
    public void CatchSFX()
    {
        audioSFX.PlayOneShot(scriptableSound.SFX[(int)SFX.Catch]);
    }
    public void PenaltySFX()
    {
        audioSFX.PlayOneShot(scriptableSound.SFX[(int)SFX.Penalty]);
    }
    public void WaveClearSFX()
    {
        audioSFX.PlayOneShot(scriptableSound.SFX[(int)SFX.WaveClear]);
    }
    public void WaveFailSFX()
    {
        audioSFX.PlayOneShot(scriptableSound.SFX[(int)SFX.WaveFail]);
    }

}
