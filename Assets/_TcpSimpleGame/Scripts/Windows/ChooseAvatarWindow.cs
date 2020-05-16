using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChooseAvatarWindow : MonoBehaviour, IView {

	private static ChooseAvatarWindow _instance;
	public static ChooseAvatarWindow Instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<ChooseAvatarWindow>();
			return _instance;
		}
	}

	[SerializeField]
	GameObject chooseAvatarWindow = null;

	[SerializeField]
	GameObject avaliablePreviewAvatarPrefab = null;
	[SerializeField]
	GameObject disablePreviewAvatarPrefab = null;
	[SerializeField]
	GameObject previewAvatarPrefab = null;
	[SerializeField]
	Transform parentVBox = null;

	void Start(){
		if (chooseAvatarWindow == null) {
			print ("Didn't find chooseAvatarWindow");
			Application.Quit ();
		}
		if (avaliablePreviewAvatarPrefab == null) {
			print ("Didn't find avaliablePreviewAvatarPrefab");
			Application.Quit ();
		}
		if (disablePreviewAvatarPrefab == null) {
			print ("Didn't find disablePreviewAvatarPrefab");
			Application.Quit ();
		}
		if (previewAvatarPrefab == null) {
			print ("Didn't find previewAvatarPrefab");
			Application.Quit ();
		}
		if (parentVBox == null) {
			print ("Didn't find parentVBox");
			Application.Quit ();
		}
	}

	public void ClearSubs(){
		if (parentVBox.childCount > 0) {
			foreach (Transform child in parentVBox.transform) {
				if (child.name != "ChooseAvatarText") {
					Debug.Log (child.name);
					GameObject.Destroy(child.gameObject);
				}
			}
		}
	}

	public void AddDisable(){
		GameObject newGo = (GameObject) Instantiate (disablePreviewAvatarPrefab, parentVBox, true);
	}

	public void AddAvailable(){
		GameObject newGo = (GameObject) Instantiate (avaliablePreviewAvatarPrefab, parentVBox, true);
		AvailablePreviewAvatarScript script = newGo.AddComponent<AvailablePreviewAvatarScript> ();
	}

	public void AddCharacter(int id, string name, int level){
		GameObject newGo = (GameObject) Instantiate (previewAvatarPrefab, parentVBox, true);
		PreviewAvatarScript script = newGo.AddComponent<PreviewAvatarScript> ();
		script.Id = id;
		script.Name = name;
		script.Level = level;
	}
		
	public void Show(){
		chooseAvatarWindow.SetActive (true);
	}

	public void Hide(){
		chooseAvatarWindow.SetActive (false);
	}
}
