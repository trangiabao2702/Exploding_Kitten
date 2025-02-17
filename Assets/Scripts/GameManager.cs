using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        WaitingToDealTheCards,
        GamePlaying,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.WaitingToDealTheCards:
                Deck.Instance.InitDeck();

                state.Value = State.GamePlaying;

                break;
            case State.GamePlaying:
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    }

    public bool IsWaitingToDealTheCards()
    {
        return state.Value == State.WaitingToDealTheCards;
    }

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public int GetNumberOfPlayers()
    {
        return 2;
    }

    public List<ICardObjectParent> GetPlayers()
    {
        List<ICardObjectParent> players = new List<ICardObjectParent>();

        players.Add(Player.LocalInstance);
        players.Add(OtherPlayer.LocalInstance);
        
        return players;
    }
}
