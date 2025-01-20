using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ICardObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private Transform cardsOnHandTransform;
    [SerializeField] private Button drawButton;
    [SerializeField] private Button playButton;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Awake()
    {
        Instance = this;

        drawButton.onClick.AddListener(() =>
        {
            CardObject newCardObject = Deck.Instance.DrawCard();

            newCardObject.SetCardObjectParent(this);
        });
        playButton.onClick.AddListener(() =>
        {
            
        });
    }

    private void Update()
    {

    }

    public Transform GetCardObjectFollowTransform()
    {
        return cardsOnHandTransform;
    }

    public List<CardObject> GetCardObjectList()
    {
        return cardsOnHand;
    }

    public void AddCardObject(CardObject cardObject)
    {
        cardsOnHand.Add(cardObject);
    }

    public void RemoveCardObject(CardObject cardObject)
    {
        cardsOnHand.Remove(cardObject);
    }

    public bool HasCardOnHand(CardObject cardObject)
    {
        return cardsOnHand.Contains(cardObject);
    }
}
