using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour,ISaveable
{
    public static SceneLoadManager instance;
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    public int currentLevel;
    public float currentLevelHealthAddition;
    public float currentLevelDamageAddition;
    public Vector2 levelAddition;
    [Header("事件監聽")]
    public SceneLoadEventSO sceneLoadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;

    [Header("廣播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEventSO;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("場景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    public GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;

    public float fadeDuration;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        sceneLoadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true, false);

        //newGame();
    }

    private void OnEnable()
    {
        sceneLoadEventSO.LoadRequestEvent += onLoadRequestEvent;
        newGameEvent.onEventRaised += newGame;
        backToMenuEvent.onEventRaised += onBackToMenuEvent;

        ISaveable saveable = this;
        saveable.registerSaveDate();
    }

    private void OnDisable()
    {
        sceneLoadEventSO.LoadRequestEvent -= onLoadRequestEvent;
        newGameEvent.onEventRaised -= newGame;
        backToMenuEvent.onEventRaised -= onBackToMenuEvent;

        ISaveable saveable = this;
        saveable.unregisterSaveDate();
    }

    private void onBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true, false);
    }

    private void newGame() 
    {
        currentLevel = 0;
        currentLevelHealthAddition = 0;
        currentLevelDamageAddition = 0;
        levelAddition = new Vector2(0.1f, 0.05f);

        playerTrans.GetComponent<Character>().newGame();
        playerTrans.GetComponent<Character>().isDead = false;

        sceneToLoad = firstLoadScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true, false);
    }

    private void onLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen, bool nextLevel)
    {
        if (isLoading)
            return;

        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (nextLevel)
        {
            currentLevel += 1;
            currentLevelHealthAddition += levelAddition.x;
            currentLevelDamageAddition += levelAddition.y;
        }
        if (currentLoadedScene != null)
        {
            StartCoroutine(unLoadPreviousScene());
        }
        else 
        {
            LoadNewScene();
        }
    }

    private IEnumerator unLoadPreviousScene() 
    {
        if (fadeScreen) 
        {
            //漸黑
            fadeEventSO.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true, false);

        if (currentLoadedScene != null) 
        {
            yield return currentLoadedScene.sceneReference.UnLoadScene();
        }

        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene() 
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += onLoadCompleted;
    }

    private void onLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadedScene = sceneToLoad;

        playerTrans.position = positionToGo;
        
        var isMenu = (currentLoadedScene.sceneType == SceneType.Menu);

        playerTrans.gameObject.SetActive(!isMenu);
        if (fadeScreen) 
        {
            //漸透明
            fadeEventSO.FadeOut(fadeDuration);
        }
        isLoading = false;


        //場景完成後事件
        //if(currentLoadedScene.sceneType != SceneType.Menu)
        afterSceneLoadedEvent.raiseEvent();
    }

    public DataDefinition getDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void getSaveDate(Data data)
    {
        data.saveGameScene(currentLoadedScene);
        data.levelSave = currentLevel;
    }

    public void loadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID)) 
        {
            positionToGo = data.characterPosDict[playerID];
            sceneToLoad = data.getSavedScene();
            currentLevel = data.levelSave;

            onLoadRequestEvent(sceneToLoad, positionToGo, true, false);
        }
    }
}
