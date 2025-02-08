using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefinition getDataID();

    void registerSaveDate() => DataManager.instance.registerSaveData(this);

    void unregisterSaveDate() => DataManager.instance.unRegisterSaveData(this);

    void getSaveDate(Data data);

    void loadData(Data data);
}
