using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardObjectParent
{
    public Transform GetCardObjectFollowTransform();

    public List<CardObject> GetCardObjectList();

    public void AddCardObject(CardObject cardObject);

    public void RemoveCardObject(CardObject cardObject);
}
