using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public int levelSave;
    public string sceneToSave;
    public List<string> cardPool;
    public List<string> levelPool;

    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();

    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public Dictionary<IBuff, Character> buffCharacter = new Dictionary<IBuff, Character>();

    public Dictionary<string, Vector3> portalSave = new Dictionary<string, Vector3>();

    public void saveGameScene(GameSceneSO savedScene) 
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    public void saveGameSceneList(List<GameSceneSO> savedSceneList) 
    {
        levelPool = new List<string>();
        foreach(var savedScene in savedSceneList) 
        {
            levelPool.Add(JsonUtility.ToJson(savedScene));
        }
    }

    public void savePortal(List<GameObject> portalList) 
    {
        portalSave = new Dictionary<string, Vector3>();
        foreach(var portal in portalList) 
        {
            GameSceneSO savedScene = portal.GetComponent<Portal>().sceneToGo;
            portalSave.Add(JsonUtility.ToJson(savedScene),portal.transform.position);
        }
    }

    public GameSceneSO getSavedScene() 
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);

        return newScene;
    }

    public List<GameSceneSO> getSaveGameSceneList() 
    {
        List<GameSceneSO> newSceneList = new List<GameSceneSO>();
        foreach(var level in levelPool) 
        {
            var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
            JsonUtility.FromJsonOverwrite(level, newScene);
            newSceneList.Add(newScene);
        }
        return newSceneList;
    }

    public Dictionary<GameSceneSO, Vector3> getSavePortal()
    {
        Dictionary<GameSceneSO, Vector3> newPortalList = new Dictionary<GameSceneSO, Vector3>();
        foreach (var portal in portalSave)
        {
            var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
            JsonUtility.FromJsonOverwrite(portal.Key, newScene);
            newPortalList.Add(newScene, portal.Value);
        }
        return newPortalList;
    }
}
