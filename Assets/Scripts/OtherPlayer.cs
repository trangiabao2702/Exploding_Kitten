using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class OtherPlayer : NetworkBehaviour, ICardObjectParent
{
    //public static OtherPlayer Instance { get; private set; }
    public static OtherPlayer LocalInstance { get; private set; }

    [SerializeField] private Transform cardsOnHandTransform;
    [SerializeField] private TextMeshProUGUI countCardsOnHandText;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Awake()
    {
        //Instance = this;
    }

    private void Update()
    {
        countCardsOnHandText.text = cardsOnHand.Count.ToString();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            LocalInstance = this;
        }
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
