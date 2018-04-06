using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeartState {EMPTY, FULL, HALF}

public class HeartBehavior : MonoBehaviour {
	
	public Sprite FullHeart;
	public Sprite HalfHeart;
	public Sprite EmptyHeart;
    public float posX;
    public SpriteRenderer spriteRenderer {get; private set;}
	public HeartState state {
        get{ return _state; }
        set{
            if (_state == value) return;
            _state = value;
            if (value == HeartState.EMPTY) {
                spriteRenderer.sprite = EmptyHeart;
            } else if (value == HeartState.FULL) {
                spriteRenderer.sprite = FullHeart;
            } else if (value == HeartState.HALF) {
                spriteRenderer.sprite = HalfHeart;
            }
        }
    }
    private HeartState _state;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

    void Update(){
        transform.localScale = transform.parent.localScale;
        float newX = transform.localScale.x * posX;
        transform.localPosition = new Vector3(newX, transform.localPosition.y);
    }
}
