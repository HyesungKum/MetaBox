using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NewCostomer();
public delegate void ScoreModi(int value);
public delegate void CorrectIngred(Vector3 pos);
public delegate void WrongIngred(Vector3 pos);
public delegate void CorrectFood();
public delegate void WrongFood(int wrongLevel);
public delegate void GamePause();
public delegate void GameResume();
public delegate void GameOver();

static public class StaticEventReciver
{
    static public NewCostomer NewCostomer = null;
    static public ScoreModi ScoreModi = null;
    static public CorrectIngred CorrectIngred = null;
    static public WrongIngred WrongIngred = null;
    static public CorrectFood CorrectFood = null;
    static public WrongFood WrongFood = null;
    static public GamePause GamePause = null;
    public static GameResume GameResume = null;
    static public GameOver GameOver = null;

    static public void CallNewComstomer() => NewCostomer?.Invoke();
    static public void CallScoreModi(int value) => ScoreModi?.Invoke(value);
    static public void CallCorrectIngred(Vector3 pos) => CorrectIngred?.Invoke(pos);
    static public void CallWrongIngred(Vector3 pos) => WrongIngred?.Invoke(pos);
    static public void CallCorrectFood() => CorrectFood?.Invoke();
    static public void CallWrongFood(int wrongLevel) => WrongFood?.Invoke(wrongLevel);
    static public void CallGamePause() => GamePause?.Invoke();
    static public void CallGameResume() => GameResume?.Invoke();
    static public void CallGameOver() => GameOver?.Invoke();
}