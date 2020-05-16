using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manna : MonoBehaviour
{
	private int _current;
	private int _maximum;
	private List<MannaView> mannaViews = new List<MannaView>();

	void Awake(){
		MannaView localMannaView = GetComponentInChildren<MannaView> ();
		mannaViews.Add (localMannaView);
	}

	public void AddGlobalMannaView(MannaView mannaView){
		mannaViews.Add (mannaView);
	}


	public int Current {
		get { return _current;}
		set { _current = value;
			foreach (MannaView healthView in mannaViews) {
				healthView.Current = _current;
			}
		}
	}

	public int Maximum {
		get { return _maximum;}
		set { _maximum = value;
			foreach (MannaView healthView in mannaViews) {
				healthView.Maximum = _maximum;
			}
		}
	}

	public void addMannaView(MannaView healthView) {
		mannaViews.Add (healthView);
	}


}


