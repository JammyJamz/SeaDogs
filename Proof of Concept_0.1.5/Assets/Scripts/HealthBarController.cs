using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthbar;
    private int currentHealth = 3;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = currentHealth;
        if (Input.GetKeyDown(KeyCode.T))
        {
            currentHealth = currentHealth - 1; ;
        }
    }
}
