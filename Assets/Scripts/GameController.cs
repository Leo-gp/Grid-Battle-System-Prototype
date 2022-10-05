using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
	[Tooltip("Fill with the Player1 tag (MUST BE the same as the Player1 gameobject tag)")]
	public string player1Tag;
	[Tooltip("Fill with the Player2 tag (MUST BE the same as the Player2 gameobject tag)")]
	public string player2Tag;
	[Tooltip("Starting X and Y position of the Leader 1 on the board")]
	public Vector2 leader1StartPosition;
	[Tooltip("Starting X and Y position of the Leader 2 on the board")]
	public Vector2 leader2StartPosition;
	[Tooltip("Prefab with the attack icon and text to display the attack value")]
	public GameObject attackIcon;
	[Tooltip("Prefab with the health icon and text to display the health value")]
	public GameObject healthIcon;
	[Tooltip("Position that the attack icon is IN RELATION TO THE CARD ON THE BOARD")]
	public Vector3 attackIconPosition;
	[Tooltip("Position that the health icon is IN RELATION TO THE CARD ON THE BOARD")]
	public Vector3 healthIconPosition;
	[Tooltip("Number of cards each player draws at the start of the game")]
	public int startingHandCardsAmount;
	[Tooltip("Maximum number of cards each player can have in hand")]
	public int MaxHandCardsAmount;
	[Tooltip("Center position of the player's hand")]
	public Vector3 handCardsPosition;
	[Tooltip("Offset in X position of the hand cards")]
	public float handCardsOffset;
	[Tooltip("Scale of the hand cards")]
	public Vector2 handCardsScale;

	[HideInInspector] public Player player1; // Player 1
	[HideInInspector] public Player player2; // Player 2
	[HideInInspector] public BoardTile[] tiles; // All tiles of the board
	[HideInInspector] public Card selectedCard; // Current selected card
	[HideInInspector] public List<BoardTile> possibleDestinationsTiles; // Tiles that can be destinations from the selected card
	[HideInInspector] public List<Card> possibleTargetCards; // Cards that can be the target of the selected card

	private Card player1Leader; // Player 1 Leader card
	private Card player2Leader; // Player 2 Leader Card

	public static GameController instance; // Variable that holds this script and can be used in outside scripts

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		GridController.instance.GenerateGrid(); // Generate the game grid

		tiles = FindObjectsOfType<BoardTile>(); // Fill tiles list

		possibleDestinationsTiles = new List<BoardTile>();
		possibleTargetCards = new List<Card>();

		SetPlayersAndCards();

		SummonCard(player1Leader, GetTile(leader1StartPosition));
		SummonCard(player2Leader, GetTile(leader2StartPosition));
	}

	void SummonCard (Card cardToBeSummoned, BoardTile targetTile) // Summon a card on the board at the targetTile position
	{
		Card card = Instantiate(cardToBeSummoned, targetTile.transform);
		card.transform.position = targetTile.transform.position;

		GameObject atkIcon = Instantiate(attackIcon, card.transform);
		atkIcon.transform.position = card.transform.position + new Vector3(attackIconPosition.x, attackIconPosition.y, attackIconPosition.z);

		GameObject hlthIcon = Instantiate(healthIcon, card.transform);
		hlthIcon.transform.position = card.transform.position + new Vector3(healthIconPosition.x, healthIconPosition.y, healthIconPosition.z);

		Text atkTxt = atkIcon.GetComponentInChildren<Text>();
		card.attackValueText = atkTxt;
		card.attackValueText.text = card.attackValue.ToString();

		Text hlthTxt = hlthIcon.GetComponentInChildren<Text>();
		card.healthValueText = hlthTxt;
		card.healthValueText.text = card.healthValue.ToString();
	}

	public BoardTile GetTile (Vector2 tileToGetPosition) // Get a tile from its position on the board
	{
		foreach (BoardTile tile in tiles)
		{
			if (tile.tilePosition == tileToGetPosition)
			{
				return tile;
			}
		}
		throw new UnityException("The tile (" + tileToGetPosition.x + "," + tileToGetPosition.y + ") does not exist!");
	}

	public void HighlightTile (BoardTile tile) // Change a tile color to its highlighted color
	{
		if (tile.rend.color == GridController.instance.tileNormalColor)
			tile.rend.color = GridController.instance.tileNormalMouseOverColor;
		
		else if (tile.rend.color == GridController.instance.tileSelectedColor)
			tile.rend.color = GridController.instance.tileSelectedMouseOverColor;
		
		else if (tile.rend.color == GridController.instance.tileDestinationColor)
			tile.rend.color = GridController.instance.tileDestinationMouseOverColor;
		
		else if (tile.rend.color == GridController.instance.tileTargetColor)
			tile.rend.color = GridController.instance.tileTargetMouseOverColor;
	}

	public void UnhighlightTile (BoardTile tile) // Change a tile color to its unhighlighted color
	{
		if (tile.rend.color == GridController.instance.tileNormalMouseOverColor)
			tile.rend.color = GridController.instance.tileNormalColor;

		else if (tile.rend.color == GridController.instance.tileSelectedMouseOverColor)
			tile.rend.color = GridController.instance.tileSelectedColor;

		else if (tile.rend.color == GridController.instance.tileDestinationMouseOverColor)
			tile.rend.color = GridController.instance.tileDestinationColor;

		else if (tile.rend.color == GridController.instance.tileTargetMouseOverColor)
			tile.rend.color = GridController.instance.tileTargetColor;
	}

	public void HighlightDestinations (Card card) // Highlight (change color) tiles that are possible destinations of a card
	{
		if (card != null)
		{
			foreach (BoardTile tile in tiles)
			{
				if (TileCanBeDestination(card, tile))
				{
					tile.possibleDestination = true;
					possibleDestinationsTiles.Add(tile);
					tile.rend.color = GridController.instance.tileDestinationColor;
				}
			}
		}
	}

	public void HighlightTargets (Card card) // Highlight (change color) tiles that have possible target cards of a card
	{
		if (card != null)
		{
			foreach (BoardTile tile in tiles)
			{
				if (tile.transform.childCount > 0)
				{
					Card targetCard = tile.transform.GetComponentInChildren<Card>();
					if (CardCanBeTarget(card, targetCard))
					{
						targetCard.possibleTarget = true;
						possibleTargetCards.Add(targetCard);
						tile.rend.color = GridController.instance.tileTargetColor;
					}
				}
			}
		}
	}
		
	public void ClearSelectionsAndHighlights () // Clear all the selected cards, lists and highlighted tiles
	{
		selectedCard.GetComponentInParent<BoardTile>().rend.color = GridController.instance.tileNormalColor;

		foreach (BoardTile tile in possibleDestinationsTiles)
		{
			tile.rend.color = GridController.instance.tileNormalColor;
			tile.possibleDestination = false;
		}

		foreach (Card targetCard in possibleTargetCards)
		{
			targetCard.GetComponentInParent<BoardTile>().rend.color = GridController.instance.tileNormalColor;
		}

		selectedCard = null;
		possibleDestinationsTiles.Clear();
		possibleTargetCards.Clear();
	}
		
	public bool TileCanBeDestination (Card movingCard, BoardTile targetTile) // Check if a targetTile can be a possible destination of a movingCard
	{
		if (movingCard != null && targetTile != null) // If the movingCard and the targetTile exist
		{
			if (targetTile.transform.childCount > 0) // If targetTile has a child (card), return false (it will not be a destination)
			{
				return false;
			}
			// Else, if targetTile has not a child (card) and is within movingCard's move range, return true (it will be a destination)
			else if (IsWithinMoveRange(movingCard, targetTile)) 
			{
				return true;
			}
		}
		return false;
	}

	public bool CardCanBeTarget (Card attackingCard, Card targetCard) // Check if a targetCard can be a possible target of an attackingCard
	{
		if (attackingCard != null && targetCard != null) // If the cards exist
		{
			// If attackingCard and targetCard have the same tag (same team), return false (it cannot be a target)
			if (attackingCard.tag == targetCard.tag)
			{
				return false;
			}
			// Else, if attackingCard and targetCard have different tags (not the same team)
			// and targetCard is within attackingCard's attack range, return true (it will be a possible target)
			else if (IsWithinAttackRange(attackingCard, targetCard))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsWithinMoveRange (Card movingCard, BoardTile targetTile) // Check if a targetTile is within a movingCard's move range
	{
		Vector2 cardPosition = movingCard.GetComponentInParent<BoardTile>().tilePosition;

		for (int i = 1; i < movingCard.yMoveRange + 1; i++)
		{
			if (targetTile.tilePosition.x == cardPosition.x && (targetTile.tilePosition.y == cardPosition.y + i
				|| targetTile.tilePosition.y == cardPosition.y - i))
			{
				return true;
			}
		}

		for (int j = 1; j < movingCard.xMoveRange + 1; j++)
		{
			if (targetTile.tilePosition.y == cardPosition.y && (targetTile.tilePosition.x == cardPosition.x + j 
				|| targetTile.tilePosition.x == cardPosition.x - j))
			{
				return true;
			}
		}

		for (int k = 1; k < movingCard.xyMoveRange + 1; k++)
		{
			if ((targetTile.tilePosition.y == cardPosition.y + k || targetTile.tilePosition.y == cardPosition.y - k)
				&& (targetTile.tilePosition.x == cardPosition.x + k || targetTile.tilePosition.x == cardPosition.x - k))
			{
				return true;
			}
		}

		if (movingCard.canLMove)
		{
			if (((targetTile.tilePosition.y == cardPosition.y + 2 || targetTile.tilePosition.y == cardPosition.y - 2)
				  && (targetTile.tilePosition.x == cardPosition.x + 1 || targetTile.tilePosition.x == cardPosition.x - 1))
			    || ((targetTile.tilePosition.y == cardPosition.y + 1 || targetTile.tilePosition.y == cardPosition.y - 1)
				  && (targetTile.tilePosition.x == cardPosition.x + 2 || targetTile.tilePosition.x == cardPosition.x - 2)))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsWithinAttackRange (Card attackingCard, Card targetCard) // Check if a targetCard is within an attackingCard's attack range
	{
		Vector2 attackingCardPosition = attackingCard.GetComponentInParent<BoardTile>().tilePosition;
		Vector2 targetCardPosition = targetCard.GetComponentInParent<BoardTile>().tilePosition;

		for (int i = 1; i < attackingCard.yAttackRange + 1; i++)
		{
			if (targetCardPosition.x == attackingCardPosition.x && (targetCardPosition.y == attackingCardPosition.y + i
				|| targetCardPosition.y == attackingCardPosition.y - i))
			{
				return true;
			}
		}

		for (int j = 1; j < attackingCard.xAttackRange + 1; j++)
		{
			if (targetCardPosition.y == attackingCardPosition.y && (targetCardPosition.x == attackingCardPosition.x + j
				|| targetCardPosition.x == attackingCardPosition.x - j))
			{
				return true;
			}
		}

		for (int k = 1; k < attackingCard.xyAttackRange + 1; k++)
		{
			if ((targetCardPosition.y == attackingCardPosition.y + k || targetCardPosition.y == attackingCardPosition.y - k)
				&& (targetCardPosition.x == attackingCardPosition.x + k || targetCardPosition.x == attackingCardPosition.x - k))
			{
				return true;
			}
		}

		if (attackingCard.canLAttack)
		{
			if (((targetCardPosition.y == attackingCardPosition.y + 2 || targetCardPosition.y == attackingCardPosition.y - 2)
				  && (targetCardPosition.x == attackingCardPosition.x + 1 || targetCardPosition.x == attackingCardPosition.x - 1))
				|| ((targetCardPosition.y == attackingCardPosition.y + 1 || targetCardPosition.y == attackingCardPosition.y - 1)
				  && (targetCardPosition.x == attackingCardPosition.x + 2 || targetCardPosition.x == attackingCardPosition.x - 2)))
			{
				return true;
			}
		}
		return false;
	}

	public void EngageBattle (Card card1, Card card2) // Two cards engage a battle
	{
		card1.currentHealth -= card2.currentAttack; // card1 loses hp equal to the card2's attack
		card2.currentHealth -= card1.currentAttack; // // card2 loses hp equal to the card1's attack

		card1.healthValueText.text = card1.currentHealth.ToString(); // card1's health icon update its value
		card2.healthValueText.text = card2.currentHealth.ToString(); // card2's health icon update its value

		if (card1.currentHealth <= 0) // If card1 has 0 or less of health, kill it
		{
			Die(card1);
		}
		if (card2.currentHealth <= 0) // If card2 has 0 or less of health, kill it
		{
			Die(card2);
		}
	}

	void Die (Card cardToDie) // Make a card die (be destroyed)
	{
		Destroy(cardToDie.gameObject);
	}

	void ShuffleCards (List<Card> listToShuffle) // Shuffle a list of cards
	{
		List<Card> newList = new List<Card>();
		int iterations = listToShuffle.Count;
		for (int i = 0; i < iterations; i++)
		{
			Card randomCard = listToShuffle[Random.Range(0, listToShuffle.Count)];
			listToShuffle.Remove(randomCard);
			newList.Add(randomCard);
		}
		foreach (Card card in newList)
		{
			listToShuffle.Add(card);
		}
	}

	void SetPlayersAndCards () // Set players and cards at the start of the game
	{
		Player[] players = GameObject.FindObjectsOfType<Player>();

		foreach (Player player in players)
		{
			if (player.tag == player1Tag)
			{
				player1 = player;
			}
			else if (player.tag == player2Tag)
			{
				player2 = player;
			}
			else
			{
				throw new UnityException("Player not found! Check the players tags in GameController.");
			}
		}

		foreach (Card card in player1.deckCards)
		{
			card.tag = player1.tag;

			if (card.cardType == Card.CardType.Leader)
			{
				player1Leader = card;
			}
		}
		player1.deckCards.Remove(player1Leader);
		ShuffleCards(player1.deckCards);
		for (int i = 0; i < startingHandCardsAmount; i++)
		{
			DrawCard(player1);
		}

		foreach (Card card in player2.deckCards)
		{
			card.tag = player2.tag;

			if (card.cardType == Card.CardType.Leader)
			{
				player2Leader = card;
			}
		}
		player2.deckCards.Remove(player2Leader);
		ShuffleCards(player2.deckCards);
		for (int i = 0; i < startingHandCardsAmount; i++)
		{
			DrawCard(player2);
		}
	}

    public void DrawCard (Player player) // A player draws a card from his deckCards
	{
		if (player.deckCards.Count > 0 && player.handCards.Count < MaxHandCardsAmount)
        {
            Card drawCard = player.deckCards[0];
            player.deckCards.Remove(drawCard);
            player.handCards.Add(drawCard);
            if (player == player1)
            {
                Hand playerHand = player.GetComponentInChildren<Hand>();
                drawCard = Instantiate(drawCard, playerHand.transform);
                drawCard.GetComponent<SpriteRenderer>().sprite = drawCard.cardSprite;
                playerHand.OrganizeHand(player);
            }
        }
	}
}