using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyWindow : MonoBehaviour, IView {

	private static LobbyWindow _instance;
	public static LobbyWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<LobbyWindow>();
			return _instance;
		}
	}


	[SerializeField]
	GameObject lobbyWindow = null;

	public void Show(){
		lobbyWindow.SetActive (true);
	}

	public void Hide(){
		lobbyWindow.SetActive (false);
	}
}
