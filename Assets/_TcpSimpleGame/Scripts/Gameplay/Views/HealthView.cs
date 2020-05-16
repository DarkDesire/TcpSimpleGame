using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthView : MonoBehaviour
{
	[SerializeField]
	Slider healthSlider;

	private int _current;
	private int _maximum;
	private bool _IsDead;
	private int minimum = 0;
	private int maximumStart = 1;

	void Awake(){
		healthSlider = GetComponentInChildren<Slider>();
		healthSlider.minValue = minimum;
		healthSlider.maxValue = maximumStart;
		healthSlider.value = maximumStart;
	}

	public int Current {
		set { _current = value;
			healthSlider.value = _current;
		}
	}

	public int Maximum {
		set {
			_maximum = value;
			healthSlider.maxValue = _maximum;
		}
	}

}


