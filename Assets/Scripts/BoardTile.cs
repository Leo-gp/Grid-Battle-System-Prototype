using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour 
{
	[HideInInspector] public Vector2 tilePosition; // Tile position on the board
	[HideInInspector] public bool possibleDestination; // Check if the tile is a possible destination
	[HideInInspector] public SpriteRenderer rend; // Tile renderer

	void Start ()
	{
		rend = GetComponent<SpriteRenderer>();
		rend.color = GridController.instance.tileNormalColor;
	}

	void OnMouseEnter ()
	{
		GameController.instance.HighlightTile(this);
	}

	void OnMouseExit ()
	{
		GameController.instance.UnhighlightTile(this);
	}

	void OnMouseDown ()
	{
		if (transform.childCount > 0) // If this tile has a child (card)
		{
			Card clickedCard = transform.GetComponentInChildren<Card>(); // The current child (card) of this tile (that was just clicked)
			Card selectedCard = GameController.instance.selectedCard; // Get the selected card (if there is one)

			if (clickedCard == selectedCard) // If the just clickedCard is the same as the already selected one, clear selections and highlights
			{
				GameController.instance.ClearSelectionsAndHighlights();
			}
			else if (GameController.instance.selectedCard == null) // Else, if there was not a selected card, select the clickedCard and highlight possible destinations and targets
			{
				GameController.instance.selectedCard = clickedCard; // GameController gets the clickedCard to be selected
				rend.color = GridController.instance.tileSelectedColor; // Tile changes its color to selectedColor
				GameController.instance.HighlightDestinations(clickedCard); // Possible destination tiles of the clickedCard are highlighted
				GameController.instance.HighlightTargets(clickedCard); // Possible target cards of the clickedCard are highlighted
			}
			else if (GameController.instance.IsWithinAttackRange(selectedCard, clickedCard)) // Else, if there was a selected card, attack the clickedCard (if enemy)
			{
				if (selectedCard.tag != clickedCard.tag) // If they have different tags, they are enemies
				{
					GameController.instance.EngageBattle(selectedCard, clickedCard); // Battle selectedCard with the clickedCard
					GameController.instance.ClearSelectionsAndHighlights();
				}
			}
		}
		// Else, if this tile has not a child (card) and selectedCard exists and this tile is a possible destination of it, move the selectedCard to it
		else if (possibleDestination)
		{
			Card selectedCard = GameController.instance.selectedCard;
			if (selectedCard != null)
			{
				GameController.instance.ClearSelectionsAndHighlights();
				selectedCard.transform.SetParent(this.transform); // selectedCard change its parent
				selectedCard.transform.position = this.transform.position; // selectedCard gets the same position as its parent
			}
		}
	}
}