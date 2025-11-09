using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUi;
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject playerPrefab;
    
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume(); 
                Time.timeScale = 1f;
            }
            else 
            { 
                Pause(); 
                Time.timeScale = 0f; 
            }
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
        GameIsPaused=true;
        
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
        SceneManager.LoadScene("Main Menu");
    }
    private IEnumerator SetSelectedNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }



}