using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject instance = Instantiate(prefab, transform);
            instance.SetActive(false);
            availableObjects.Enqueue(instance);
        }
    }

    public GameObject GetObject()
    {
        if (availableObjects.Count == 0)
        {
            // Создаем новый объект, если все объекты заняты
            GameObject instance = Instantiate(prefab, transform);
            return instance;
        }

        GameObject obj = availableObjects.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }
}