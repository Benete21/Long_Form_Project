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
        // Get the PlayerInput component (make sure it's on this GameObject or find it)
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();
        
        // Get the pause action from your PauseMenu action map
        if (playerInput != null)
        {
            pauseAction = playerInput.actions["PauseMenu"];
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
            pauseAction.performed += OnPausePressed;
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    void Update()
    {
        // Keep keyboard support as backup
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        Time.timeScale = 1f; // Reset time scale
        GameIsPaused = false;
        SceneManager.LoadScene("Main Menu");
    }
    
    private IEnumerator SetSelectedNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
}