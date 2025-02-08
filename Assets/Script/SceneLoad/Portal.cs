using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour,IInteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;


    public void triggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, sceneToGo.positionToGo, true, true);
    }

}
