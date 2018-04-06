using UnityEngine;
using System;
using SimpleJSON;

public class MapLoader : MonoBehaviour {
	public GameObject tilePrefab;
	public GameObject cliffPrefab;
	public GameObject wallPrefab;
	public GameObject startPosPrefab;
	public GameObject backgroundPrefab;
	private enum _tileType{
		VOID=0, NORMAL, STARTPOS
	};

	// Use this for initialization
	void Start () {
		TextAsset mapText = Resources.Load ("Maps/map_test3") as TextAsset;
		JSONNode mapInfo = JSON.Parse (mapText.text);
		MakeMap(mapInfo);
	}

	void MakeMap (JSONNode mapInfo) {
		MakeTiles (mapInfo);
		MakeBackground (mapInfo);
	}

	void MakeTiles(JSONNode mapInfo) {
		int mapWidth = mapInfo ["width"].AsInt;
		int mapHeight = mapInfo ["height"].AsInt;
		JSONNode tileInfo = mapInfo ["layers"][0]["data"];

		string tileSetName = mapInfo ["tileset"].Value;
		Sprite[] tileSet = Resources.LoadAll<Sprite> ("Sprites/Tiles/Tile" + tileSetName);
		Sprite cliffSprite = Resources.Load<Sprite> ("Sprites/Tiles/TileCliff");

		float viewHeight = 2f * Camera.main.orthographicSize;
		float viewWidth = viewHeight * Camera.main.aspect;
		float tileWidth = tileSet [0].bounds.size.x;
		float tileHeight = tileSet [0].bounds.size.y;

		for (int column = 0; column < mapWidth; column += 1) {
			bool isTileExistInColumn = false;
			for (int row = 0; row < mapHeight; row += 1) {
				int tileIndex = mapWidth * row + column;
				float tilePosX = -(viewWidth / 2) + column * tileWidth + tileWidth / 2;
				float tilePosY = -(viewHeight / 2) + (mapHeight - row) * (tileHeight - 11 / tileSet [0].pixelsPerUnit) - tileHeight / 2;
				int tileSpriteIndex = UnityEngine.Random.Range (0, tileSet.Length);
				switch (tileInfo [tileIndex].AsInt) {
				case (int)_tileType.VOID:
					if (isTileExistInColumn) {
						MakeTileElement (cliffPrefab, cliffSprite, new Vector2 (tilePosX, tilePosY), row - 1);
					}
					if (checkShouldBlock (tileInfo, row, column, mapWidth, mapHeight)) {
						MakeWallMask(wallPrefab, new Vector2(tilePosX, tilePosY), tileWidth, tileHeight);
					}
					break;
				case (int)_tileType.NORMAL:
					isTileExistInColumn = true;
					float ydiff = UnityEngine.Random.Range (-4, 4) / tileSet [0].pixelsPerUnit;
					MakeTileElement (tilePrefab, tileSet [tileSpriteIndex], new Vector2 (tilePosX, tilePosY + ydiff), row);
					break;
				case (int)_tileType.STARTPOS:
					MakeTileElement (tilePrefab, tileSet [tileSpriteIndex], new Vector2 (tilePosX, tilePosY), row);
					MakeStartPos (new Vector3(tilePosX + tileWidth / 2, tilePosY + tileHeight / 2, -1));
					break;
				default:
					throw new IndexOutOfRangeException ("Tile type is out of range, received: " + tileInfo [tileIndex].AsInt);
				}
			}
		}
	}

	void MakeTileElement(GameObject elementPrefab, Sprite tileSprite, Vector2 tilePos, int depth) {
		var newTile = Instantiate (elementPrefab);
		newTile.GetComponent<SpriteRenderer> ().sprite = tileSprite;
		newTile.GetComponent<SpriteRenderer> ().sortingOrder = depth;
		newTile.transform.position = tilePos;
	}

	void MakeStartPos(Vector3 startPos){
		var startPosInstance = Instantiate (startPosPrefab);
		startPosInstance.transform.position = startPos;
	}

	void MakeBackground(JSONNode mapInfo) {
		string backgroundName = mapInfo["background"];
		Sprite backgroundSprite = Resources.Load<Sprite> ("Sprites/StageBackgrounds/Background" + backgroundName) as Sprite;
		float viewHeight = 2f * Camera.main.orthographicSize;
		float viewWidth = viewHeight * Camera.main.aspect;
		float backgroundHeight = backgroundSprite.bounds.size.y;
		float scaleToApply = viewHeight / backgroundHeight;

		var backgroundDrawer = Instantiate (backgroundPrefab);
		backgroundDrawer.GetComponent<SpriteRenderer> ().sprite = backgroundSprite;
		Vector2 backgroundPos = new Vector2 (-viewWidth / 2, viewHeight / 2);
		backgroundDrawer.transform.position = backgroundPos;
		backgroundDrawer.transform.localScale = new Vector2(scaleToApply, scaleToApply);
	}

	void MakeWallMask(GameObject wallmaskPrefab, Vector2 wallPos, float width, float height) {
		var wallMask = Instantiate (wallmaskPrefab);
		wallMask.transform.position = wallPos;
	}

	bool checkShouldBlock(JSONNode tileInfo, int row, int column, int mapWidth, int mapHeight){
		Vector2[] posToCheck = {
			new Vector2 (row - 1, column),
			new Vector2 (row + 1, column),
			new Vector2 (row, column - 1),
			new Vector2 (row, column + 1)
		};
		bool shouldBlock = false;

		foreach(Vector2 pos in posToCheck){
			if ((pos.x < 0 || pos.x >= mapHeight) || (pos.y < 0 || pos.y >= mapWidth)) continue;
			int tileIndex = (int) (mapWidth * pos.x + pos.y);
			if (tileInfo [tileIndex].AsInt != (int) _tileType.VOID) {
				shouldBlock = true;
				break;
			}
		}
		return shouldBlock;
	}
}
