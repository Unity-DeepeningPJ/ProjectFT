using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject pooledObjectPrefab; // Ǯ�� ������ ������Ʈ ������
    public int poolSize = 10; // �ʱ� Ǯ ũ��
    public bool canExpand = true; // Ǯ ũ�⸦ Ȯ���� �� �ִ��� ����

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
            obj.name = pooledObjectPrefab.name + " (Pooled)"; // ������Ʈ �̸� ����
            pooledObjects.Add(obj);
        }

        // �� ���� �� ������Ʈ Ǯ ����
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

        // Ǯ�� ��� ������ ������Ʈ�� ������ ���� ���� (Ȯ�� ���� ���� Ȯ��)
        if (canExpand)
        {
            GameObject obj = Instantiate(pooledObjectPrefab);
            obj.SetActive(false);
            obj.name = pooledObjectPrefab.name + " (Pooled)"; // ������Ʈ �̸� ����
            pooledObjects.Add(obj);
            return obj;
        }

        return null; // ��� ������ ������Ʈ�� ����, Ȯ�嵵 �Ұ����� ��� null ��ȯ
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    // �� ���� �� ������Ʈ Ǯ ����
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
