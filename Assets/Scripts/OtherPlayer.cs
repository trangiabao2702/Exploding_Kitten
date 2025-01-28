using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherPlayer : MonoBehaviour, ICardObjectParent
{
    public static OtherPlayer Instance { get; private set; }

    [SerializeField] private Transform cardsOnHandTransform;
    [SerializeField] private TextMeshProUGUI countCardsOnHandText;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        countCardsOnHandText.text = cardsOnHand.Count.ToString();
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
}
