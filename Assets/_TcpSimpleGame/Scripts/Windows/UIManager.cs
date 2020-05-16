using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
	enum State { LOGIN = 0, SET_FAMILY = 1, CHOOSE_AVATAR = 2, SET_NEW_AVATAR = 3, LOBBY = 4, GAMEPLAY = 5, WAITING = 6 }
	State state = State.LOGIN;

	private static UIManager _instance;
	public static UIManager Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<UIManager>();
			return _instance;
		}
	}

	[SerializeField]
	GameObject standByCamera = null;
	[SerializeField]
	GameObject gameplayCamera = null;

	private LoginWindow loginWindow 					{ get { return LoginWindow.Instance; } }
	private SetFamilyWindow setFamilyWindow 			{ get { return SetFamilyWindow.Instance; } }
	private ChooseAvatarWindow chooseAvatarWindow	 	{ get { return ChooseAvatarWindow.Instance; } }
	private SetNewAvatarWindow setNewAvatarWindow 		{ get { return SetNewAvatarWindow.Instance; } }
	private LobbyWindow lobbyWindow 					{ get { return LobbyWindow.Instance; } }
	private GameplayWindow gameplayWindow 				{ get { return GameplayWindow.Instance; } }
	private WaitingWindow waitingWindow 				{ get { return WaitingWindow.Instance; } }

	private Dictionary<string, IView> views = new Dictionary<string, IView> ();

	private void HideAllViews(){
		foreach(KeyValuePair<string, IView> entry in views)
		{
			entry.Value.Hide ();
		}
	}

	void Awake ()
	{
		views.Add ("LOGIN", loginWindow);
		views.Add ("SET_FAMILY", setFamilyWindow);
		views.Add ("CHOOSE_AVATAR", chooseAvatarWindow);
		views.Add ("SET_NEW_AVATAR", setNewAvatarWindow);
		views.Add ("LOBBY", lobbyWindow);
		views.Add ("GAMEPLAY", gameplayWindow);
		views.Add ("WAITING", waitingWindow);
	}


	void Start(){
		SetStateLogin ();
		standByCamera.SetActive (true);
	}

	public void SetStateLogin() {
		SetState(State.LOGIN);
	}

	public void SetStateSetFamily() {
		SetState(State.SET_FAMILY);
	}

	public void SetStateChooseAvatar() {
		SetState(State.CHOOSE_AVATAR);
	}

	public void SetStateSetNewAvatar(){
		SetState(State.SET_NEW_AVATAR);
	}

	public void SetStateLobby(){
		SetState(State.LOBBY);
	}

	public void SetStateGameplay(){
		gameplayCamera.SetActive (true);
		standByCamera.SetActive (false);
		SetState(State.GAMEPLAY);
	}
		
	public void SetStateWaiting(){
		SetState(State.WAITING);
	}
		
	private void SetState(State value) {
		HideAllViews ();
		int stateInt = (int)(object)value;
		switch (stateInt) {
		case (int)State.LOGIN:
			views ["LOGIN"].Show ();
			break;
		case (int)State.SET_FAMILY:
			views ["SET_FAMILY"].Show ();
			break;
		case (int)State.CHOOSE_AVATAR:
			views ["CHOOSE_AVATAR"].Show ();
			break;
		case (int)State.SET_NEW_AVATAR:
			views ["SET_NEW_AVATAR"].Show ();
			break;
		case (int)State.LOBBY:
			views ["LOBBY"].Show ();
			break;
		case (int)State.GAMEPLAY:
			views ["GAMEPLAY"].Show ();
			break;
		case (int)State.WAITING:
			views ["WAITING"].Show ();
			break;
		}
		state = (State)value;
	}

}
