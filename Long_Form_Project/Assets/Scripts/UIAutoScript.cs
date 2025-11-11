using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIAutoSelect : MonoBehaviour
{
    public GameObject firstSelected;
    
    private void OnEnable()     
    {
        // Clear any existing selection
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        
        // Wait for Input System to be ready
        StartCoroutine(SetSelectedNextFrame());
    }
    
    private IEnumerator SetSelectedNextFrame()
    {
        // Wait one frame for UI Input Module to initialize
        yield return null;
        
        // Additional wait if using gamepad (Input System needs extra time)
        if (Gamepad.current != null)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        // Set the selected GameObject
        if (firstSelected != null && firstSelected.activeInHierarchy && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}