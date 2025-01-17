using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField] private CardObjectSO cardObjectSO;

    private ICardObjectParent cardObjectParent;

    public CardObjectSO GetCardObjectSO() { return cardObjectSO; }

    public ICardObjectParent GetCardObjectParent()
    {
        return cardObjectParent;
    }

    public void SetCardObjectParent(ICardObjectParent cardObjectParent)
    {
        this.cardObjectParent = cardObjectParent;

        cardObjectParent.AddCardObject(this);

        this.transform.SetParent(cardObjectParent.GetCardObjectFollowTransform());
    }

    public static CardObject SpawnCardObject(CardObjectSO cardObjectSO, ICardObjectParent cardObjectParent)
    {
        Transform cardObjectTransform = Instantiate(cardObjectSO.prefab);

        CardObject cardObject = cardObjectTransform.GetComponent<CardObject>();
        cardObject.SetCardObjectParent(cardObjectParent);

        return cardObject;
    }
}
