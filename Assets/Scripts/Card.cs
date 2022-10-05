using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour 
{
	public enum CardType {Minion, Leader}; // Card types

	[Tooltip("Sprite of the card")]
	public Sprite cardSprite;
	[Tooltip("Sprite of the card on the grid")]
	public Sprite cardOnGridSprite;
	[Tooltip("Type of the card")]
	public CardType cardType;
	[Tooltip("Distance of tiles that this card can move to the right or left")]
	public int xMoveRange;
	[Tooltip("Distance of tiles that this card can move up or down")]
	public int yMoveRange;
	[Tooltip("Distance of tiles that this card can move diagonally")]
	public int xyMoveRange;
	[Tooltip("Distance from the target card that this card can attack to the right or left")]
	public int xAttackRange;
	[Tooltip("Distance from the target card that this card can attack up or down")]
	public int yAttackRange;
	[Tooltip("Distance from the target card that this card can attack diagonally")]
	public int xyAttackRange;
	[Tooltip("Allow the card to move in 'L' directions")]
	public bool canLMove;
	[Tooltip("Allow the card to attack in 'L' directions")]
	public bool canLAttack;
	[Tooltip("Default attack value of the card")]
	public int attackValue;
	[Tooltip("Default health value of the card")]
	public int healthValue;
	[Tooltip("Default mana cost of the card")]
	public int manaCost;

	[HideInInspector] public int currentAttack; // Current attack value of the card
	[HideInInspector] public int currentHealth; // Current health value of the card
	[HideInInspector] public int currentManaCost; // Current mana cost of the card

	[HideInInspector] public Text attackValueText; // Text that displays the attack value of the card in the game
	[HideInInspector] public Text healthValueText; // Text that displays the health value of the card in the game
	[HideInInspector] public Text manaCostText; // Text that displays the mana cost of the card in the game

	[HideInInspector] public bool possibleTarget; // True if the card is a possible target of another one

	void Start ()
	{
		currentAttack = attackValue;
		currentHealth = healthValue;
	}
}