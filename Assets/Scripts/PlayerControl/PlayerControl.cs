using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	private HeroActor hero;

	void Awake () {
		hero = GetComponent<HeroActor> ();
	}

    void Start () {
        if (!hero.isLocalPlayer) {
            Destroy(this);
        }
    }
	void Update () {
        if (hero.isDead) return;
		ReactToMoveCommand ();
		ReactToActionCommand ();
	}

	void ReactToMoveCommand(){
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		Vector2 moveVector = new Vector2 (inputX, inputY);
		hero.Move (moveVector.normalized);
	}

	void ReactToActionCommand(){
        if (Input.GetButtonDown ("Attack")) {
            hero.DoAttack();
        } else if (Input.GetButtonDown ("Rope")) {
            hero.ThrowRope();
        } else if (Input.GetButton ("Skill_1") && Input.GetButton ("Skill_2")) {
            hero.UseSkill(HeroSkillIndex.ULTIMATE);
        } else if (Input.GetButton ("Skill_1")) {
            hero.UseSkill(HeroSkillIndex.SKILL1);
        } else if (Input.GetButton ("Skill_2")) {
            hero.UseSkill(HeroSkillIndex.SKILL2);
        } else if (Input.GetButtonDown ("Interact")) {
            hero.OnInteractKeyDown();
        } else if (Input.GetButtonUp ("Interact")) {
            hero.OnInteractKeyUp();
        }
	}
}
