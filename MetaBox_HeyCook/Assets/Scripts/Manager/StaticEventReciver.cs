using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NewCostomer();
public delegate void ScoreModi(int value);
public delegate void GamePause();
public delegate void GameResume();
public delegate void GameOver();

static public class StaticEventReciver
{
    static public NewCostomer NewCostomer = null;
    static public ScoreModi ScoreModi = null;
    static public GamePause GamePause = null;
    public static GameResume GameResume = null;
    static public GameOver GameOver = null;

    static public void CallNewComstomer()
    {
        NewCostomer?.Invoke();
    }
    static public void CallScoreModi(int value)
    {
        ScoreModi?.Invoke(value);
    }
    static public void CallGamePause()
    {
        GamePause?.Invoke();
    }
    static public void CallGameResume()
    {
        GameResume?.Invoke();
    }
    static public void CallGameOver()
    {
        GameOver?.Invoke();
    }
}