using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    Queue<T> pool = null;

    public int PoolMaxSize { get; set; } = 20;
    public int PoolMinSize { get; set; } = 5;
    private int objCount = 0;

    T obj = null;

    protected ObjectPool()
    {
        pool = new Queue<T>();
    }

    abstract public T CreatePool();

    public void PoolInit()
    {
        if (obj == null)
        {
            obj = CreatePool();
        }

        for(int i = 0; i < PoolMinSize; i++)
        {
            objCount++;
            T inst = Instantiate<T>(obj);
            inst.gameObject.SetActive(false);
            pool.Enqueue(inst);
        }
    }

    public T Get()
    {
        Debug.Log(pool.Count);
        Debug.Log("�����ٰ�");
        if (obj == null)
        {
            obj = CreatePool();
        }

        if (pool.Count > 0)
        {
            pool.Peek().gameObject.SetActive(true);
            return pool.Dequeue();
        }
        else
        {
            Debug.Log("���� ������ٰ�");
            objCount++;
            return Instantiate<T>(obj);
        }
    }

    public void Release(T tObj)
    {
        Debug.Log("�� ��Ȱ��ȭ");
        if (objCount >= PoolMaxSize)
        {
            Destroy(tObj);
        }
        else
        {
            tObj.gameObject.SetActive(false);
            pool.Enqueue(tObj);
            Debug.Log(pool.Count);
        }
    }

    public void DestroyPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            Destroy(pool.Dequeue());
        }
    }

}
