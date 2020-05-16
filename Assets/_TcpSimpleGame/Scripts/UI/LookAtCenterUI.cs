using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LookAtCenterUI : MonoBehaviour
{
    private RectTransform _transform;
	private Transform UIposition;

    private Vector3 _screenCenter;
    void Start()
    {
		_transform = GetComponent<RectTransform>();
		UIposition = GameObject.FindGameObjectWithTag ("UIposition").transform;
    }

    void LateUpdate()
    {
		_transform.LookAt(UIposition);
    }
}
