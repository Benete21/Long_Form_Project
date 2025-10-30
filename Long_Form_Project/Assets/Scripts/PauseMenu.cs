using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    
    private PlayerControls controls;
    private bool menuPressed;
    [SerializeField] private GameObject pauseFirstButton;

    public GameObject pauseMenuUi;
    
    void Start() {
        Cursor.visible = true;
    }

    void Awake()
    {
        controls = new PlayerControls(); // Initialize controls!
        
        // Only toggle on button press, not release
        controls.Player.PauseMenu.performed += ctx => TogglePause();
    }
    
    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();
    
    void Update()
    {
        // Keyboard fallback for Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
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
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Langa_Current");

    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
    }
    
    private IEnumerator SetSelectedNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }


    public void MainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("APP IS QUITTING");
    }
}
