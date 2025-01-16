using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private Button drawButton;
    [SerializeField] private TextMeshProUGUI countCards;

    private List<CardObjectSO> cardsOnHand = new List<CardObjectSO>();

    private void Awake()
    {
        Instance = this;

        drawButton.onClick.AddListener(() =>
        {
            cardsOnHand.Add(Deck.Instance.DrawCard());
        });
    }

    private void Update()
    {
        countCards.text = cardsOnHand.Count.ToString();
        foreach (CardObjectSO card in cardsOnHand)
        {
            Debug.Log(card);
        }
    }
}
