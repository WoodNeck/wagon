using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RopeBehavior : MonoBehaviour {
    public GameObject ropeHolderPrefab;
	public DistanceJoint2D joint { get; set; }
	public Vector2 connectedBodySize{ get; set; }
	public Collider2D targetCollider{ get; set; }
    public GameObject owner {get; set;}
    public RopeHolderBehavior ropeHolder {get; set;}
	public SpriteRenderer spriteRenderer{ get; private set; }

	void Awake(){
		joint = null;
		connectedBodySize = new Vector2 ();
		targetCollider = null;
        owner = null;
        ropeHolder = null;
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnDestroy(){
		if (joint != null) {
			Destroy (joint);
		}
        if (ropeHolder != null){
            Destroy(ropeHolder.gameObject);
        }
	}

	public void InitializeRope(GameObject heroObject, GameObject other){
        owner = heroObject;
		owner.GetComponent<HeroActor> ().rope = this;
		targetCollider = other.transform.Find("MoveCollider").GetComponent<Collider2D> ();

		joint = owner.AddComponent<DistanceJoint2D>() as DistanceJoint2D;
		joint.connectedBody = other.GetComponent<Rigidbody2D> ();
		joint.autoConfigureDistance = false;
		joint.maxDistanceOnly = true;
		joint.enableCollision = true;
		joint.distance = 0.66f;

		// Attached to player(hero)
		joint.anchor = owner.GetComponent<HeroActor>().AnchorPos();
		// Attached to target
		joint.connectedAnchor = other.GetComponent<Ropeable>().AnchorPos();

        GameObject holder = Instantiate(ropeHolderPrefab);
        holder.transform.parent = other.transform;
        holder.transform.localPosition = (Vector3) other.GetComponent<Ropeable>().AnchorPos();
        holder.GetComponent<SpriteRenderer>().size = new Vector2 (0.15f, 0.03f);
        ropeHolder = holder.GetComponent<RopeHolderBehavior>();
	}

	void Update(){
        if (joint == null) return;
        int targetSortingOrder = joint.connectedBody.GetComponent<SpriteRenderer>().sortingOrder;
        int ownerSortingOrder = owner.GetComponent<SpriteRenderer>().sortingOrder;
        spriteRenderer.sortingOrder = Mathf.Min(targetSortingOrder, ownerSortingOrder) - 1;
        DrawRope();
        CalcRopeForce();
	}

    public void ApplyRopeForce(Vector2 forceDir){
        Ropeable attachedObject = joint.connectedBody.transform.root.GetComponent<Ropeable>();
        attachedObject.ApplyRopeForce(forceDir);
    }

    void CalcRopeForce(){
        HeroActor hero = owner.GetComponent<HeroActor>();
        if (!hero.isLocalPlayer) return;
        if (hero.state == ActorState.IDLE) return;
		Vector2 ropeForce = joint.GetReactionForce (Time.deltaTime);
        if (!float.IsNaN(ropeForce.x) && !float.IsNaN(ropeForce.y)){
            ropeForce = ropeForce.normalized;
			hero.ropeController.CmdApplyRopeForce (ropeForce);
        }
    }

    void DrawRope(){
		Vector2 heroPos = new Vector2 (joint.transform.position.x, joint.transform.position.y); //+ new Vector2 (joint.anchor.x, joint.anchor.y);
		Vector2 targetPos = new Vector2 (joint.connectedBody.transform.position.x, joint.connectedBody.transform.position.y) + targetCollider.offset;

		Vector2 targetToHero = heroPos - targetPos;
		float dirSize = Vector2.Angle (Vector2.right, targetToHero);
		float dirSign = Mathf.Sign (Vector2.Dot (Vector2.up, targetToHero));
		float ropeDirection = dirSign * dirSize;
		if (ropeDirection < 0) ropeDirection += 360;

		transform.position = targetPos + targetToHero / 2f;
		spriteRenderer.size = new Vector2 (targetToHero.magnitude, 0.03f);
		transform.localRotation = Quaternion.Euler (0, 0, ropeDirection);
    }

    /* void OnGUI () {
        Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
        float scale = Screen.dpi / 96.0f;
        GUIStyle boxStyle = new GUIStyle (GUI.skin.label);
        boxStyle.fontSize = (int) (20 * scale);

        Vector2 ropeForce = joint.GetReactionForce (Time.deltaTime);

        Rect charPos = new Rect (pos.x + 22, Screen.height - pos.y - 20, 400, 100);
        GUI.Label (charPos, string.Format ("{0:F2}", ropeForce.x), boxStyle);
        Rect charPos2 = new Rect (pos.x + 22, Screen.height - pos.y, 400, 100);
        GUI.Label (charPos2, string.Format ("{0:F2}", ropeForce.y), boxStyle);
    } */
}
