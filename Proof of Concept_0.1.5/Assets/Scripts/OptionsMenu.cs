using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Dropdown resolutionDropDown;

    private Resolution[] resolutions;

    // The most complicated part of the entire code. For screen resolutions, each PC or hardware has their own set of defaulted resolutions.
    private void Start()
    {
        Screen.fullScreen = false;
        int i;

        // Takes the array of the resolution and then clears it out so that new resolutions depending on computer hardware can be used.
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        int currentResolutionIndex = 0;

        // Creates a linked list algorithm that goes through the length of the resolutions that the hardware has and adds the width and height on to the dropdown menu.
        List<string> options = new List<string>();
        for (i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        // Takes in all the information regarding the PC hardware settings from the Start function and updates it for the player when chosen a specific resolution in game.
        Resolution newResolution = resolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        Debug.Log(newResolution.width);
        Debug.Log(newResolution.height);
    }

    public void SetVolume (float volume)
    {
        // name this the same way as you name the audio parameter under the audio mixer. Ensure when adding script that when On Click is added to click on the dynamic int option (topmost option).
        // Dynamic int - sets the amount based on the UI interaction with player, I.E. if the player slides the volume slider to 0 the game updates itself to state that no audio will be used.

        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        // This code takes in the quality option selected in the drop down selection in game. Ensure when adding script that when On Click is added to click on the dynamic int option (topmost option).
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        Debug.Log(isFullScreen);
    }
}
