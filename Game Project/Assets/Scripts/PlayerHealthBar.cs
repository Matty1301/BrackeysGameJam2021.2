using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private RectTransform rectTransform;
    private Controller playerController;
    private Slider healthBarFill;

    private void Awake()
    {
        playerController = FindObjectOfType<Controller>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(playerController.maxHealth, 20);
        healthBarFill = GetComponent<Slider>();
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
