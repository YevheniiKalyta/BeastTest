using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SceneItemsPool: MonoBehaviour
{
    [SerializeField] private SceneItem sceneItemPrefab;
    public ObjectPool<SceneItem> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<SceneItem> (CreateMethod, OnGetAction, OnReleaseAction);
    }

    protected virtual void OnReleaseAction(SceneItem sceneItem) => sceneItem.gameObject.SetActive(false);

    protected virtual void OnGetAction(SceneItem sceneItem) => sceneItem.gameObject.SetActive(true);

    protected virtual SceneItem CreateMethod() => Instantiate(sceneItemPrefab);

    public SceneItem CreateSceneItem(Item item, Vector3 position)
    {
        SceneItem sceneItem = objectPool.Get();
        sceneItem.transform.position = position;
        sceneItem.SetItem(item);
        return null;
    }
}
