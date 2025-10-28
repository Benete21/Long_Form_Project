using UnityEngine;
using System.Collections;

public class InGameInstructions : MonoBehaviour
{
    [Header("UI to Open")]
    public GameObject uiPanel;

    [Header("UI Display Settings")]
    [Tooltip("How long the UI stays visible (in seconds)")]
    public float displayTime = 3f; // Adjustable in Inspector

    private static Coroutine currentCoroutine; // Tracks the currently running coroutine
    private static GameObject currentUI;        // Tracks the currently active UI panel

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowThisUIPanel();
        }
    }

    private void ShowThisUIPanel()
    {
        // If another UI is active, stop its coroutine and hide it
        if (currentUI != null && currentUI != uiPanel)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentUI.SetActive(false);
        }

        // Start showing this UI
        currentCoroutine = StartCoroutine(ShowUIPanel());
        currentUI = uiPanel;
    }

    private IEnumerator ShowUIPanel()
    {
        uiPanel.SetActive(true);
        Debug.Log("UI Shown: " + uiPanel.name);

        yield return new WaitForSeconds(displayTime);

        uiPanel.SetActive(false);
        Debug.Log("UI Hidden: " + uiPanel.name);

        // Clear static trackers if this is the currently active one
        if (currentUI == uiPanel)
        {
            currentUI = null;
            currentCoroutine = null;
        }
    }
}