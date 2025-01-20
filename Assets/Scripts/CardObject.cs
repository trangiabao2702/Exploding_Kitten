using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IPointerClickHandler
{
    public enum CardType
    {
        Attack,
        Cat,
        Defuse,
        ExplodingKitten,
        Favor,
        Nope,
        SeeTheFuture,
        Shuffle,
        Skip,
    }

    [SerializeField] private CardObjectSO cardObjectSO;
    [SerializeField] private CardType cardType;

    private ICardObjectParent cardObjectParent;
    private bool isSelected = false;

    public CardObjectSO GetCardObjectSO() { return cardObjectSO; }

    public ICardObjectParent GetCardObjectParent()
    {
        return cardObjectParent;
    }

    public void SetCardObjectParent(ICardObjectParent cardObjectParent)
    {
        this.cardObjectParent = cardObjectParent;

        cardObjectParent.AddCardObject(this);

        Transform cardObjectParentFollowTransform = cardObjectParent.GetCardObjectFollowTransform();
        this.transform.SetParent(cardObjectParentFollowTransform);
        this.transform.position = cardObjectParentFollowTransform.position;
    }

    public static CardObject SpawnCardObject(CardObjectSO cardObjectSO, ICardObjectParent cardObjectParent)
    {
        Transform cardObjectTransform = Instantiate(cardObjectSO.prefab);

        CardObject cardObject = cardObjectTransform.GetComponent<CardObject>();
        cardObject.SetCardObjectParent(cardObjectParent);

        return cardObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player.Instance.HasCardOnHand(this))
        {
            if (isSelected)
            {
                MoveDown();
            }
            else
            {
                MoveUp();
            }
        }
    }

    private void MoveUp()
    {
        transform.position = transform.position + Vector3.up * .5f;
        isSelected = true;
    }

    private void MoveDown()
    {
        transform.position = transform.position - Vector3.up * .5f;
        isSelected = false;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public CardType GetCardType()
    {
        return cardType;
    }
}
