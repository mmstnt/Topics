using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject newGameButton;
    [Header("¼s¼½")]
    public VoidEventSO loadDataEvent;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void LoadGame() 
    {
        if (!DataManager.instance.start)
        {
            loadDataEvent.raiseEvent();
        }
    }
}
