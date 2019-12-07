using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBarController : MonoBehaviour
{
    private Slider healthbar;
    public static int currentHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public static void RemoveHealth()
    {
        if(Salty_Rusty_Controller.isSalty)
        {
            CanvasData.saltyAnim.SetBool("flinch", true);
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
