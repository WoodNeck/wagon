using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairProxyBehavior : MonoBehaviour, Environment {
	public Sprite[] stairSprite;
    public int prevHeight, nextHeight;
	public GameObject stairPrefab;
	public Direction4 direction;

    int sizeX = 0, sizeY = 0;
    float tileSize;

	void Awake () {
        MakeStairs();
        ModifyCollider();
		Destroy (GetComponent<SpriteRenderer>());
	}

    void MakeStairs(){
        sizeX = (int) transform.localScale.x;
        sizeY = (int) transform.localScale.y;
        tileSize = stairSprite[0].bounds.size.x;
		Vector2 initialPos = CalcInitialPos ();
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				GameObject newStair = Instantiate (stairPrefab) as GameObject;
				Vector2 newPos = initialPos + CalcDiff (i, j);
                newStair.transform.position = new Vector3 (newPos.x, newPos.y);
				newStair.GetComponent<SpriteRenderer> ().sprite = stairSprite [(int)direction];
				newStair.GetComponent<SpriteRenderer> ().sortingOrder = (int) -(newStair.transform.position.y * 10) + j;
			}
		}
    }

	Vector2 CalcInitialPos(){
		
		float initialPosX = transform.position.x - tileSize * (sizeX - 1) / 2f;
		float initialPosY;
		if (direction != Direction4.WEST) {
            initialPosY = transform.position.y - (tileSize * sizeY) / 2f + tileSize / 2f;
        } else {
			initialPosY = transform.position.y + (tileSize * sizeY) / 2f - tileSize * (sizeY);
		}
		return new Vector2 (initialPosX, initialPosY);
	}

	Vector2 CalcDiff(int column, int row){
		float diffX = column * tileSize;
		float diffY = 0f;
		if (direction == Direction4.EAST) {
			diffY = column * (tileSize / 2f) + row * tileSize;
		} else if (direction == Direction4.WEST) {
			diffY = -column * (tileSize / 2f) + row * tileSize;
		} else {
			diffY = row * tileSize;
		}

		return new Vector2 (diffX, diffY);
	}

    void ModifyCollider(){
        if (direction == Direction4.EAST || direction == Direction4.WEST){
            PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
            Vector2[] points = collider.GetPath(0);
            float totalLength = Mathf.Abs(points[0].y - points[1].y);
            float fallValue = totalLength * ((tileSize / 2f) * sizeX) / (tileSize * sizeY);
            points[2] = new Vector2(points[2].x, points[2].y - fallValue);
            points[3] = new Vector2(points[3].x, points[3].y - fallValue);
            collider.SetPath(0, points);
        }
    }

    public Vector2 ModifyMove(GameObject obj, Vector2 velocity){
        if (direction == Direction4.EAST) {
			if (velocity.x != 0f){
				velocity = Mathf.Sign (velocity.x) * new Vector2(44, 22).normalized * velocity.magnitude;
			}
		} else if (direction == Direction4.WEST) {
			if (velocity.x != 0f){
				velocity = Mathf.Sign (velocity.x) * new Vector2(44, -22).normalized * velocity.magnitude;
			}
		} else if (direction == Direction4.NORTH) {
			if (velocity.y > 0f)
				velocity.y = velocity.y * 0.707f;
			else
				velocity.y = velocity.y * 1.414f;
		} else if (direction == Direction4.SOUTH) {
			if (velocity.y < 0f)
				velocity.y = velocity.y * 0.707f;
			else
				velocity.y = velocity.y * 1.414f;
        }

        if (obj.tag == "Wagon"){
            switch(direction){
                case Direction4.EAST:
                    velocity += new Vector2 (-44, -22).normalized * 0.1f;
                    break;
                case Direction4.WEST:
                    velocity += new Vector2 (44, -22).normalized * 0.1f;
                    break;
                case Direction4.NORTH:
                    velocity += Vector2.down * 0.1f;
                    break;
                case Direction4.SOUTH:
                    velocity += Vector2.up * 0.1f;
                    break;
            }
        }
        return velocity;
    }

    public float Height(Vector3 position){
        Vector3 localPos = position - transform.position;
        float result;
        float lerpValue;
        if (direction == Direction4.EAST){
            lerpValue = (localPos.x / (tileSize * sizeX / 2f) + 1) / 2f;
            result = Vector2.Lerp(new Vector2(prevHeight, 0), new Vector2(nextHeight, 0), lerpValue).x;
        } else if (direction == Direction4.WEST){
            lerpValue = (localPos.x / (tileSize * sizeX / 2f) + 1) / 2f;
            result = Vector2.Lerp(new Vector2(nextHeight, 0), new Vector2(prevHeight, 0), lerpValue).x;
        } else if (direction == Direction4.NORTH){
            lerpValue = (localPos.y / (tileSize * sizeY / 2f) + 1) / 2f;
            result = Vector2.Lerp(new Vector2(prevHeight, 0), new Vector2(nextHeight, 0), lerpValue).x;
        } else {
            lerpValue = (localPos.y / (tileSize * sizeY / 2f) + 1) / 2f;
            result = Vector2.Lerp(new Vector2(nextHeight, 0), new Vector2(prevHeight, 0), lerpValue).x;
        }
        return result;
    }

	public void Enter(GameObject obj){
        if (obj.tag == "Wagon"){
            WagonActor wagon = obj.GetComponent<WagonActor>();
            Quaternion rotation;
            if (direction == Direction4.EAST) {
                rotation = Quaternion.Euler (new Vector3 (0, 0, 22.5f));
            } else if (direction == Direction4.WEST) {
                rotation = Quaternion.Euler (new Vector3 (0, 0, -22.5f));
            } else {
                rotation = Quaternion.Euler(Vector3.zero);
            }
            wagon.spriteRenderer.transform.localRotation = rotation;
        }
	}

    public void Stay(GameObject obj){

    }

	public void Exit(GameObject obj){
        BaseActor actor = obj.GetComponent<BaseActor> ();
        if (actor != null){
            actor.direction.SetDirection(Direction.Dir4ToDir8(actor.direction.dir4));
        }
        if (obj.tag == "Wagon"){
            WagonActor wagon = obj.GetComponent<WagonActor>();
            wagon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
	}
}
