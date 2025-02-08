using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("®∆•Û∫ ≈•")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(this.gameObject);

        saveData = new Data();
    }

    public void OnEnable()
    {
        saveDataEvent.onEventRaised += save;
        loadDataEvent.onEventRaised += load;
    }

    public void OnDisable()
    {
        saveDataEvent.onEventRaised -= save;
        loadDataEvent.onEventRaised -= load;
    }

    public void registerSaveData(ISaveable saveable) 
    {
        if (!saveableList.Contains(saveable)) 
        {
            saveableList.Add(saveable);
        }
    }

    public void unRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void save() 
    {
        foreach (var saveable in saveableList) 
        {
            saveable.getSaveDate(saveData);
        }

        foreach (var a in saveData.characterPosDict)
        {
            Debug.Log(a);
        }
    }

    public void load() 
    {
        foreach (var saveable in saveableList)
        {
            saveable.loadData(saveData);
        }
    }
}
