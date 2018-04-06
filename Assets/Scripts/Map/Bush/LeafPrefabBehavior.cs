using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPrefabBehavior : MonoBehaviour {
    public GameObject leafBasePrefab;
    public GameObject leafPrefab;
    public Sprite[] leafSprite;

	void Awake () {
        AddLeaf(Direction4.NORTH);
        AddLeaf(Direction4.SOUTH);
        AddLeaf(Direction4.WEST);
        AddLeaf(Direction4.EAST);
    }

    void AddLeaf(Direction4 direction){
		Vector2 baseSize = leafBasePrefab.GetComponent<SpriteRenderer>().bounds.size;
        Vector2 leafSize = leafSprite[0].bounds.size;
        
        int sizeX = (int) transform.localScale.x;
        int sizeY = (int) transform.localScale.y;

        Vector3 initPos = new Vector3();
        int prevHeight, nextHeight;
        int prevHeightDiff;
        int heightDiffEase = 3;
        int scaleX = 1, scaleY = 1;
        int totalCount = 0;

        if (direction == Direction4.NORTH){
            initPos.x = transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x / 2f;
            initPos.y = transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y / 2f;
            totalCount = sizeX;
            scaleY = -1;
        } else if (direction == Direction4.SOUTH){
            initPos.x = transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x / 2f;
            initPos.y = transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2f;
            totalCount = sizeX;
            scaleY = 1;
        } else if (direction == Direction4.WEST){
            initPos.x = transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x / 2f;
            initPos.y = transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y / 2f;
            totalCount = sizeY;
        } else if (direction == Direction4.EAST){
            initPos.x = transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2f;
            initPos.y = transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y / 2f;
            totalCount = sizeY;
        }

        prevHeight = 0;
        prevHeightDiff = 0;
        for (int i = 0; i < totalCount; i++){
            if (i != totalCount - 1) {
                int minVal = Mathf.Max(0, prevHeight - (heightDiffEase - prevHeightDiff));
                int maxVal = Mathf.Min(4, prevHeight + (heightDiffEase - prevHeightDiff));
                nextHeight = UnityEngine.Random.Range(minVal, maxVal);
            } else {
                nextHeight = 0;
            }
            int heightDiff = Mathf.Abs(prevHeight - nextHeight);
            int spriteIndex = 7 * heightDiff + UnityEngine.Random.Range(0, 7);

            int baseCount = Mathf.Min(prevHeight, nextHeight);
            for (int j = 0; j < baseCount; j++){
                GameObject newBase = Instantiate(leafBasePrefab);
                Vector3 basePosDiff = new Vector3();
                if (direction == Direction4.NORTH){
                    basePosDiff = new Vector3(i * baseSize.x + baseSize.x / 2f, (2*j + 1) * baseSize.y / 2f);
                } else if (direction == Direction4.SOUTH){
                    basePosDiff = new Vector3(i * baseSize.x + baseSize.x / 2f, -(2*j + 1) * baseSize.y / 2f);
                } else if (direction == Direction4.WEST){
                    basePosDiff = new Vector3(-(2*j + 1) * baseSize.y / 2f, -i * baseSize.x - baseSize.x / 2f);
                } else if (direction == Direction4.EAST){
                    basePosDiff = new Vector3((2*j + 1) * baseSize.y / 2f, -i * baseSize.x - baseSize.x / 2f);
                }
                newBase.transform.position = initPos + basePosDiff;
                if (direction == Direction4.WEST || direction == Direction4.EAST){
                    newBase.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                }
            }

            GameObject newLeaf = Instantiate(leafPrefab);
            Vector3 leafPosDiff = new Vector3();
            if (direction == Direction4.NORTH){
                leafPosDiff = new Vector3(i * baseSize.x + baseSize.x / 2f, (baseCount * baseSize.y + leafSize.y / 2f));
            } else if (direction == Direction4.SOUTH){
                leafPosDiff = new Vector3(i * baseSize.x + baseSize.x / 2f, -(baseCount * baseSize.y + leafSize.y / 2f));
            } else if (direction == Direction4.WEST){
                leafPosDiff = new Vector3(-(baseCount * baseSize.y + leafSize.y / 2f), -i * baseSize.x - baseSize.x / 2f);
            } else if (direction == Direction4.EAST){
                leafPosDiff = new Vector3((baseCount * baseSize.y + leafSize.y / 2f), -i * baseSize.x - baseSize.x / 2f);
            }
            newLeaf.transform.position = initPos + leafPosDiff;

            if (prevHeight > nextHeight){
                scaleX = -1;
            } else {
                scaleX = 1;
            }
            if (direction == Direction4.EAST) scaleX *= -1;
            newLeaf.transform.localScale = new Vector3(scaleX, scaleY);
            if (direction == Direction4.EAST){
                newLeaf.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            } else if (direction == Direction4.WEST){
                newLeaf.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            }
            newLeaf.GetComponent<SpriteRenderer>().sprite = leafSprite[spriteIndex];

            prevHeight = nextHeight;
            prevHeightDiff = heightDiff;
        }
    }
}
