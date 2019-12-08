using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public static int numOfReses;

    public LevelLoader levelLoader;
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider mouseSlider;
    public Slider contrSlider;
    public TMPro.TMP_Dropdown resDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;

    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public Button backOptionsButton;

    public GameObject MainMenuBehavior;
    public GameObject OptionsObject;
    public GameObject Main_Menu;
    public GameObject quitMenu;
    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject GraphicsObject;
    public Button yes;
    public Button no;

    float lastVertical;
    float lastDpadUp;
    float lastHorizontal;
    float lastDpadLeft;

    int closedResChildCount;
    int closedQualityChildCount;

    int resDropIndex;
    int qualityDropIndex;
    int quitMenuIndex;

    Resolution[] resolutions;

    private int mainMenuIndex;
    private int optionsIndex;

    private bool inOptions;
    private bool inMain;
    private bool inQuit;
    private bool lastResStateClosed;
    private bool lastQualStateClosed;
    private bool usingXboxController;


    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, true);
    }

    public void Start()
    {
        usingXboxController = false;
        quitMenuIndex = 0;
        inQuit = false;
        closedResChildCount = resDropdown.gameObject.transform.childCount;
        mainMenuIndex = -1;
        optionsIndex = 0;
        closedQualityChildCount = qualityDropdown.gameObject.transform.childCount;
        lastVertical = 0f;
        lastDpadUp = 0f;
        lastHorizontal = 0f;
        lastDpadLeft = 0f;
        lastResStateClosed = true;
        lastQualStateClosed = false;

        inOptions = false;
        inMain = true;

        if (PlayerPrefs.GetInt("qualityIndex", 1000) != 1000)
        {
            qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex");
            qualityDropIndex = PlayerPrefs.GetInt("qualityIndex");
        }
        else
        {
            qualityDropdown.value = 2;
            qualityDropIndex = 2;
        }
        

        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResIndex = i;
                resDropIndex = i;
            }
        }
        numOfReses = resolutions.Length;
        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();

        PlayerPrefs.SetInt("loadedFromMenu", 1);

        if (PlayerPrefs.GetFloat("mouseSens", 1000f) != 1000f)
        {
            mouseSlider.value = PlayerPrefs.GetFloat("mouseSens");
        }
        else
        {
            mouseSlider.value = 200f;
        }

        if (PlayerPrefs.GetFloat("controllerSens", 1000f) != 1000f)
        {
            contrSlider.value = PlayerPrefs.GetFloat("controllerSens");
        }
        else
        {
            contrSlider.value = 100f;
        }

        if (PlayerPrefs.GetFloat("masterVol", 1000f) != 1000f)
        {
            mixer.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
            masterSlider.value = PlayerPrefs.GetFloat("masterVol");
        }
        else
        {
            mixer.SetFloat("masterVol", 1f);
            masterSlider.value = 1f;
        }
        if (PlayerPrefs.GetFloat("musicVol", 1000f) != 1000f)
        {
            mixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
            musicSlider.value = PlayerPrefs.GetFloat("musicVol");
        }
        else
        {
            mixer.SetFloat("musicVol", 1f);
            musicSlider.value = 1f;
        }
        if (PlayerPrefs.GetFloat("sfxVol", 1000f) != 1000f)
        {
            mixer.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVol"));
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVol");
        }
        else
        {
            mixer.SetFloat("sfxVol", 1f);
            sfxSlider.value = 1f;
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Xbox Start") || Input.GetButtonDown("Xbox Select") || Input.GetButtonDown("Xbox RS") ||
           Input.GetButtonDown("Xbox LS") || Input.GetButtonDown("Xbox B") || Input.GetButtonDown("Xbox LB") ||
           Input.GetButtonDown("Xbox RB") || Input.GetButtonDown("Xbox Interact") || Input.GetButtonDown("Xbox Jump") ||
           Input.GetButtonDown("Xbox Character Switch") || Input.GetButtonDown("Xbox Start") || Input.GetAxisRaw("Xbox Right Trigger") > 0 ||
           Input.GetAxisRaw("Xbox Left Trigger") > 0 || Input.GetAxisRaw("Xbox Dpad Up Down") != 0 || Input.GetAxisRaw("Xbox Dpad Left Right") != 0 ||
           Input.GetAxisRaw("Controller X") != 0 || Input.GetAxisRaw("Controller Y") != 0 || Input.GetAxisRaw("Xbox Vertical") != 0 ||
           Input.GetAxisRaw("Xbox Horizontal") != 0)
        {
            usingXboxController = true;
        }
        else
        {
            if (Input.anyKeyDown || Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                GameObject myEventSystem = GameObject.Find("EventSystem");
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                usingXboxController = false;
            }
        }

        if(usingXboxController)
        {
            updateIndex();
            if (inMain)
            {
                if (Input.GetAxisRaw("Xbox Vertical") > 0 && lastVertical <= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") > 0 && lastDpadUp <= 0)
                {
                    mainMenuIndex--;
                    if (mainMenuIndex == -1)
                        mainMenuIndex = 2;
                }
                else if (Input.GetAxisRaw("Xbox Vertical") < 0 && lastVertical >= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") < 0 && lastDpadUp >= 0)
                {
                    mainMenuIndex++;
                    if (mainMenuIndex == 3)
                        mainMenuIndex = 0;
                }

                if (mainMenuIndex == 0)
                    playButton.Select();
                else if (mainMenuIndex == 1)
                    optionsButton.Select();
                else
                    quitButton.Select();
            }
            else if (inOptions)
            {
                if (Input.GetButtonDown("Xbox B") && resDropdown.gameObject.transform.childCount == closedResChildCount && qualityDropdown.gameObject.transform.childCount == closedQualityChildCount)
                {
                    backFromOptions();
                }
                else
                {
                    if ((Input.GetAxisRaw("Xbox Vertical") > 0 && lastVertical <= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") > 0 && lastDpadUp <= 0) && resDropdown.gameObject.transform.childCount == closedResChildCount && qualityDropdown.gameObject.transform.childCount == closedQualityChildCount)
                    {
                        optionsIndex--;
                        if (optionsIndex == -1)
                            optionsIndex = 7;
                    }
                    else if ((Input.GetAxisRaw("Xbox Vertical") < 0 && lastVertical >= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") < 0 && lastDpadUp >= 0) && resDropdown.gameObject.transform.childCount == closedResChildCount && qualityDropdown.gameObject.transform.childCount == closedQualityChildCount)
                    {
                        optionsIndex++;
                        if (optionsIndex == 8)
                            optionsIndex = 0;
                    }

                    if (optionsIndex == 0)
                    {
                        if (resDropdown.gameObject.transform.childCount > closedResChildCount)
                        {
                            if (lastResStateClosed)
                            {
                                lastResStateClosed = false;
                                //resDropdown.SetValueWithoutNotify(resDropIndex);
                                ResItem.index = resDropIndex;
                            }
                            if (Input.GetAxisRaw("Xbox Vertical") < 0 && lastVertical >= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") < 0 && lastDpadUp >= 0)
                            {
                                resDropIndex++;
                                if (resDropIndex == numOfReses)
                                    resDropIndex--;
                                //resDropdown.SetValueWithoutNotify(resDropIndex);
                                ResItem.index = resDropIndex;
                            }
                            else if (Input.GetAxisRaw("Xbox Vertical") > 0 && lastVertical <= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") > 0 && lastDpadUp <= 0)
                            {
                                resDropIndex--;
                                if (resDropIndex == -1)
                                    resDropIndex++;
                                //resDropdown.SetValueWithoutNotify(--resDropIndex);
                                ResItem.index = resDropIndex;
                            }
                        }
                        else
                        {
                            resDropdown.Select();
                            lastResStateClosed = true;
                        }
                    }
                    else if (optionsIndex == 1)
                    {
                        if (qualityDropdown.gameObject.transform.childCount > closedQualityChildCount)
                        {
                            if (lastQualStateClosed)
                            {
                                lastQualStateClosed = false;
                                //resDropdown.SetValueWithoutNotify(resDropIndex);
                                QualItem.index = qualityDropIndex;
                            }
                            if (Input.GetAxisRaw("Xbox Vertical") < 0 && lastVertical >= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") < 0 && lastDpadUp >= 0)
                            {
                                qualityDropIndex++;
                                if (qualityDropIndex == 3)
                                    qualityDropIndex--;
                                //resDropdown.SetValueWithoutNotify(resDropIndex);
                                QualItem.index = qualityDropIndex;
                            }
                            else if (Input.GetAxisRaw("Xbox Vertical") > 0 && lastVertical <= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") > 0 && lastDpadUp <= 0)
                            {
                                qualityDropIndex--;
                                if (qualityDropIndex == -1)
                                    qualityDropIndex++;
                                //resDropdown.SetValueWithoutNotify(--resDropIndex);
                                QualItem.index = qualityDropIndex;
                            }
                        }
                        else
                        {
                            qualityDropdown.Select();
                            lastQualStateClosed = true;
                        }

                    }
                    else if (optionsIndex == 2)
                    {
                        mouseSlider.Select();
                        if (Input.GetAxisRaw("Xbox Horizontal") > 0 && lastHorizontal <= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") > 0 && lastDpadLeft <= 0)
                        {
                            mouseSlider.value += 20;
                        }
                        else if (Input.GetAxisRaw("Xbox Horizontal") < 0 && lastHorizontal >= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") < 0 && lastDpadLeft >= 0)
                        {
                            mouseSlider.value -= 20;
                        }
                    }
                    else if (optionsIndex == 3)
                    {
                        contrSlider.Select();
                        if (Input.GetAxisRaw("Xbox Horizontal") > 0 && lastHorizontal <= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") > 0 && lastDpadLeft <= 0)
                        {
                            contrSlider.value += 20;
                        }
                        else if (Input.GetAxisRaw("Xbox Horizontal") < 0 && lastHorizontal >= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") < 0 && lastDpadLeft >= 0)
                        {
                            contrSlider.value -= 20;
                        }
                    }
                    else if (optionsIndex == 4)
                    {
                        masterSlider.Select();
                        if (Input.GetAxisRaw("Xbox Horizontal") > 0 && lastHorizontal <= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") > 0 && lastDpadLeft <= 0)
                        {
                            masterSlider.value += 80 / 10f;
                        }
                        else if (Input.GetAxisRaw("Xbox Horizontal") < 0 && lastHorizontal >= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") < 0 && lastDpadLeft >= 0)
                        {
                            masterSlider.value -= 80 / 10f;
                        }
                    }
                    else if (optionsIndex == 5)
                    {
                        musicSlider.Select();
                        if (Input.GetAxisRaw("Xbox Horizontal") > 0 && lastHorizontal <= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") > 0 && lastDpadLeft <= 0)
                        {
                            musicSlider.value += 80 / 10f;
                        }
                        else if (Input.GetAxisRaw("Xbox Horizontal") < 0 && lastHorizontal >= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") < 0 && lastDpadLeft >= 0)
                        {
                            musicSlider.value -= 80 / 10f;
                        }
                    }
                    else if (optionsIndex == 6)
                    {
                        sfxSlider.Select();
                        if (Input.GetAxisRaw("Xbox Horizontal") > 0 && lastHorizontal <= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") > 0 && lastDpadLeft <= 0)
                        {
                            sfxSlider.value += 80 / 10f;
                        }
                        else if (Input.GetAxisRaw("Xbox Horizontal") < 0 && lastHorizontal >= 0 || Input.GetAxisRaw("Xbox Dpad Left Right") < 0 && lastDpadLeft >= 0)
                        {
                            sfxSlider.value -= 80 / 10f;
                        }
                    }
                    else if (optionsIndex == 7)
                    {
                        backOptionsButton.Select();
                    }
                }
            }
            else if (inQuit)
            {
                if (Input.GetAxisRaw("Xbox Vertical") < 0 && lastVertical >= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") < 0 && lastDpadUp >= 0)
                {
                    quitMenuIndex++;
                    if (quitMenuIndex == 2)
                        quitMenuIndex--;
                }
                else if (Input.GetAxisRaw("Xbox Vertical") > 0 && lastVertical <= 0 || Input.GetAxisRaw("Xbox Dpad Up Down") > 0 && lastDpadUp <= 0)
                {
                    quitMenuIndex--;
                    if (quitMenuIndex == -1)
                        quitMenuIndex++;
                }

                if (quitMenuIndex == 0)
                {
                    yes.Select();
                }
                else
                {
                    no.Select();
                }
            }


            lastDpadUp = Input.GetAxisRaw("Xbox Dpad Up Down");
            lastVertical = Input.GetAxisRaw("Xbox Vertical");
            lastDpadLeft = Input.GetAxisRaw("Xbox Dpad Left Right");
            lastHorizontal = Input.GetAxisRaw("Xbox Horizontal");
        }
        
    }

    public void LateUpdate()
    {
        PlayerPrefs.SetInt("qualityIndex", qualityDropdown.value);
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        mixer.SetFloat("masterVol", masterSlider.value);
        PlayerPrefs.SetFloat("masterVol", masterSlider.value);


        mixer.SetFloat("musicVol", musicSlider.value);
        PlayerPrefs.SetFloat("musicVol", musicSlider.value);


        mixer.SetFloat("sfxVol", masterSlider.value);
        PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);

        PlayerPrefs.SetFloat("controllerSens", contrSlider.value);
        PlayerPrefs.SetFloat("mouseSens", mouseSlider.value);
    }

    public  void updateIndex()
    {
        //for (int i = 0; i < resolutions.Length; i++)
        //{
       //     if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
        //    {
        //        //resDropIndex = i;
        //    }
       // }
    }

    public void Play()
    {
        Cursor.visible = false;
        levelLoader.LoadLevel(1);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
    public void Options()
    {
        Main_Menu.SetActive(false);
        optionsMenu.SetActive(true);

        inMain = false;
        inOptions = true;
    }
    public void backFromOptions()
    {
        Main_Menu.SetActive(true);
        optionsMenu.SetActive(false);

        inMain = true;
        inOptions = false;
    }

    public void backFromQuitConfirm()
    {
        quitMenu.SetActive(false);
        Main_Menu.SetActive(true);

        inMain = true;
        inQuit = false;
    }

    public void QuitConfirm()
    {
        quitMenu.SetActive(true);
        Main_Menu.SetActive(false);

        inMain = false;
        inQuit = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }
}
