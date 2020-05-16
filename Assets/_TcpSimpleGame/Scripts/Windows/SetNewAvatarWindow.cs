using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetNewAvatarWindow : MonoBehaviour, IView {

	private static SetNewAvatarWindow _instance;
	public static SetNewAvatarWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<SetNewAvatarWindow>();
			return _instance;
		}
	}


	[SerializeField]
	GameObject setNewAvatarWindow = null;

	[SerializeField]
	InputField newAvatarField = null;

	public string getNewAvatar()
	{
		return newAvatarField.text;
	}

	public void Show(){
		setNewAvatarWindow.SetActive (true);
	}

	public void Hide(){
		newAvatarField.text = string.Empty;
		setNewAvatarWindow.SetActive (false);
	}
}
