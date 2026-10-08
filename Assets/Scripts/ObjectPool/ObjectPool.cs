using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private uint _initPoolSize;
    [SerializeField] private PooledObject _pooledObject;

    private Stack<PooledObject> _poolStack;

    private void Start()
    {
        SetupPool();
    }

    // プールを作成する
    private void SetupPool()
    {
        _poolStack = new Stack<PooledObject>();
        PooledObject instance = null;
        for (int i = 0; i < _initPoolSize; i++)
        {
            instance = Instantiate(_pooledObject);
            instance.transform.SetParent(this.transform);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            _poolStack.Push(instance);
        }
    }

    public PooledObject GetPooledObject()
    {
        // Poolが足りなければ新しく生成
        if (_poolStack.Count == 0)
        {
            PooledObject newInstance = Instantiate(_pooledObject);
            newInstance.transform.SetParent(this.transform);
            newInstance.Pool = this;
            return newInstance;
        }

        // Stackから取り出す
        PooledObject nextInstance = _poolStack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        if (pooledObject.gameObject.activeSelf == false) return;

        _poolStack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }

    public void ClearAllPoolObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                PooledObject pooled = child.GetComponent<PooledObject>();
                if (pooled != null)
                {
                    ReturnToPool(pooled);
                }
            }
        }
    }
}
