using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitingWindow : MonoBehaviour, IView {

	private static WaitingWindow _instance;
	public static WaitingWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<WaitingWindow>();
			return _instance;
		}
	}


	[SerializeField]
	GameObject waitingWindow = null;

	public void Show(){
		waitingWindow.SetActive (true);
	}

	public void Hide(){
		waitingWindow.SetActive (false);
	}
}
