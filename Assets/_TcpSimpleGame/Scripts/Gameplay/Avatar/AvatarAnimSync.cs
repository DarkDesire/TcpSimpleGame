using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Avatar))]
public class AvatarAnimSync : MonoBehaviour {
	NetworkManager networkManager;
	Avatar avatar;
	int previousAnimState;

	// Use this for initialization
	void Awake () {
		networkManager = FindObjectOfType<NetworkManager>();
		avatar = GetComponent<Avatar>();
		previousAnimState = avatar.AnimState;
	}

	void Start(){
		StartCoroutine ("AnimSync");
	}

	IEnumerator AnimSync(){
		while (true) {
			if (avatar.AnimState!=previousAnimState){ // if animation is changed
				networkManager.sendAvatarAnimChange();
				previousAnimState = avatar.AnimState;
			}
			yield return new WaitForSeconds(0.001f); // 1s/100 tick ~ 10millis
		}
	}
}
