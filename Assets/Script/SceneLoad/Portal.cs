using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour,IInteractable
{
    [Header("¼s¼½")]
    public SceneLoadEventSO sceneLoadEventSO;
    public VoidEventSO portalChooseEvent;

    [Header("°Ñ¼Æ")]
    public SpriteRenderer image;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public bool isChoose;

    private void OnEnable()
    {
        isChoose = false;
    }

    public void changeImage() 
    {
        image.sprite = sceneToGo.image;
    }

    public void triggerAction()
    {
        isChoose = true;
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGo, sceneToGo.positionToGo, true, true);
        portalChooseEvent.raiseEvent();
    }

}
