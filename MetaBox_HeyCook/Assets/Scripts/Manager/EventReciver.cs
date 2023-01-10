using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NewCostomer();
public delegate void GameOver();
public delegate void ScoreModi(int value);

static public class EventReciver
{
    static public NewCostomer NewCostomer = null;
    static public GameOver GameOver = null;
    static public ScoreModi ScoreModi = null;

    static public void CallNewComstomer()
    {
        NewCostomer?.Invoke();
    }
    static public void CallGameOver()
    {
        GameOver?.Invoke();
    }
    static public void CallScoreModi(int value)
    {
        ScoreModi?.Invoke(value);
    }
}