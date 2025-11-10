using UnityEngine;
using System.Collections;

public class InGameInstructions : MonoBehaviour
{
    [Header("UI to Open")]
    public GameObject uiPanel;

    [Header("UI Display Settings")]
    [Tooltip("How long the UI stays visible (in seconds)")]
    public float displayTime = 3f;

    private static InGameInstructions currentInstance; // Track the instance, not just the coroutine
    private Coroutine myCoroutine; // Instance-specific coroutine reference

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowThisUIPanel();
        }
    }

    private void ShowThisUIPanel()
    {
        // If another instance is active, tell IT to stop its own coroutine
        if (currentInstance != null && currentInstance != this)
        {
            currentInstance.StopMyCoroutine();
        }

        // Start showing this UI
        myCoroutine = StartCoroutine(ShowUIPanel());
        currentInstance = this;
    }

    private void StopMyCoroutine()
    {
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null;
        }
        
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    private IEnumerator ShowUIPanel()
    {
        uiPanel.SetActive(true);
        Debug.Log("UI Shown: " + uiPanel.name);

        yield return new WaitForSeconds(displayTime);

        uiPanel.SetActive(false);
        Debug.Log("UI Hidden: " + uiPanel.name);

        // Clear static tracker if this is still the active instance
        if (currentInstance == this)
        {
            currentInstance = null;
            myCoroutine = null;
        }
    }
}