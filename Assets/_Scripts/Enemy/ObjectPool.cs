using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject pooledObjectPrefab; // 풀에 저장할 오브젝트 프리팹
    public int poolSize = 10; // 초기 풀 크기
    public bool canExpand = true; // 풀 크기를 확장할 수 있는지 여부

    private List<GameObject> pooledObjects;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pooledObjectPrefab);
            obj.SetActive(false);
            obj.name = pooledObjectPrefab.name + " (Pooled)"; // 오브젝트 이름 지정
            pooledObjects.Add(obj);
        }

        // 씬 변경 시 오브젝트 풀 정리
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        // 풀에 사용 가능한 오브젝트가 없으면 새로 생성 (확장 가능 여부 확인)
        if (canExpand)
        {
            GameObject obj = Instantiate(pooledObjectPrefab);
            obj.SetActive(false);
            obj.name = pooledObjectPrefab.name + " (Pooled)"; // 오브젝트 이름 지정
            pooledObjects.Add(obj);
            return obj;
        }

        return null; // 사용 가능한 오브젝트가 없고, 확장도 불가능한 경우 null 반환
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    // 씬 변경 시 오브젝트 풀 정리
    void OnSceneUnloaded(Scene scene)
    {
        foreach (GameObject obj in pooledObjects)
        {
            Destroy(obj);
        }
        pooledObjects.Clear();
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
