using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour 
{
	public void OrganizeHand (Player player)
	{
		Transform hand = player.GetComponentInChildren<Hand>().transform;

		Vector3 cardPosition = GameController.instance.handCardsPosition;
		float offset = GameController.instance.handCardsOffset;
		Vector2 cardScale = GameController.instance.handCardsScale;

		for (int i = 0; i < hand.childCount; i++)
		{
			Transform card = hand.GetChild(i);
			card.position = new Vector3(cardPosition.x + offset * i, cardPosition.y, cardPosition.z);
			card.localScale = new Vector3 (cardScale.x, cardScale.y, 1);
		}
	}
}