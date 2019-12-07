using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject MainMenuBehavior;
    public GameObject OptionsObject;
    public GameObject Main_Menu;
    public GameObject VolumeObject;
    public GameObject ControlsObject;
    public GameObject GraphicsObject;

    public void Start()
    {
        PlayerPrefs.SetInt("loadedFromMenu", 1);
    }

    public void Play()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("White Box 1");
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
    public void Options()
    {
        Debug.Log("Enable: Options");
        OptionsObject.SetActive(true);
        Main_Menu.SetActive(false);
    }

    public void Controls()
    {
        Debug.Log("Enable: Controls");
        ControlsObject.SetActive(true);
        OptionsObject.SetActive(false);
        Main_Menu.SetActive(false);
    }

    public void Volume()
    {
        Debug.Log("Enable: Volume");
        VolumeObject.SetActive(true);
        OptionsObject.SetActive(false);
        Main_Menu.SetActive(false);
    }

    public void Graphics()
    {
        Debug.Log("Enable: Graphics");
        GraphicsObject.SetActive(true);
        OptionsObject.SetActive(false);
        Main_Menu.SetActive(false);
    }

    public void Back()
    {
        OptionsObject.SetActive(false);
        VolumeObject.SetActive(false);
        ControlsObject.SetActive(false);
        GraphicsObject.SetActive(false);
        Main_Menu.SetActive(true);
        Debug.Log("Enable: Main Menu");
    }

    public void OptionsBack()
    {
        VolumeObject.SetActive(false);
        ControlsObject.SetActive(false);
        GraphicsObject.SetActive(false);
        Main_Menu.SetActive(false);
        OptionsObject.SetActive(true);
        Debug.Log("Enable: Options");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }
}
