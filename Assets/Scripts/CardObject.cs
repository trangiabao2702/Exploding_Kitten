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
    [SerializeField] private Transform selected;

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

        bool canSetCardSelected = selected.gameObject.activeSelf ? CardsListUI.Instance.UnlockSelectCard() == 0 : CardsListUI.Instance.LockSelectCard() > 0;
        AdjustCardSelected(canSetCardSelected);
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

    public void SetCardSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

    public CardType GetCardType()
    {
        return cardType;
    }

    public void AdjustCardSelected(bool canSetCardSelected)
    {
        // To change selected state in cards list ui
        selected.gameObject.SetActive(canSetCardSelected);
        CardsListUI.Instance.AdjustSelectCardObjects(this, canSetCardSelected);
    }

    public void PlayCard()
    {
        switch (cardType)
        {
            case CardType.SeeTheFuture:
                SeeTheFuture();
                break;
            case CardType.Shuffle:
                Shuffle();
                break;
            default:
                break;
        }
    }

    // ------ Pack 1: Original Edition ------
    private void SeeTheFuture(int numberOfCards = 3)
    {
        List<CardObject> cardsInDeck = Deck.Instance.GetCardObjectList();
        int numberToShow = Mathf.Min(numberOfCards, cardsInDeck.Count);
        CardsListUI.Instance.Show(cardsInDeck.GetRange(0, numberToShow));
    }

    private void Shuffle()
    {
        Deck.Instance.ShuffleDeck();
    }
}
