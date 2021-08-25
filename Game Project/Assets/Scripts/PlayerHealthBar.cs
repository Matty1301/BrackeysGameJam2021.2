using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private PlayerController playerController;
    private Slider healthBarFill;

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        healthBarFill = GetComponent<Slider>();
    }

    void Start()
    {
        healthBarFill.value = 1;
    }

    void Update()
    {
        if (playerController != null)
        {
            healthBarFill.value = (float)playerController.health / playerController.maxHealth;
        }
    }
}
