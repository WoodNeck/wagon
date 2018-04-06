using UnityEngine;
using System.Collections;

public class TileProxyBehavior : MonoBehaviour, Environment {
	public GameObject tilePrefab;

    public int heightLevel;

	void Awake () {
		Vector2 tileSize = tilePrefab.GetComponent<TileBehavior> ().tileSet [0].bounds.size;
		float pixelsPerUnit = tilePrefab.GetComponent<TileBehavior> ().tileSet [0].pixelsPerUnit;
        
        int sizeX = (int) transform.localScale.x;
        int sizeY = (int) transform.localScale.y;

        Vector3 initPos = new Vector3();
        initPos.x = transform.position.x - GetComponent<BoxCollider2D>().bounds.size.x / 2f + tileSize.x /2f;
        initPos.y = transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2f - tileSize.y /2f;

		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				float ydiff = UnityEngine.Random.Range (-4, 4) / pixelsPerUnit;
				GameObject newTile = Instantiate (tilePrefab) as GameObject;
                Vector3 posDiff = new Vector3(i * tileSize.x, -j * (tileSize.y - 0.11f) + ydiff);
				newTile.transform.position = initPos + posDiff;
				newTile.GetComponent<SpriteRenderer> ().sortingOrder = (int) -(gameObject.transform.position.y * 10) + j;
			}
		}

		Destroy (GetComponent<SpriteRenderer>());
    }

    public Vector2 ModifyMove(GameObject obj, Vector2 velocity){
        return velocity;
    }

    public float Height(Vector3 position){
        return heightLevel;
    }

    public void Enter(GameObject obj){

    }
    public void Stay(GameObject obj){

    }
    public void Exit(GameObject obj){

    }
}
