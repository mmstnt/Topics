using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public int levelSave;
    public string sceneToSave;

    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();

    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public void saveGameScene(GameSceneSO savedScene) 
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    public GameSceneSO getSavedScene() 
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);

        return newScene;
    }
}
