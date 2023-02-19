using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignInPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputUserID = null;
    [SerializeField] Button signInButton = null;

    void Awake()
    {
        signInButton.onClick.AddListener(OnClick_SignIn);
    }

    void OnClick_SignIn()
    {
        if (inputUserID.text.Length < 4) return;

        string fileName = "UserData";
        string path = $"{Application.dataPath}/{fileName}.txt";

        File.WriteAllText(path, inputUserID.text);
        this.gameObject.SetActive(false);
    }
}
