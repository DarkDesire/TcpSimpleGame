using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvailablePreviewAvatarScript : MonoBehaviour {
	
	private Button buttonToStateSetNewAvatar;
	private UIManager managerUI { get { return UIManager.Instance; } }

	void Awake(){
		buttonToStateSetNewAvatar = GetComponentInChildren<Button> ();
		if (buttonToStateSetNewAvatar == null) {
			Debug.Log ("Didnt find buttonToStateSetNewAvatar");
		}
	}
		
	void Start () {
		buttonToStateSetNewAvatar.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		managerUI.SetStateSetNewAvatar ();
	}
}
