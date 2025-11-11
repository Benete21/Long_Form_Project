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
    [SerializeField] private GameObject howToPlayUi;
    [SerializeField] private GameObject mainMenuUi;

    void Awake()
    {
        controls = new PlayerControls(); // Initialize controls!
    }
    
    void Start()
    {
        // Set the first button selected when menu loads
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
        
        Cursor.visible = true;
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
    
    public void OpenHowToPlay()
    {
        mainMenuUi.SetActive(false);
        howToPlayUi.SetActive(true);

        // Clear selection first
        EventSystem.current.SetSelectedGameObject(null);

        // Optional: if your HowToPlay script has a first selected element
        var howToPlayFirst = howToPlayUi.GetComponentInChildren<UnityEngine.UI.Selectable>();
        if (howToPlayFirst != null)
            EventSystem.current.SetSelectedGameObject(howToPlayFirst.gameObject);
    }

    public void CloseHowToPlay()
    {
        howToPlayUi.SetActive(false);
        mainMenuUi.SetActive(true);

        // Reset selection to pause menu’s first button
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
    }
    public void IntroNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }
}
