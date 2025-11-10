using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAutoSelect : MonoBehaviour
{
    [Header("Selection")]
    public GameObject firstSelected;
    
    [Header("Panels")]
    public GameObject keyboardPanel;
    public GameObject controllerPanel;
    
    [Header("Buttons")]
    public GameObject keyboardButton;
    public GameObject controllerButton;

    [Header("Settings")]
    [Tooltip("Force reselection if nothing is selected")]
    public bool forceReselection = true;
    
    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found in scene! UI navigation will not work.");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SelectAfterFrame());
    }

    private void Update()
    {
        // Force reselection if nothing is selected (useful for gamepad navigation)
        if (forceReselection && eventSystem != null && eventSystem.currentSelectedGameObject == null)
        {
            SelectFirstAvailable();
        }
    }

    private IEnumerator SelectAfterFrame()
    {
        // Wait for end of frame to ensure UI is fully initialized
        yield return new WaitForEndOfFrame();
        
        SelectFirstAvailable();
    }

    private void SelectFirstAvailable()
    {
        if (eventSystem == null) return;

        // Clear current selection first
        eventSystem.SetSelectedGameObject(null);
        
        // Try to select the first selected object if it's active
        if (firstSelected != null && firstSelected.activeInHierarchy)
        {
            // Make sure the object has a Selectable component
            Selectable selectable = firstSelected.GetComponent<Selectable>();
            if (selectable != null && selectable.interactable)
            {
                eventSystem.SetSelectedGameObject(firstSelected);
                Debug.Log($"Selected: {firstSelected.name}");
            }
            else
            {
                Debug.LogWarning($"First selected object '{firstSelected.name}' is not interactable or missing Selectable component!");
            }
        }
        else
        {
            Debug.LogWarning("First selected object is null or inactive!");
        }
    }
    
    public void SwitchToKeyboard()
    {
        if (keyboardPanel != null) keyboardPanel.SetActive(true);
        if (controllerPanel != null) controllerPanel.SetActive(false);
        if (keyboardButton != null) keyboardButton.SetActive(true);
        if (controllerButton != null) controllerButton.SetActive(false);
        
        StartCoroutine(ReselectAfterSwitch());
    }

    public void SwitchToController()
    {
        if (controllerPanel != null) controllerPanel.SetActive(true);
        if (keyboardPanel != null) keyboardPanel.SetActive(false);
        if (controllerButton != null) controllerButton.SetActive(true);
        if (keyboardButton != null) keyboardButton.SetActive(false);
        
        StartCoroutine(ReselectAfterSwitch());
    }
    
    private IEnumerator ReselectAfterSwitch()
    {
        yield return new WaitForEndOfFrame();
        
        SelectFirstAvailable();
    }

    // Optional: Call this from other scripts to force reselection
    public void ForceReselect()
    {
        StartCoroutine(SelectAfterFrame());
    }
}