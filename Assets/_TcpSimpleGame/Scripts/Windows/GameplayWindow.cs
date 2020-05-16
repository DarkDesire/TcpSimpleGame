using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayWindow : MonoBehaviour, IView {

	private static GameplayWindow _instance;
	public static GameplayWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<GameplayWindow>();
			return _instance;
		}
	}

	private float killedBySeconds = 4;
	private int respawnSeconds = 10;
	private int lastRespawnSeconds;

	[SerializeField]
	GameObject gameplayWindow = null;
	[SerializeField]
	GameObject killedBy = null;
	Text killedByText = null;
	[SerializeField]
	GameObject respawn = null;
	Text respawnText = null;

	void Awake(){
		lastRespawnSeconds = respawnSeconds;
		killedByText = killedBy.GetComponentInChildren<Text> ();
		respawnText = respawn.GetComponentInChildren<Text> ();
	}

	public void Show(){
		gameplayWindow.SetActive (true);
	}

	public void Hide(){
		gameplayWindow.SetActive (false);
	}

	public void RespawnCoroutineOn(){
		StartCoroutine ("RespawnCoroutine");
	}

	public void KilledByCoroutineOn(string value){
		StartCoroutine (KilledByCoroutine(value));
	}

	IEnumerator RespawnCoroutine(){
		respawn.SetActive (true);
		while (lastRespawnSeconds >= 1) {
			respawnText.text = string.Format ("Respawn will be in {0} s.", lastRespawnSeconds.ToString ());
			lastRespawnSeconds = lastRespawnSeconds - 1;
			yield return new WaitForSeconds (1f);
		}
		lastRespawnSeconds = respawnSeconds;
		respawn.SetActive (false);
	}


	IEnumerator KilledByCoroutine(string value){
		killedBy.SetActive (true);
		killedByText.text = value;
		yield return new WaitForSeconds (killedBySeconds);
		killedBy.SetActive (false);
	}
}
