using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    private const float timeTurn = 30f;

    [SerializeField] private Image timerImage;

    private float countdownTurn;

    private void Update()
    {
        countdownTurn -= Time.deltaTime;

        if (countdownTurn < 0f)
        {
            ResetCountdownTurn();
        }

        timerImage.fillAmount = countdownTurn / timeTurn;
    }

    private void ResetCountdownTurn()
    {
        countdownTurn = 30f;
    }
}
