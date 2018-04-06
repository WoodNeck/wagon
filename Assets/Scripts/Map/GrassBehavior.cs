using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBehavior : MonoBehaviour {
    public Material shadowMaterial;
    public SpriteRenderer spriteRenderer {get; private set;}

	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(-100 * transform.position.y);

        CheckGrassOverlap();
	}

    void CheckGrassOverlap(){
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector3[] pointsToCheck = new Vector3[4];
        Vector3 boxSize = collider.bounds.size;
        pointsToCheck[0] = collider.bounds.min;
        pointsToCheck[1] = collider.bounds.max;
        pointsToCheck[2] = collider.bounds.center + new Vector3(boxSize.x / 2, 0) - new Vector3(0, boxSize.y / 2);
        pointsToCheck[3] = collider.bounds.center - new Vector3(boxSize.x / 2, 0) + new Vector3(0, boxSize.y / 2);

        foreach(Vector3 point in pointsToCheck){
            Collider2D[] overlaps = Physics2D.OverlapPointAll(point);
            foreach(Collider2D overlap in overlaps){
                if (overlap.tag == "Grass"){
                    if (overlap.gameObject.Equals(gameObject)) continue;

                    if (overlap.transform.position.y < transform.position.y){
                        spriteRenderer.material = shadowMaterial;
                        break;
                    }
                }
            }
        }
    }
}
