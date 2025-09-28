using UnityEngine;

public class OpenUI : MonoBehaviour
{
    [Header("UI to Open")]
    public GameObject uiPanel;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(true);
            }
        }
    }
}