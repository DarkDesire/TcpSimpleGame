using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PreviewAvatarScript : MonoBehaviour {
	
	private int id;
	private string _name;
	private int level;

	private Text nameText;
	private Text levelText;
	private Text[] textArray;

	private Button buttonToLobby;

	private UIManager managerUI { get { return UIManager.Instance; } }
	private NetworkManager networkManager;

	void Awake(){			
		networkManager = FindObjectOfType<NetworkManager>();
		if (networkManager == null) {
			Debug.Log ("Didnt find networkManager");
		}

		buttonToLobby = GetComponentInChildren<Button> ();
		if (buttonToLobby == null) {
			Debug.Log ("Didnt find buttonToLobby");
		}
	}

	public int Id {
		get { return id; }
		set { id = value; }
	}

	public string Name {
		get { return _name; }
		set { _name = value; 
			if (nameText == null) {
				nameText = transform.GetComponentsInChildren<Text> () [0];
			}
			nameText.text = value;
		}
	}
		
	public int Level {
		get { return level; }
		set { level = value; 
			if (levelText == null) {
				levelText = transform.GetComponentsInChildren<Text> () [1];
			}
			levelText.text = string.Format ("Level: {0}", value);
		}
	}
		
	void Start () {
		buttonToLobby.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		managerUI.SetStateLobby ();
		networkManager.avatarId = Id;
	}
}
