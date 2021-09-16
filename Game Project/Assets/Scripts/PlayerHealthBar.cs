using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private RectTransform rectTransform;
    private Controller playerController;
    private Slider healthBarFill;

    private void OnEnable()
    {
        RoomTemplates.playerSpawned += Initialize;
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<Controller>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3((float)playerController.maxHealth / 300, 1, 1);
        healthBarFill = GetComponent<Slider>();
        healthBarFill.maxValue = playerController.maxHealth;
        healthBarFill.value = healthBarFill.maxValue;
    }

    void Update()
    {
        if (playerController != null)
        {
            healthBarFill.value = playerController.health;
        }
    }

    private void OnDisable()
    {
        RoomTemplates.playerSpawned -= Initialize;
    }
}
