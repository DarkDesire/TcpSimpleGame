using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetFamilyWindow : MonoBehaviour, IView {

	private static SetFamilyWindow _instance;
	public static SetFamilyWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<SetFamilyWindow>();
			return _instance;
		}
	}
		
	[SerializeField]
	GameObject setFamilyWindow = null;

	[SerializeField]
	InputField familyField = null;

	public string getFamily()
	{
		return familyField.text;
	}

	// interface IView 
	public void Show(){
		setFamilyWindow.SetActive (true);
	}
	public void Hide(){
		familyField.text = string.Empty;
		setFamilyWindow.SetActive (false);
	}
}
