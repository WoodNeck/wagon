using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushAnimBehavior : StateMachineBehaviour {
    public int minLeaf;
    public int maxLeaf;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        BushBehavior bush = animator.GetComponent<BushBehavior>();
        int bushDepth = bush.GetComponent<SpriteRenderer>().sortingOrder;
        int leafCount = Random.Range(minLeaf, maxLeaf + 1);
        for (int i = 0; i < leafCount; i++){
            GameObject leaf = Instantiate(bush.bushLeafPrefab);
            SpriteRenderer leafRenderer = leaf.GetComponent<SpriteRenderer>();
            leafRenderer.sprite = bush.bushLeaf[Random.Range(0, bush.bushLeaf.Length)];
            leafRenderer.sortingOrder = bushDepth + 1;
            Rigidbody2D leafRigidbody = leaf.GetComponent<Rigidbody2D>();
            leafRigidbody.gravityScale = 0.03f;
            Vector2 leafVector = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.4f, 0.6f)).normalized;
			leafRigidbody.AddForce (leafVector * 0.5f, ForceMode2D.Impulse);
			leafRigidbody.AddTorque (Random.Range(-20f, 20f), ForceMode2D.Impulse);
        }
	}
}
