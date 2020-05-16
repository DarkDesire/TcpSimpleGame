using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GuildNameView : MonoBehaviour
{
	Text nameText;
	[SerializeField]
	GameObject nameView;

	void Awake() {
		nameText = nameView.gameObject.GetComponent<Text>();
		nameText.color = Color.grey;
	}

	public string GuildName {
		get { return nameText.text; }
		set { 
			if (value == string.Empty) {
				nameView.SetActive(false);
			} else {
				nameView.SetActive(true);
			}
			nameText.text = string.Format("<{0}>", value); 
		}
	}
}