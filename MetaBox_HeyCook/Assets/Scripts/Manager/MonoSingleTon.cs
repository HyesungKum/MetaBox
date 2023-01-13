using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T instance = null;

    static public T Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>(typeof(T) as T) ;

                if (instance == null)
                {
                    GameObject instObj = new(typeof(T).ToString(), typeof(T));
                    instance = instObj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (FindObjectOfType<SoundManager>().gameObject != this.gameObject) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
