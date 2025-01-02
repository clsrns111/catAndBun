using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    public static PrefabFactory Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public T Create<T>(T prefab, Vector3 pos = default, Quaternion rotation = default) where T : MonoBehaviour
    {
        if(rotation == default)
            rotation = Quaternion.identity;

        if (pos == default)
            pos = prefab.transform.position;

        T instance = Instantiate(prefab, pos, rotation); 

        return instance;
    }
}
