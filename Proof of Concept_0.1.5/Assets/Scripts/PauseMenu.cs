﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public static bool gameWon = false;
    public static int coinsCollected;

    public Text coinText;

    [SerializeField] GameObject PauseMenuBehavior;
    public GameObject winMenu;

    private void Start()
    {
        gameWon = false;
        winMenu.SetActive(false);
        coinsCollected = 0;
    }
    private void Awake()
    {
        gameWon = false;
        winMenu.SetActive(false);
        coinsCollected = 0;
    }

    void Update()
    {
        coinText.text = coinsCollected.ToString();
        if (!gameWon)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
    }
    public void Resume()
    {
        Cursor.visible = false;
        PauseMenuBehavior.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
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
        Cursor.visible = true;
        PauseMenuBehavior.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }
}