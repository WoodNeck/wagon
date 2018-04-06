using UnityEngine;
using System.Collections;

public class BackgroundBehavior : MonoBehaviour {
	public float scrollSpeed;

	private float backgroundWidth;
	private float cameraWidth;

	public Vector3 originalPos {get; set;}
	public GameObject previousScroll {get; set;}
	public GameObject nextScroll {get; set;}

	void Awake () {
		backgroundWidth = GetComponent<SpriteRenderer> ().sprite.bounds.size.x;
		cameraWidth = 2f * Camera.main.orthographicSize * Camera.main.aspect;
		originalPos = new Vector3 (0, 0);
		previousScroll = null;
		nextScroll = null;
	}

	void Update () {
		Vector3 cameraPos = Camera.main.gameObject.transform.position;

		if (transform.position.x + backgroundWidth / 2f < cameraPos.x - cameraWidth / 2f) {
			if (nextScroll != null)
				nextScroll.GetComponent<BackgroundBehavior> ().previousScroll = null;
			Destroy (gameObject);
		} else if (transform.position.x + backgroundWidth / 2f < cameraPos.x + cameraWidth / 2f) {
			if (nextScroll == null) {
				nextScroll = Instantiate(gameObject, new Vector3(-100, -100), Quaternion.identity) as GameObject;
				nextScroll.GetComponent<BackgroundBehavior> ().originalPos = new Vector3(originalPos.x + backgroundWidth, originalPos.y, originalPos.z);
				nextScroll.GetComponent<BackgroundBehavior> ().previousScroll = gameObject;
				nextScroll.GetComponent<BackgroundBehavior> ().nextScroll = null;
			}
		}
	}

	void LateUpdate () {
		Vector3 cameraPos = Camera.main.gameObject.transform.position;
		transform.position = new Vector3 (originalPos.x + scrollSpeed * cameraPos.x, cameraPos.y, 0f);
	}
}