using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public enum HeroSkillIndex{
    SKILL1, SKILL2, ULTIMATE
}

public class HeroActor : BaseActor, Ropeable, Interactable {
    public RopeBehavior rope { get; set; }
    public GameObject attackCollider { get; private set; }
    public PlayerCommander commander { get; private set; }
    public RopeController ropeController { get; private set; }
    public Interactor interactor { get; private set; }
    public HeroSkill[] skill { get; private set; }

    [SyncVar(hook="NetworkSetDirection")] Direction8 networkDirection;
    private HeroActor revivingHero = null;
    private IEnumerator reviveRoutine = null;
    private float reviveGauge = 0f;
    private float maxReviveGauge = 100f;

    protected override void Awake () {
        base.Awake ();
        hp = GetComponent<HeartHealthContainer> ();
		hp.transform.parent = transform;
        hp.SetMaxHealth (3);
        rope = null;
        attackCollider = transform.Find("AttackCollider").gameObject;
        interactor = GetComponent<Interactor>();
        commander = GetComponent<PlayerCommander>();
        ropeController = GetComponent<RopeController>();

        skill = new HeroSkill[3];
        skill[0] = new HealSkill();
        skill[1] = new HealSkill();
        skill[2] = new HealSkill();
        directionalDeadZone = 0.40f;
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate () {
        body.velocity = moveInput;
        if (isLocalPlayer) {
            SetDirection (moveInput);
            if (body.velocity.sqrMagnitude > 0) {
                animator.SetFloat("speed", 1);
            } else {
                animator.SetFloat("speed", 0);
            }
        }
    }

    protected override void SetDirection (Vector2 dir) {
        if (dir.sqrMagnitude > directionalDeadZone) {
            Direction8 newDir8 = Direction.ToDirection8 (dir);
            if (newDir8 != direction.dir8) CmdSetDirection(newDir8);
            direction.SetDirection (newDir8);
            animator.SetFloat ("direction", direction.ToAnimDirection ());
        }
    }

    [Command] protected void CmdSetDirection(Direction8 dir){
        networkDirection = dir;
    }

    protected void NetworkSetDirection(Direction8 dir8){
        networkDirection = dir8;
        direction.SetDirection(dir8);
        animator.SetFloat("direction", direction.ToAnimDirection());
    }

    public override void Die (GameObject attackedObject) {
        if (!isDead) {
			isDead = true;
            if (isLocalPlayer) ropeController.CmdDestroyRope ();
            body.velocity = new Vector2 (0, 0);
            animator.SetTrigger ("Dead");
        }
    }

    public void Revive() {
        if (!isDead) return;
        isDead = false;
        animator.SetTrigger("Revive");
        hp.CmdHeal(3f);

        if (revivingHero != null) {
            revivingHero.interactor.FinishInteract();
            revivingHero = null;
        }
    }

	public override void Damage(GameObject attackedObject, float amount){
        if (!isLocalPlayer) return;
        if (isDead) return;

		if (hp != null) {
            hp.CmdDamage(attackedObject, amount);
        } else {
            Debug.LogError("Hero's hp is null.");
        }
    }

    public override bool CanMove () {
        if      (state == ActorState.ROPE)     return false;
        else if (state == ActorState.ROPED)    return false;
        else if (state == ActorState.ATTACK)   return false;
        else if (state == ActorState.DISABLED) return false;
        else if (interactor.isInteracting)     return false;
        return true;
    }

    public override void EventGround(){
        // DO NOTHING
    }

    public void DoAttack () {
        if (CanAttack()){
            animator.SetTrigger ("Attack");
        }
    }

    bool CanAttack(){
        if      (state == ActorState.ROPE)    return false;
        else if (state == ActorState.ROPING)  return false;
        else if (state == ActorState.DEAD)    return false;
        else if (interactor.isInteracting)   return false;
        return true;
    }

    public void ThrowRope () {
        if (!CanMove()) return;

        if (rope == null) {
            animator.SetTriggerOneFrame ("RopeUsed");
        } else {
            ropeController.CmdDestroyRope ();
        }
    }

    public bool CanRoped(){
        return true;
    }

    public void AttachRope(RopeBehavior rope){
        rope.ropeHolder.spriteRenderer.size = new Vector2 (0.15f, 0.03f);
        body.drag = 100f;
        animator.SetTrigger("Roped");
    }

    public void DetachRope(RopeBehavior rope){
        body.drag = 0f;
        if (!isDead){
            animator.SetTrigger("Unroped");
        }
    }
    public void ApplyRopeForce(Vector2 forceDir){
        body.AddForce(forceDir);
    }
    public Vector2 AnchorPos(){
        return moveCollider.offset;
    }

    public void OnInteractKeyDown () {
        interactor.OnInteractKeyDown();
    }
    
    public void OnInteractKeyUp () {
        interactor.OnInteractKeyUp();
    }

    public bool CanInteract() {
        if (isDead && revivingHero == null) return true;
        return false;
    }

    public InteractType Interact (HeroActor hero) {
        revivingHero = hero;
        reviveRoutine = ReviveRoutine();
        StartCoroutine(reviveRoutine);
        return InteractType.HOLD;
    }

    public void StopInteract (HeroActor hero){
        revivingHero = null;
        reviveGauge = 0f;
        StopCoroutine(reviveRoutine);
    }

    IEnumerator ReviveRoutine(){
        while (reviveGauge < maxReviveGauge){
            reviveGauge += 2f;
            yield return new WaitForSeconds(.1f);
        }
        Revive();
    }

    public void UseSkill(HeroSkillIndex index){
        skill[(int)index].Execute(this);
    }

    void OnGUI(){
        bool isReviving = (revivingHero != null);
        if (isReviving){
            Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);

            Texture2D guageTexture = new Texture2D(1, 1);
            guageTexture.SetPixel(0, 0, Color.white);
            guageTexture.Apply();
            Rect reviveGaugeOutline = new Rect (pos.x - 30f, Screen.height - pos.y - 20f, 60, 10);
            GUI.color = Color.white;
            GUI.DrawTexture(reviveGaugeOutline, guageTexture);

            Rect reviveGaugeRect = new Rect (pos.x - 30f, Screen.height - pos.y - 20f, Mathf.Floor(60 * (reviveGauge / maxReviveGauge)), 10);
            GUI.color = Color.red;
            GUI.DrawTexture(reviveGaugeRect, guageTexture);
        }
    }
}
