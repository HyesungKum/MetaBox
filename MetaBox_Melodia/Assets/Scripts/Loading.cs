using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncLoad = null;
    [SerializeField] Slider loadingBar = null;

    void Start()
    {
        StartCoroutine(nameof(Load));
    }

    IEnumerator Load()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Melodia");

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }
    }
    
}

