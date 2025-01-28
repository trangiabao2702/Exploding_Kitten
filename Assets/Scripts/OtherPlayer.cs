using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public OtherPlayer Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI countCardsOnHandText;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Update()
    {
        countCardsOnHandText.text = cardsOnHand.Count.ToString();
    }

    public List<CardObject> GetCardsOnHand()
    {
        return cardsOnHand;
    }
}
