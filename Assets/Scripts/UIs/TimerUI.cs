using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private void Update()
    {
        if (GameManager.Instance.IsPlayerPlayTurn())
        {
            timerImage.fillAmount = GameManager.Instance.GetPlayerPlayTurnTimerNormalized();
        }
        else if (GameManager.Instance.IsPlayerEndTurn())
        {
            timerImage.fillAmount = GameManager.Instance.GetPlayerEndTurnTimerNormalized();
        }
        else { }
    }
}
