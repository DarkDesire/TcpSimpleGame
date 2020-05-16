using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NameView : MonoBehaviour
{
	Text nameText;

	void Awake() {
		nameText = GetComponent<Text>();
	}

	public string Guild {
		get { return nameText.text; }
		set { nameText.text = string.Format("{0}", value); }
	}
}


