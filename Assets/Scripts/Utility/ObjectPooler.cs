using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectPooler
{
    private static readonly Dictionary<int, List<GameObject>> DictionaryOfPooledGameObjects =
        new();

    public static void PoolGameObject<T>(T obj, int amm, Transform parent) where T : MonoBehaviour
    {
        PoolGameObject(obj.gameObject, amm, parent);
    }

    public static void PoolGameObject(GameObject obj, int amm, Transform parent)
    {
        List<GameObject> pool = new List<GameObject>();

        parent = parent ? parent : new GameObject($"{obj.name}'s Pool List").transform;


        for (int i = 0; i < amm; i++)
        {
            GameObject clone = Object.Instantiate(obj, parent);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.gameObject.SetActive(false);
            pool.Add(clone.gameObject);
        }


        if (DictionaryOfPooledGameObjects.ContainsKey(obj.GetInstanceID()))
        {
            DictionaryOfPooledGameObjects[obj.GetInstanceID()].AddRange(pool);
        }
        else
        {
            DictionaryOfPooledGameObjects.Add(obj.GetInstanceID(), pool);
        }
    }


    public static GameObject Instantiate(GameObject objToInstantiate, int amm = 300, Transform parent = null)
    {
        foreach (var keyPairVal in DictionaryOfPooledGameObjects)
        {
            if (keyPairVal.Key.Equals(objToInstantiate.GetInstanceID()))
            {
                foreach (var element in keyPairVal.Value)
                {
                    if (!element.activeSelf)
                    {
                        return element;
                    }
                }

                break;
            }
        }


        PoolGameObject(objToInstantiate, amm, parent);
        return Instantiate(objToInstantiate, amm, parent);
    }

    public static GameObject Instantiate(GameObject objToInstantiate, Vector3 position, Quaternion rotation,
        int amm = 300,
        Transform parent = null)
    {
        GameObject foundElement = Instantiate(objToInstantiate, amm, parent);
        foundElement.transform.position = position;
        foundElement.transform.rotation = rotation;

        return foundElement;
    }

    public static T Instantiate<T>(T objToInstantiate, int amm = 300, Transform parent = null) where T : MonoBehaviour
    {
        return Instantiate(objToInstantiate.gameObject, amm, parent).GetComponent<T>();
    }

    public static T Instantiate<T>(T objToInstantiate, Vector3 position, Quaternion rotation, int amm = 300,
        Transform parent = null) where T : MonoBehaviour
    {
        return Instantiate(objToInstantiate.gameObject, position, rotation, amm, parent).GetComponent<T>();
    }
}