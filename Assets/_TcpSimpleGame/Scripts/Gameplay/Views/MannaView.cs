using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MannaView : MonoBehaviour
{
	[SerializeField]
	Slider mannaSlider;

	private int _current;
	private int _maximum;
	private int minimum = 0;
	private int maximumStart = 1;

	void Awake(){
		mannaSlider = GetComponentInChildren<Slider>();
		mannaSlider.minValue = minimum;
		mannaSlider.maxValue = maximumStart;
		mannaSlider.value = maximumStart;
	}

	public int Current {
		set { _current = value;
			mannaSlider.value = _current;
		}
	}

	public int Maximum {
		set {
			_maximum = value;
			mannaSlider.maxValue = _maximum;
		}
	}

}


