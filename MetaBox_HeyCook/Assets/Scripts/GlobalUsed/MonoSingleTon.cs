using UnityEngine;

namespace Kum
{
    public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
    {
        static private T instance = null;
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
                    instance = FindObjectOfType<T>(typeof(T) as T);

                    if (instance == null)
                    {
                        GameObject instObj = new(typeof(T).ToString(), typeof(T));
                        instObj.TryGetComponent(out T _instance);
                        instance = _instance;
                    }
                }

                return instance;
            }
        }

        protected void Awake()
        {
            if (FindObjectOfType<T>(typeof(T) as T).gameObject != this.gameObject)
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        protected void OnApplicationQuit()
        {
            applicationQuitting = true;
        }
    }
}
