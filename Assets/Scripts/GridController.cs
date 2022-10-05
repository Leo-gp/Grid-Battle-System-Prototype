using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour 
{
	[Tooltip("Tile that will be instantiated")]
	public BoardTile tile;
	[Tooltip("Number of tiles on the width of the grid")]
	public int gridWidth;
	[Tooltip("Number of tiles on the length of the grid")]
	public int gridLength;
	[Tooltip("Where the grid starts in world space")]
	public Vector3 firstTilePosition;
	[Tooltip("Offset of the tiles in X")]
	public float tileOffsetX;
	[Tooltip("Offset of the tiles in Y")]
	public float tileOffsetY;
	[Tooltip("Tile normal color")]
	public Color tileNormalColor;
	[Tooltip("Tile normal color when mouse over")]
	public Color tileNormalMouseOverColor;
	[Tooltip("Tile color when it has a selected card")]
	public Color tileSelectedColor;
	[Tooltip("Tile selected color when mouse over")]
	public Color tileSelectedMouseOverColor;
	[Tooltip("Tile color when it is a possible destination of a selected card")]
	public Color tileDestinationColor;
	[Tooltip("Tile destination color when mouse over")]
	public Color tileDestinationMouseOverColor;
	[Tooltip("Tile color when it has a possible target card of a selected card")]
	public Color tileTargetColor;
	[Tooltip("Tile target color when mouse over")]
	public Color tileTargetMouseOverColor;

	public static GridController instance; // Variable that holds this script and can be used in outside scripts

	void Awake ()
	{
		instance = this;
	}

	public void GenerateGrid ()
	{
		for (int x = 0; x < gridWidth; x++)
		{
			// columnFirstTile is the first tile of the current column
			Vector3 columnFirstTile = new Vector3(firstTilePosition.x + tileOffsetX * x, firstTilePosition.y - tileOffsetY * x, transform.position.z);

			for (int y = 0; y < gridLength; y++)
			{
				Vector3 newTilePos = new Vector3(columnFirstTile.x + tileOffsetX * y, columnFirstTile.y + tileOffsetY * y, transform.position.z);
				BoardTile newTile = Instantiate(tile, newTilePos, Quaternion.identity, this.transform);
				newTile.name = "Tile(" + x + "," + y + ")";
				newTile.tilePosition = new Vector2(x, y);
			}
		}
	}
}