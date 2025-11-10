using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUi;
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject playerPrefab;
    
    private PlayerInput playerInput;
    private InputAction pauseAction;
    
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();
        
        if (playerInput != null)
        {
            // Look for the action named "Pause" inside the "PauseMenu" action map
            pauseAction = playerInput.actions.FindAction("PauseMenu/Pause");
            
            // Alternative: if you named it differently
            // pauseAction = playerInput.actions["Pause"];
            
            if (pauseAction != null)
            {
                Debug.Log("Pause action found!");
            }
            else
            {
                Debug.LogError("Pause action not found! Check your Input Actions setup.");
            }
        }
    }
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.Enable();
            pauseAction.performed += OnPausePressed;
            Debug.Log("Pause action enabled");
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
            pauseAction.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        Debug.Log("Pause button pressed!");
        TogglePause();
    }

    void Update()
    {
        // Direct gamepad check as fallback
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            Debug.Log("Start button pressed directly!");
            TogglePause();
        }
        
        // Keyboard support
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed!");
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (playerPrefab != null)
            playerPrefab.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (playerPrefab != null)
            playerPrefab.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Main Menu");
    }
    
    private IEnumerator SetSelectedNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
}