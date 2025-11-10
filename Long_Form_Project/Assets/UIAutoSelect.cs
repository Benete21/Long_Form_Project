using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAutoSelect : MonoBehaviour
{
    public GameObject firstSelected;
    public GameObject keyboardPanel;
    public GameObject controllerPanel;
    
    public GameObject keyboardButton;
    public GameObject controllerButton;

    private void OnEnable()
    {
        // Use a coroutine to ensure selection happens after the frame is complete
        StartCoroutine(SelectAfterFrame());
    }

    private IEnumerator SelectAfterFrame()
    {
        // Wait for end of frame to ensure UI is fully initialized
        yield return new WaitForEndOfFrame();
        
        // Clear current selection first
        EventSystem.current.SetSelectedGameObject(null);
        
        // Set the new selection
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
    
    public void SwitchToKeyboard()
    {
        keyboardPanel.SetActive(true);
        controllerPanel.SetActive(false);
        keyboardButton.SetActive(true);
        controllerButton.SetActive(false);
        
        // Reselect the back button after switching
        StartCoroutine(ReselectAfterSwitch());
    }

    public void SwitchToController()
    {
        controllerPanel.SetActive(true);
        keyboardPanel.SetActive(false);
        controllerButton.SetActive(true);
        keyboardButton.SetActive(false);
        
        // Reselect the back button after switching
        StartCoroutine(ReselectAfterSwitch());
    }
    
    private IEnumerator ReselectAfterSwitch()
    {
        yield return new WaitForEndOfFrame();
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
