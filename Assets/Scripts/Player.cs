using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public List<Card> deckCards = new List<Card>(); // Cards on the deck
	public List<Card> handCards = new List<Card>(); // Cards on the hand
	public List<Card> gridCards = new List<Card>(); // Cards on the grid
}