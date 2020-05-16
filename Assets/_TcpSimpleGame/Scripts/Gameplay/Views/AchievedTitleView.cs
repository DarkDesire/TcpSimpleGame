using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievedTitleView : MonoBehaviour
{
	Text titleText;
	[SerializeField]
	GameObject achievedTitleView;

	void Awake() {
		titleText = achievedTitleView.gameObject.GetComponent<Text>();
		titleText.color = Color.blue;
	}

	public string AchievedTitle {
		get { return titleText.text; }
		set { 
			if (value == string.Empty) {
				achievedTitleView.SetActive(false);
			} else {
				achievedTitleView.SetActive(true);
			}
			titleText.text = string.Format("{0}", value); 
		}
	}
}


