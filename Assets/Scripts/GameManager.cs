using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
}
