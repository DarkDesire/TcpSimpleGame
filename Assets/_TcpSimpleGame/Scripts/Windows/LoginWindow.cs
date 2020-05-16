using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginWindow : MonoBehaviour, IView {

	private static LoginWindow _instance;
	public static LoginWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<LoginWindow>();
			return _instance;
		}
	}


	[SerializeField]
	GameObject loginWindow = null;

	[SerializeField]
	InputField emailField = null;
	[SerializeField]
	InputField passField = null;

	public string getEmail()
	{
		return emailField.text;
	}

	public string getPass()
	{
		return passField.text;
	}

	public void Show(){
		loginWindow.SetActive (true);
	}

	public void Hide(){
		emailField.text = string.Empty;
		passField.text = string.Empty;
		loginWindow.SetActive (false);
	}
}
