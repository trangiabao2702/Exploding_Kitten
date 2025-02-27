using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum State
    {
        DealingTheCards,
        PlayerPlayTurn,
        PlayerEndTurn,
        PlayerUseCard,
        GameOver,
    }

    private const float playerPlayTurnTimerMax = 45f;
    private const float playerEndTurnTimerMax = 15f;
    private const float playerUseCardTimerMax = 15f;
    private State state;
    private float playerPlayTurnTimer;
    private float playerEndTurnTimer;
    private float playerUseCardTimer;

    private void Awake()
    {
        Instance = this;

        state = State.DealingTheCards;
    }

    private void Start()
    {
        Player.Instance.OnDrawCard += Player_OnDrawCard;
        DrawnCardUI.Instance.OnPlayerEndTurn += DrawnCardUI_OnPlayerEndTurn;
        CardsListUI.Instance.OnShowUI += CardsListUI_OnUseCard;
        CardsListUI.Instance.OnHideUI += CardsListUI_OnHideUI;
    }

    private void Update()
    {
        switch (state)
        {
            case State.DealingTheCards:
                Deck.Instance.InitDeck();

                state = State.PlayerPlayTurn;
                OnStateChanged?.Invoke(this, EventArgs.Empty);

                playerPlayTurnTimer = playerPlayTurnTimerMax;
                break;
            case State.PlayerPlayTurn:
                playerPlayTurnTimer -= Time.deltaTime;
                if (playerPlayTurnTimer < 0f)
                {
                    state = State.PlayerEndTurn;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                    playerEndTurnTimer = playerEndTurnTimerMax;
                }

                break;
            case State.PlayerEndTurn:
                playerEndTurnTimer -= Time.deltaTime;
                if (playerEndTurnTimer < 0f)
                {
                    // Need check if only one player rest -> Change state to GameOver
                    // state = State.GameOver;
                    // OnStateChanged?.Invoke(this, EventArgs.Empty);

                    state = State.PlayerPlayTurn;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                    playerPlayTurnTimer = playerPlayTurnTimerMax;
                }

                break;
            case State.PlayerUseCard:
                playerUseCardTimer -= Time.deltaTime;
                if (playerUseCardTimer < 0f)
                {
                    state = State.PlayerPlayTurn;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.GameOver:
                break;
        }
    }

    private void Player_OnDrawCard(object sender, EventArgs e)
    {
        state = State.PlayerEndTurn;
        OnStateChanged?.Invoke(this, EventArgs.Empty);

        playerEndTurnTimer = playerEndTurnTimerMax;
    }

    private void DrawnCardUI_OnPlayerEndTurn(object sender, EventArgs e)
    {
        state = State.PlayerPlayTurn;
        OnStateChanged?.Invoke(this, EventArgs.Empty);

        playerPlayTurnTimer = playerPlayTurnTimerMax;
    }

    private void CardsListUI_OnUseCard(object sender, EventArgs e)
    {
        state = State.PlayerUseCard;
        OnStateChanged?.Invoke(this, EventArgs.Empty);

        playerUseCardTimer = playerUseCardTimerMax;
    }

    private void CardsListUI_OnHideUI(object sender, EventArgs e)
    {
        if (state != State.PlayerUseCard)
        {
            return;
        }

        state = State.PlayerPlayTurn;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetNumberOfPlayers()
    {
        return 2;
    }

    public List<ICardObjectParent> GetPlayers()
    {
        List<ICardObjectParent> players = new List<ICardObjectParent>();

        players.Add(Player.Instance);
        players.Add(OtherPlayer.Instance);
        
        return players;
    }

    public bool IsDealingTheCards()
    {
        return state == State.DealingTheCards;
    }

    public bool IsPlayerPlayTurn()
    {
        return state == State.PlayerPlayTurn;
    }

    public bool IsPlayerEndTurn()
    {
        return state == State.PlayerEndTurn;
    }

    public bool IsPlayerUseCard()
    {
        return state == State.PlayerUseCard;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetPlayerPlayTurnTimerNormalized()
    {
        return playerPlayTurnTimer / playerPlayTurnTimerMax;
    }

    public float GetPlayerEndTurnTimerNormalized()
    {
        return playerEndTurnTimer / playerEndTurnTimerMax;
    }

    public float GetPlayerUseCardTimerNormalized()
    {
        return playerUseCardTimer / playerUseCardTimerMax;
    }
}
