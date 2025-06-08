using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage_pool : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImagePrefab;

    private Queue<GameObject> available = new Queue<GameObject>();

    public static AfterImage_pool instance { get; private set; }

    private void Awake()
    {
        instance= this;
        GrowPool();
    }

    private void GrowPool() {
        for (int i = 0; i <10; i++) {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddtoPool(instanceToAdd);
        }
    }

    public void AddtoPool(GameObject Instance) { 
        Instance.SetActive(false);
        available.Enqueue(Instance);
    }

    public GameObject GetFromPool() {
        if (available.Count == 0) { 
            GrowPool();
        }

        var Instance = available.Dequeue();
        Instance.SetActive(true);
        return Instance;
    }
}
