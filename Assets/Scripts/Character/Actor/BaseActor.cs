using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum ActorState {
    IDLE, RUN, ATTACK, ROPE, ROPING, ROPED, DEAD, DISABLED
}

// Actor class for all ingame object, including player characters and enemies.
public abstract class BaseActor : NetworkBehaviour, Damageable, Heightable {
    public SpriteRenderer spriteRenderer {get; protected set;}
	public Rigidbody2D body {get; protected set;}
	public Collider2D moveCollider {get; protected set;}
	public Animator animator {get; protected set;}
    public SpriteRenderer shadowDrawer { get; private set; }

    public ActorState state { get; set; }
    public HealthContainer hp {get; protected set;}
    public Direction direction {get; protected set;}
    public EnvironmentChecker environment { get; protected set; }
    public HeightBehavior heighter { get; protected set; }
    public bool isDead {get; protected set;}

    protected IEnumerator flashRoutine = null;
    protected Vector2 moveInput = new Vector2 (0, 0);
	protected float speedMultiplier = 0.65f;
	protected float directionalDeadZone = 0.03f;

	protected virtual void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		body = GetComponent<Rigidbody2D> ();
		moveCollider = transform.Find("MoveCollider").GetComponent<Collider2D> ();
		animator = GetComponent<Animator> ();
        shadowDrawer = transform.Find("ShadowDrawer").GetComponent<SpriteRenderer> ();

        state = ActorState.IDLE;
        direction = new Direction(Direction4.EAST);
        environment = gameObject.AddComponent<EnvironmentChecker>();
        environment.colliderOffset = (Vector3) moveCollider.offset;
        heighter = GetComponent<HeightBehavior>();
        heighter.AddHeightHoldingChild(moveCollider.gameObject);
        heighter.AddHeightHoldingChild(shadowDrawer.gameObject);
        isDead = false;
	}

	protected virtual void Update () {
        if (direction.dir4 == Direction4.WEST) {
            transform.localScale = new Vector3 (-1, 1, 1);
        } else if (direction.dir4 == Direction4.EAST) {
            transform.localScale = new Vector3 (1, 1, 1);
        }
        spriteRenderer.sortingOrder = (int)(-100 * (transform.position.y + moveCollider.offset.y));
	}

	protected virtual void FixedUpdate() {
        moveInput = environment.current.ModifyMove(gameObject, moveInput);
        body.velocity = moveInput;
        SetDirection (moveInput);
        if (body.velocity.sqrMagnitude > 0) {
            animator.SetFloat("speed", 1);
        } else {
            animator.SetFloat("speed", 0);
        }
	}

    protected virtual void SetDirection(Vector2 dir) {
        if (body.velocity.sqrMagnitude > directionalDeadZone) {
            Direction8 dir8 = Direction.ToDirection8(dir);
            direction.SetDirection(dir8);
        }
    }

    public virtual void Die(GameObject attackedObject){
        isDead = true;
        body.velocity = Vector3.zero;
        moveInput = Vector2.zero;
    }

    public abstract void Damage(GameObject attackedObject, float amount);

    public virtual void EventDamaged(){
        if (flashRoutine == null){
            flashRoutine = FlashRoutine();
            StartCoroutine(flashRoutine);
        } else {
            StopCoroutine(flashRoutine);
            flashRoutine = FlashRoutine();
            StartCoroutine(flashRoutine);
        }
    }

    IEnumerator FlashRoutine(){
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.material.color = Color.white;
    }

    public virtual void Move(Vector2 input){
        if (CanMove ()) {
            moveInput = input * speedMultiplier;
        } else {
            moveInput = Vector2.zero;
        }
    }

    public virtual void ForceMove(Vector2 input){
        moveInput = input * speedMultiplier;
    }

    public virtual bool CanMove(){
        if (isDead) return false;
        return true;
    }

    public abstract void EventGround();
}
