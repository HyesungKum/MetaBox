using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T instance = null;
    static private object _lock = new object();
    static private bool applicationQuitting = false;

    static public T Inst
    {
        get
        {
            if (applicationQuitting)
            {
                return null;
            }

            if (instance == null)
            {
                instance = FindObjectOfType<T>(typeof(T) as T) ;

                if (instance == null)
                {
                    lock (_lock)
                    {
                        GameObject instObj = new(typeof(T).ToString(), typeof(T));
                        instance = instObj.GetComponent<T>();
                    }
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (FindObjectOfType<T>(typeof(T) as T).gameObject != this.gameObject) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        applicationQuitting = true;
    }
}
