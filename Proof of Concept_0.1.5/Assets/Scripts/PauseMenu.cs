using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public static bool gameWon = false;
    public static int coinsCollected;

    public GameObject crosshair;

    public Text coinText;

    [SerializeField] GameObject PauseMenuBehavior;
    public GameObject winMenu;

    private void Start()
    {
        gameWon = false;
        winMenu.SetActive(false);
        coinsCollected = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Awake()
    {
        gameWon = false;
        winMenu.SetActive(false);
        coinsCollected = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        coinText.text = coinsCollected.ToString();
        if (!gameWon)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Xbox Start"))
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
        }
        else
        {
            winMenu.SetActive(true);
            GameIsPaused = true;
            Cursor.visible = true;
            Time.timeScale = 0f;

        }

        if(Salty_Rusty_Controller.isAiming)
        {
            crosshair.SetActive(true);
        }
        else
        {
            crosshair.SetActive(false);
        }


    }
    public void Resume()
    {
        Cursor.visible = false;
        PauseMenuBehavior.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void MainMenu()
    {
        Cursor.visible = true;
        GameIsPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene("Main Menu");
    }
    void Pause()
    {
        crosshair.SetActive(false);
        Cursor.visible = true;
        PauseMenuBehavior.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }

    public static void GameWon()
    {
        gameWon = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}