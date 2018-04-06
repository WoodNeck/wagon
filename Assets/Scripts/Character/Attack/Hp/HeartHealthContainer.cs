using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class HeartHealthContainer : HealthContainer {
	public GameObject heartPrefab;

    private List<HeartBehavior> hearts = new List<HeartBehavior> ();
    
    private IEnumerator showRoutine = null;

	void OnDestroy(){
		foreach (HeartBehavior heart in hearts) {
			Destroy (heart);
		}
	}

    public override void Damage(GameObject attackedObject, float amount){
        base.Damage(attackedObject, amount);

        bool willDead = (hp <= 0);
		if (!willDead) {
			ShowHealth();
		} else {
            StopShowHealth();
		}
        ModifyHearts();
    }

    public override void Heal(float amount){
        base.Heal(amount);

        if (!actor.isDead){
            ModifyHearts();
            ShowHealth();
        }
    }

	public override void SetMaxHealth(int value) {
		base.SetMaxHealth(value);

		float padding = 0.05f;
		float spriteWidth = heartPrefab.GetComponent<HeartBehavior> ().FullHeart.bounds.size.x;
		float initialPosX = -(spriteWidth * value + padding * (value - 1)) / 2 + spriteWidth / 2f;
		for (int i = 0; i < value; i ++) {
			GameObject heartObject = Instantiate (heartPrefab);
            HeartBehavior heart = heartObject.GetComponent<HeartBehavior>();
			heart.transform.parent = transform;
			heart.transform.localPosition = new Vector3 (0f, 0.2f);
            heart.posX = initialPosX + i * (spriteWidth + padding);
			heart.state = HeartState.FULL;
            heart.spriteRenderer.enabled = false;
			hearts.Add (heart.GetComponent<HeartBehavior>());
		}
	}

    void ModifyHearts(){
        int cnt = 0;
        foreach (HeartBehavior heart in hearts){
            float heartHp = hp - cnt;
            if (heartHp >= 1){
                heart.state = HeartState.FULL;
            } else if (heartHp >= 0.5){
                heart.state = HeartState.HALF;
            } else {
                heart.state = HeartState.EMPTY;
            }
            cnt++;
        }
    }

    void ShowHealth(){
        if (showRoutine == null) {
            showRoutine = ShowRoutine();
            StartCoroutine(showRoutine);
        }
        else {
            StopCoroutine(showRoutine);
            showRoutine = ShowRoutine();
            StartCoroutine(showRoutine);
        }
    }

    void StopShowHealth(){
        if (showRoutine != null){
            StopCoroutine(showRoutine);
        }
        foreach (HeartBehavior heart in hearts){
            heart.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    IEnumerator ShowRoutine(){
        for (int i = 0; i < 5; i ++){
            foreach (HeartBehavior heart in hearts){
                heart.GetComponent<SpriteRenderer>().enabled = true;
            }

            yield return new WaitForSeconds(0.25f);

            foreach (HeartBehavior heart in hearts){
                heart.GetComponent<SpriteRenderer>().enabled = false;
            }

            yield return new WaitForSeconds(0.25f);
        }

        showRoutine = null;
    }
}
