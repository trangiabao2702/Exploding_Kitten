using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private Color playTurnColor;
    private Color endTurnColor;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#FB7E7D", out playTurnColor);
        ColorUtility.TryParseHtmlString("#A42527", out endTurnColor);
    }

    private void Update()
    {
        if (GameManager.Instance.IsPlayerPlayTurn())
        {
            timerImage.color = playTurnColor;
            timerImage.fillAmount = GameManager.Instance.GetPlayerPlayTurnTimerNormalized();
        }
        else if (GameManager.Instance.IsPlayerEndTurn())
        {
            timerImage.color = endTurnColor;
            timerImage.fillAmount = GameManager.Instance.GetPlayerEndTurnTimerNormalized();
        }
        else { }
    }
}
