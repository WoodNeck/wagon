using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WagonActor : NetworkBehaviour, Ropeable {
    public SpriteRenderer spriteRenderer {get; protected set;}
	public Rigidbody2D body {get; protected set;}
	public Collider2D moveCollider {get; protected set;}
	public Animator animator {get; protected set;}

    public Direction direction {get; protected set;}
    public EnvironmentChecker environment { get; protected set; }

    private Vector2 rotateDir = new Vector2(1, 0);
    private float velocityDeadZone = 0.10f;

    [SyncVar] private int connectedRopes;

	void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
		body = GetComponent<Rigidbody2D> ();
		moveCollider = transform.Find("MoveCollider").GetComponent<Collider2D> ();
		animator = GetComponent<Animator> ();

        direction = new Direction(Direction4.EAST);
        environment = gameObject.AddComponent<EnvironmentChecker>();
        environment.colliderOffset = (Vector3) moveCollider.offset;

		connectedRopes = 0;
	}
    void Update(){
        spriteRenderer.sortingOrder = (int)(-100 * transform.position.y);
        SmoothRotation();
        SetDirection(rotateDir);
    }

	void FixedUpdate() {
        body.velocity = environment.current.ModifyMove(gameObject, body.velocity);
	}

    void SmoothRotation(){
		if (CanSetDirection ()) {
			Vector3 dirVec3 = new Vector3 (rotateDir.x, rotateDir.y);
			Vector3 velVec3 = new Vector3 (body.velocity.normalized.x, body.velocity.normalized.y);
			rotateDir = Vector3.Slerp (dirVec3, velVec3, 0.02f);
        }
    }

	void SetDirection(Vector2 dir){
        Direction8 dir8 = Direction.ToDirection8(dir);
        direction.SetDirection(dir8);
        animator.SetFloat("hdirection", rotateDir.x);
        animator.SetFloat("vdirection", rotateDir.y);
	}

	bool CanSetDirection(){
		if (connectedRopes <= 0) return false;
		if (rotateDir == body.velocity.normalized) return false;
		if (body.velocity.magnitude <= velocityDeadZone) return false;
		return true;
	}

    public void ApplyRopeForce(Vector2 forceDir){
        body.velocity = forceDir;
    }
    
	public void AttachRope(RopeBehavior rope){
        rope.ropeHolder.spriteRenderer.size = new Vector2(moveCollider.bounds.size.x, 0.04f);
        connectedRopes += 1;
	}

	public void DetachRope(RopeBehavior rope){
		connectedRopes = Mathf.Max(0, connectedRopes - 1);
	}

    public bool CanRoped(){
        return true;
    }

    public Vector2 AnchorPos(){
        return new Vector2(0f, 0f);
    }
}