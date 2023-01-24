using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager instance;
    public static AudioManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject(nameof(AudioManager), typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    #endregion

    [Header("Audio Source")]
    [SerializeField] public AudioSource bgmSource = null;
    [SerializeField] public AudioSource sfxSource = null;


    void Awake()
    {
        bgmSource.playOnAwake = true;
    }

    public void SFXSet()
    {
        bgmSource.Stop();
        sfxSource.Play();
        Invoke("BGMPalye",0.2f);
    }

    void BGMPalye()
    {
        bgmSource.Play();
    }
}
