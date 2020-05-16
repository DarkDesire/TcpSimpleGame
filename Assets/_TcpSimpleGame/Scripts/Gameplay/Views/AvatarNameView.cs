using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AvatarNameView : MonoBehaviour
{
	Text nameText;
	[SerializeField]
	GameObject nameView;

	void Awake() {
		nameText = nameView.gameObject.GetComponent<Text>();
		nameText.color = Color.white;
	}

	public string AvatarName {
		get { return nameText.text; }
		set { nameText.text = string.Format("{0}", value); 
		}
	}
}