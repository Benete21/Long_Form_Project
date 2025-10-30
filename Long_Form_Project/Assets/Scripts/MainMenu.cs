using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class MainMenu : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField] private GameObject playFirst;

    void Awake()
    {
        controls = new PlayerControls(); // Initialize controls!
    }
    
    void Start()
    {
        // Set the first button selected when menu loads
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
    }
    
    void OnEnable()
    {
        controls?.Enable();
        // Enable UI input module for gamepad navigation
        var inputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        if (inputModule != null)
        {
            inputModule.enabled = true;
        }
    }
    void OnDisable() => controls?.Disable();

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }
    
    private IEnumerator SetSelectedNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(playFirst);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
        EventSystem.current.SetSelectedGameObject(null);
    }
}
