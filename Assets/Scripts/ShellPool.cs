using System.Collections.Generic;
using UnityEngine;

public class ShellPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 10;

    readonly Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
            CreateObject();
    }

    public GameObject GetObject()
    {
        if (pool.Count == 0)
            CreateObject();

        return pool.Dequeue();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    void CreateObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);

        Shell shell = obj.GetComponent<Shell>();
        if (shell == null)
            shell = obj.AddComponent<Shell>();
        shell.SetPool(this);

        pool.Enqueue(obj);
    }
}
