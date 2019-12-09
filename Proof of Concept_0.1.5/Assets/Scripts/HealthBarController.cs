using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class HealthBarController : MonoBehaviour
{
    private Slider healthbar;
    public static int currentHealth = 3;

    public AudioSource saltyFlinch;
    public AudioSource rustyFlinch;
    public static AudioSource rustyFlinch1;
    public static AudioSource saltyFlinch1;

    // Start is called before the first frame update
    void Start()
    {
        rustyFlinch1 = rustyFlinch;
        saltyFlinch1 = saltyFlinch;
        healthbar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = currentHealth;
    }

    void LateUpdate()
    {
        if (CanvasData.saltyAnim.GetBool("flinch"))
        {
            CanvasData.saltyAnim.SetBool("flinch", false);
        }
        if (CanvasData.rustyAnim.GetBool("flinch"))
        {
            CanvasData.rustyAnim.SetBool("flinch", false);
        }
    }

    public static void RemoveHealth()
    {
        if (Salty_Rusty_Controller.isSalty)
        {
            CanvasData.saltyAnim.SetBool("flinch", true);
            saltyFlinch1.Stop();
            saltyFlinch1.Play();
        }
        else
        {
            CanvasData.rustyAnim.SetBool("flinch", true);
            rustyFlinch1.Stop();
            rustyFlinch1.Play();
        }
        currentHealth--;
        if(currentHealth <= 0)
        {
            CanvasData.levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public static void AddHealth()
    {
        if(currentHealth < 3)
            currentHealth++;
    }
}
