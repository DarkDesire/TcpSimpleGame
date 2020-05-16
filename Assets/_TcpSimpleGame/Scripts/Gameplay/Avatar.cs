using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
public class Avatar: MonoBehaviour {

	enum AnimStateEnum { Idle = 1, Walking = 2, Die = 3 };
	AnimStateEnum _state = AnimStateEnum.Idle;

	private GameplayWindow gameplayWindow				{ get { return GameplayWindow.Instance; } }

	private int _avatarId;
	private int _familyId;
	private int _animState;
	private string _name;
	private string _familyName;
	private int _level;
	private Health _health;
	private Manna _manna;

	private AvatarNameView avatarNameView;
	private FamilyNameView familyNameView;
	private Animator anim;

	[HideInInspector]
	public bool isMine = false;
	private AvatarMovement avatarMovement;
	private AvatarShooting avatarShooting;
	private Rigidbody avatarRigidbody;
	private CapsuleCollider capsuleCollider;


	void Awake(){
		avatarNameView = GetComponentInChildren<AvatarNameView> ();
		familyNameView = GetComponentInChildren<FamilyNameView>();
		anim = GetComponent<Animator> ();
		avatarMovement = GetComponent<AvatarMovement> ();
		avatarRigidbody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		avatarShooting = GetComponentInChildren<AvatarShooting> ();
	}

	//private GuildNameView guildNameView { get { gameObject.GetComponent<GuildNameView> (); }}
	//private AchievedTitleView achievedTitleView { get { gameObject.GetComponent<AchievedTitleView> (); }}

	public int AvatarId {
		get { return _avatarId;}
		set { _avatarId = value;}
	}
	public int FamilyId {
		get { return _familyId;}
		set { _familyId = value;}
	}
	public string AvatarName {
		get { return _name;}
		set { 
			_name = value;
			avatarNameView.AvatarName = value;
		}
	}
	public string FamilyName {
		get { return _familyName;}
		set { 
			_familyName = value;
			familyNameView.FamilyName = value;
		}
	}
	public int Level {
		get { return _level;}
		set { _level = value;}
	}

	public Health Health {
		private get { return _health;}
		set { _health = value;}
	}
	public Manna Manna {
		private get { return _manna;}
		set { _manna = value;}
	}

	private void SetState(AnimStateEnum value) {
		AnimStateEnum previousState = _state;
		int newStateInt = (int)value;
		switch (newStateInt) {
		case (int)AnimStateEnum.Idle:
			if (previousState != AnimStateEnum.Idle) {
				if (isMine) {
					avatarMovement.enabled = true;
					avatarShooting.enabled = true;
				}

				avatarRigidbody.isKinematic = false;
				capsuleCollider.enabled = true;

				anim.SetTrigger ("Idle");
			}
			break;
		case (int)AnimStateEnum.Walking:
			if (previousState != AnimStateEnum.Walking) {
				anim.SetTrigger ("Walking");
			}
			break;
		case (int)AnimStateEnum.Die:
			if (previousState != AnimStateEnum.Die) {
				if (isMine) {
					avatarMovement.enabled = false;
					avatarShooting.enabled = false;
					gameplayWindow.RespawnCoroutineOn ();
				} 

				avatarRigidbody.isKinematic = true;
				capsuleCollider.enabled = false;

				anim.SetTrigger ("Die");
			}
			break;
		}
		_state = (AnimStateEnum)value;
	}

	public void SetAnimStateToIdle(){
		SetState (AnimStateEnum.Idle);
	}
	public void SetAnimStateToWalking(){
		SetState (AnimStateEnum.Walking);
	}
	public void SetAnimStateToDie(){
		SetState (AnimStateEnum.Die);
	}

	public int AnimState {
		set { 
			SetState ((AnimStateEnum)value);
		}
		get { return (int)_state; }
	}

}