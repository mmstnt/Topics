using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [Header("事件監聽")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;

    public float fadeDuration;
    private void Awake()
    {
        currentLoadedScene = firstLoadScene;
        currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        StartCoroutine(unLoadPreviousScene());
    }

    private IEnumerator unLoadPreviousScene() 
    {
        if (fadeScreen) 
        {
            //漸入漸出
        }

        yield return new WaitForSeconds(fadeDuration);

        if (currentLoadedScene != null) 
        {
            yield return currentLoadedScene.sceneReference.UnLoadScene();
        }

        LoadNewScene();
    }

    private void LoadNewScene() 
    {
        sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
