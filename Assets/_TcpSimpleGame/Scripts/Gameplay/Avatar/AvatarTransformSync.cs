using UnityEngine;
using System.Collections;

public class AvatarTransformSync : MonoBehaviour {
	NetworkManager networkManager;
	Vector3 previousPosition;
	Vector3 previousRotation;

	// Use this for initialization
	void Awake () {
		networkManager = FindObjectOfType<NetworkManager>();

		previousPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		previousRotation = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	void Start(){
		StartCoroutine ("TransformSync");
	}

	IEnumerator TransformSync(){
		while (true) {
			if (transform.position!=previousPosition || transform.eulerAngles!=previousRotation){ // if rotation or position are changed
				networkManager.sendAvatarMove();
				previousPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
				previousRotation = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			}
			yield return new WaitForSeconds(0.001f); // 1s/100 tick ~ 10millis
		}
	}
}
