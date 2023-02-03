using UnityEngine;

namespace Kum
{
    public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
    {
        static private T instance = null;
        static private object _lock = new();
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
                        lock (_lock)
                        {
                            GameObject instObj = new(typeof(T).ToString(), typeof(T));
                            instObj.TryGetComponent(out T instance);
                        }
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (FindObjectOfType<T>(typeof(T) as T).gameObject != this.gameObject) Destroy(this.gameObject);
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        protected void OnApplicationQuit()
        {
            applicationQuitting = true;
        }
    }
}
