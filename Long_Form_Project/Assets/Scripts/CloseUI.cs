using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [Header("UI to Disable")]
    public GameObject uiPanel;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(false);
            }
        }
    }
}