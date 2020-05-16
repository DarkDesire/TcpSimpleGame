using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Avatar))]

public class Health : MonoBehaviour
{
	private int _current;
	private int _maximum;
	private Animator anim;
	private Avatar avatar;
	private int zero = 0;

	private List<HealthView> healthViews = new List<HealthView>();

	void Awake(){
		// Set up references.
		avatar = GetComponent <Avatar> ();

		HealthView localHealthView = GetComponentInChildren<HealthView> ();
		healthViews.Add (localHealthView);
	}

	public void AddGlobalHealthView(HealthView healthView){
		healthViews.Add (healthView);
	}

	public int Current {
		get { return _current;}
		set { 
			if (value == Maximum) {
				if (_current != Maximum) {
					avatar.SetAnimStateToIdle ();
				}
			} else if (value == zero) {
				if (avatar.isMine) {
					avatar.SetAnimStateToDie ();
				}
			}
			_current = value;
			if (healthViews.Count > 0) {
				foreach (HealthView healthView in healthViews) {
					healthView.Current = _current;
				}
			}
		}
	}

	public int Maximum {
		get { return _maximum;}
		set { _maximum = value;
			if (healthViews.Count > 0) {
				foreach (HealthView healthView in healthViews) {
					healthView.Maximum = _maximum;
				}
			}
		}
	}

	public void addHealthView(HealthView healthView) {
		healthViews.Add (healthView);
	}
}


