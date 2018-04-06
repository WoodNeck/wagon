using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudProxyBehavior : MonoBehaviour, Environment {
	public GameObject tilePrefab;
    public Sprite mudWater;
    public Sprite mudWaterBottom;
    public Material mudMaterial;
    public Material normalMaterial;

    public int heightLevel;

	void Awake () {
		Vector2 tileSize = tilePrefab.GetComponent<MudTileBehavior> ().tileSet [0].bounds.size;
		float pixelsPerUnit = tilePrefab.GetComponent<MudTileBehavior> ().tileSet [0].pixelsPerUnit;
        
        int sizeX = (int) transform.localScale.x;
        int sizeY = (int) transform.localScale.y;

        Vector3 initPos = new Vector3();
        initPos.x = transform.position.x - GetComponent<BoxCollider2D>().bounds.size.x / 2f + tileSize.x /2f;
        initPos.y = transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y / 2f - tileSize.y /2f;

		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				float ydiff = UnityEngine.Random.Range (-3, 1) / pixelsPerUnit;
				GameObject newTile = Instantiate (tilePrefab) as GameObject;
                Vector3 posDiff = new Vector3(i * tileSize.x, -j * (tileSize.y - 0.11f) + ydiff);
				newTile.transform.position = initPos + posDiff;
				newTile.GetComponent<SpriteRenderer> ().sortingOrder = (int) -(gameObject.transform.position.y * 10) + j;
			}
		}

		GetComponent<SpriteRenderer>().sprite = mudWater;
        GetComponent<SpriteRenderer>().sortingOrder = (int) -(gameObject.transform.position.y * 10) + sizeY;

        GameObject waterBottomDrawer = new GameObject("MudWaterBottom");
        SpriteRenderer waterBottomRenderer = waterBottomDrawer.AddComponent<SpriteRenderer>();
        waterBottomDrawer.transform.parent = this.transform;
        waterBottomDrawer.transform.position = transform.position;
        waterBottomDrawer.transform.position -= new Vector3(0, GetComponent<BoxCollider2D>().bounds.size.y / 2f);
        waterBottomDrawer.transform.position -= new Vector3(0, mudWaterBottom.bounds.size.y / 2f);
        waterBottomDrawer.transform.localScale = new Vector3(1, waterBottomDrawer.transform.localScale.y, waterBottomDrawer.transform.localScale.z);

        waterBottomRenderer.sprite = mudWaterBottom;
        waterBottomRenderer.sortingOrder = (int) -(gameObject.transform.position.y * 10) + sizeY;
        waterBottomRenderer.sortingLayerName = "TileLayer";
    }

    public Vector2 ModifyMove(GameObject obj, Vector2 velocity){
        velocity = velocity / 2f;
        return velocity;
    }

    public float Height(Vector3 position){
        return heightLevel;
    }

	public void Enter(GameObject obj){
        obj.transform.position -= new Vector3(0, 0.05f, 0);
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        if (objRenderer != null)
            objRenderer.material = mudMaterial;
	}
    
    public void Stay(GameObject obj){

	}

	public void Exit(GameObject obj){
        obj.transform.position += new Vector3(0, 0.05f, 0);
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        if (objRenderer != null)
            objRenderer.material = normalMaterial;
	}
}