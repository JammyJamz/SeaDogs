using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static void RemoveHealth()
    {
        currentHealth--;
    }

    public static void AddHealth()
    {
        currentHealth++;
    }
}
