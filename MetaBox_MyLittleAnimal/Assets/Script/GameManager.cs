using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    instance = new GameObject(nameof(GameManager), typeof(GameManager)).GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] GameObject thieves = null;

    public int Level { get; private set; }
    public bool isGaming { get; set; } = false;

    private void Start()
    {
        thieves.SetActive(false);
    }
    public void LevelSetting(int level)
    {
        Level = level;
        Debug.Log(Level);
        UIManager.Instance.RunStartText();
    }

    public void GameSetting()
    {
        thieves.SetActive(true);
    }
    public void GameStart()
    {
        isGaming = true;
    }
    
}
