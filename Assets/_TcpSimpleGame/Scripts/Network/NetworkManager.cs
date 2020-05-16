using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Google.Protobuf;


public class NetworkManager: MonoBehaviour
{
	// GUI
	private UIManager managerUI							{ get { return UIManager.Instance; } }
	private LoginWindow loginWindow						{ get { return LoginWindow.Instance; } }
	private SetFamilyWindow setFamilyWindow 			{ get { return SetFamilyWindow.Instance; } }
	private ChooseAvatarWindow chooseCharacterWindow 	{ get { return ChooseAvatarWindow.Instance; } }
	private SetNewAvatarWindow setNewCharacterWindow	{ get { return SetNewAvatarWindow.Instance; } }
	private GameplayWindow gameplayWindow				{ get { return GameplayWindow.Instance; } }

	// prefabs
	[SerializeField]
	GameObject playerPrefab = null;
	[SerializeField]
	GameObject enemyPrefab = null;
	[SerializeField]
	GameObject mannaUIPrefab = null;
	[SerializeField]
	GameObject healthUIPrefab = null;
	// camera
	[SerializeField]
	GameObject gameplayCamera  = null;
	// local
	private Dictionary<int, GameObject> players = new Dictionary<int, GameObject> ();
	// network
	private Queue msgQueue;
	[HideInInspector]
	public int avatarId;

	private Connection connection;

	void Awake ()
	{
		Application.runInBackground = true;
		connection = GetComponent<Connection> ();
		msgQueue = new Queue ();
	}

	void Start ()
	{
		StartCoroutine ("MainCycle");
	}

	IEnumerator MainCycle ()
	{
		while (true) {
			if (msgQueue.Count < 1) {
				// haven't any packets
				yield return new WaitForSeconds (0.01f);
			} else {
				PacketMSG receivedPacket = (PacketMSG)msgQueue.Dequeue ();
				int packetId = receivedPacket.Cmd;
				Cmd cmd = (Cmd)packetId;
				switch (cmd) {
				case Cmd.CmdPing:
					break;
				case Cmd.CmdLogin:
					break;
				case Cmd.CmdLoginSuccess:
					break;
				case Cmd.CmdLoginFailed:
					break;
				case Cmd.CmdSetFamilyRequest:
					managerUI.SetStateSetFamily ();
					break;
				case Cmd.CmdSetFamily:
					break;
				case Cmd.CmdSetFamilySuccess:
					break;
				case Cmd.CmdSetFamilyFailed:
					break;
				case Cmd.CmdPreviewAvatar:
					break;
				case Cmd.CmdPreviewAvatars:
					
					// очень грязный хак
					chooseCharacterWindow.ClearSubs();
					// </>

					PacketPreviewAvatars packetPreviewAvatars = PacketPreviewAvatars.Parser.ParseFrom (receivedPacket.Data);
					foreach (PacketPreviewAvatar packetPreviewAvatar in packetPreviewAvatars.Avatars) {
						chooseCharacterWindow.AddCharacter (packetPreviewAvatar.AvatarId, packetPreviewAvatar.Name, packetPreviewAvatar.Level);
					}
					int startArrayId = (packetPreviewAvatars.Avatars.Count == 0) ? 1 : packetPreviewAvatars.Avatars.Count;
					for (int avatarArrayId = startArrayId; avatarArrayId <= packetPreviewAvatars.MaxAvatars ; avatarArrayId++) {
						if (avatarArrayId <= packetPreviewAvatars.AvailableAvatars) {
							chooseCharacterWindow.AddAvailable ();
						} else {
							chooseCharacterWindow.AddDisable ();
						}
					} 

					managerUI.SetStateChooseAvatar ();
					break;
				case Cmd.CmdSetNewAvatar:
					managerUI.SetStateSetNewAvatar ();
					break;
				case Cmd.CmdSetNewAvatarSuccess:
					break;
				case Cmd.CmdSetNewAvatarFailed:
					break;
				case Cmd.CmdJoinToChannel:
					break;
				case Cmd.CmdAvatar:
					break;
				case Cmd.CmdAvatars:
					// action
					PacketAvatars packetAvatars = PacketAvatars.Parser.ParseFrom (receivedPacket.Data);

					foreach (PacketAvatar packetAvatar in packetAvatars.Avatars) {
						handlePacketAvatar (packetAvatar, respawn: false);
					}
					break;
				case Cmd.CmdJoinedAvatar:
					if (players.Count == 0) {
						managerUI.SetStateGameplay ();
					}

					PacketJoinedAvatar packetJoinedAvatar = PacketJoinedAvatar.Parser.ParseFrom (receivedPacket.Data);
					PacketAvatar packetAvatarJA = packetJoinedAvatar.Avatar;
					PacketTransform packetTransformJA = packetAvatarJA.Transform;
					Vector3 receivedPositionJA = new Vector3 (packetAvatarJA.Transform.Position.X, packetAvatarJA.Transform.Position.Y, packetAvatarJA.Transform.Position.Z);
					Vector3 receivedRotationJA = new Vector3 (0f, packetAvatarJA.Transform.Rotation.Yaw, 0f);

					GameObject newPlayer;

					if (packetAvatarJA.AvatarId == this.avatarId) { // if it's our player, we spawn prefab with input controller
						newPlayer = (GameObject)Instantiate (playerPrefab, receivedPositionJA, Quaternion.identity);
						CameraFollow cameraScript = gameplayCamera.AddComponent<CameraFollow> ();
						cameraScript.OnAwake (newPlayer.transform);
					} else { // otherwise prebaf enemy without input controller
						newPlayer = (GameObject)Instantiate (enemyPrefab, receivedPositionJA, Quaternion.identity);
					}

					newPlayer.name = packetAvatarJA.Name;
					newPlayer.transform.position = receivedPositionJA;
					newPlayer.transform.eulerAngles = receivedRotationJA;

					// add health Avatar script to avatar
					Avatar avatarScript = newPlayer.GetComponent<Avatar> ();
					if (packetAvatarJA.AvatarId == this.avatarId) { // if it's our player
						avatarScript.isMine = true;
					}
					avatarScript.AvatarId = packetAvatarJA.AvatarId;
					avatarScript.AvatarName = packetAvatarJA.Name;
					avatarScript.FamilyId = packetAvatarJA.FamilyId;
					avatarScript.FamilyName = packetAvatarJA.FamilyName;
					avatarScript.Level = packetAvatarJA.Level;
					avatarScript.AnimState = packetAvatarJA.AnimState;

					///////////////////////////////////////////////////////////////////////// HEALTH
					// add Health script to avatar
					Health health = newPlayer.GetComponent<Health> ();

					///////////////////////////////////////////////////////////////////////// MANNA
					// add manna script to avatar
					Manna manna = newPlayer.GetComponent<Manna> ();

					// gameplay UI
					HealthView globalHealthView = null;
					MannaView globalMannaView = null;

					if (packetAvatarJA.AvatarId == this.avatarId) { // if it's our player, we spawn prefab with input controller
						// find avatar bars
						GameObject avatarBars = GameObject.FindGameObjectWithTag ("AvatarBars");

						// add health UI
						GameObject globalHealthUI = (GameObject)Instantiate (healthUIPrefab, Vector3.zero, Quaternion.identity);
						globalHealthUI.transform.SetParent (avatarBars.transform, false);
						globalHealthView = globalHealthUI.GetComponent<HealthView> ();
						health.AddGlobalHealthView (globalHealthView);

						// add manna UI
						GameObject globalMannaUI = (GameObject)Instantiate (mannaUIPrefab, Vector3.zero, Quaternion.identity);
						globalMannaUI.transform.SetParent (avatarBars.transform, false);
						globalMannaView = globalMannaUI.GetComponent<MannaView> ();
						manna.AddGlobalMannaView (globalMannaView);

					}

					avatarScript.Health = health;
					health.Maximum = packetAvatarJA.Health.Maximum;
					health.Current = packetAvatarJA.Health.Current;

					avatarScript.Manna = manna;
					manna.Maximum = packetAvatarJA.Manna.Maximum;
					manna.Current = packetAvatarJA.Manna.Current;
					CapsuleCollider capsuleCollider = newPlayer.GetComponent<CapsuleCollider> ();
					capsuleCollider.enabled = true;
					players.Add (packetAvatarJA.AvatarId, newPlayer);
	
					break;
				case Cmd.CmdShootToAvatar:
					break;
				case Cmd.CmdShootSuccess:
					PacketShootSuccess packetShootSuccess = PacketShootSuccess.Parser.ParseFrom (receivedPacket.Data);
					PacketAvatar senderAvatarOne = packetShootSuccess.SenderAvatar;
					PacketPosition packetPosition = packetShootSuccess.ShootEndPosition;
					Vector3 shootEndPosition = new Vector3 (packetPosition.X, packetPosition.Y, packetPosition.Z);

					ShootSync shootSyncOne = players [senderAvatarOne.AvatarId].GetComponentInChildren<ShootSync> ();
					shootSyncOne.ShootOn (shootEndPosition);
					break;
				case Cmd.CmdShootToAvatarSuccess:
					PacketShootToAvatarSuccess packetShootToAvatarSuccess = PacketShootToAvatarSuccess.Parser.ParseFrom (receivedPacket.Data);
					int senderShootAvatarId = packetShootToAvatarSuccess.SenderShootAvatarId;
					int receiverShootAvatarId = packetShootToAvatarSuccess.ReceiverShootAvatarId;

					ShootSync shootSyncTwo = players [senderShootAvatarId].GetComponentInChildren<ShootSync> ();
					shootSyncTwo.ShootOn (players [receiverShootAvatarId].transform.position);
					break;
				case Cmd.CmdShootFailed:
					Debug.Log ("Shoot failed");
					break;
				case Cmd.CmdKilledBy:
					Debug.Log ("CmdKilledBy");
					PacketKilledBy packetKilledBy = PacketKilledBy.Parser.ParseFrom (receivedPacket.Data);
					int killerAvatarId = packetKilledBy.KillerAvatarId;
					int victimAvatarId = packetKilledBy.VictimAvatarId;

					if (victimAvatarId == this.avatarId) {
						gameplayWindow.KilledByCoroutineOn(string.Format ("You were killed by {0}", players [killerAvatarId].name));
					} else {
						gameplayWindow.KilledByCoroutineOn(string.Format ("{0} is killed by {1}", players [victimAvatarId].name, players [killerAvatarId].name));
					}

					break;
				case Cmd.CmdRespawn:
					PacketRespawn packetRespawn = PacketRespawn.Parser.ParseFrom (receivedPacket.Data);
					handlePacketAvatar (packetRespawn.Avatar, respawn: true);
					break;
				case Cmd.CmdAvatarLeft:
					PacketAvatarLeft packetAvatarLeft = PacketAvatarLeft.Parser.ParseFrom (receivedPacket.Data);
					GameObject link = players [packetAvatarLeft.AvatarId];
					players.Remove (packetAvatarLeft.AvatarId);
					DestroyImmediate (link);
					break;
				case Cmd.CmdBanned:
					break;
				case Cmd.CmdDisconnect:
					break;
				default:
					break;
				} // end switch
			} // end else
			yield return new WaitForSeconds (0.01f);
		}
	}
	// end MainCycle

	private void handlePacketAvatar(PacketAvatar packetAvatar, bool respawn) 
	{
		Vector3 receivedPosition = new Vector3 (packetAvatar.Transform.Position.X, packetAvatar.Transform.Position.Y, packetAvatar.Transform.Position.Z);
		Vector3 receivedRotation = new Vector3 (0f, packetAvatar.Transform.Rotation.Yaw, 0f);

		int receivedPlayerId = packetAvatar.AvatarId;

		Avatar avatar = players [receivedPlayerId].GetComponent<Avatar> ();
		Health avatarHealth = players [receivedPlayerId].GetComponent<Health> ();
		Manna avatarManna = players [receivedPlayerId].GetComponent<Manna> ();

		avatarHealth.Maximum = packetAvatar.Health.Maximum;
		avatarHealth.Current = packetAvatar.Health.Current;
		avatarManna.Maximum = packetAvatar.Manna.Maximum;
		avatarManna.Current = packetAvatar.Manna.Current;

		GameObject receivedPlayer = players [receivedPlayerId];
		Transform receivedPlayerTransform = receivedPlayer.transform;
		if (receivedPlayerId != this.avatarId) {		
			avatar.AnimState = packetAvatar.AnimState;
			receivedPlayerTransform.position = Vector3.Lerp (receivedPlayerTransform.position, receivedPosition, 0.99f);
			receivedPlayerTransform.eulerAngles = Vector3.Lerp (receivedPlayerTransform.eulerAngles, receivedRotation, 0.99f);
		} else {
			if (respawn) {
				receivedPlayerTransform.position = receivedPosition;
				receivedPlayerTransform.eulerAngles = receivedRotation;
			}
		}
	}

	private void SetupServer ()
	{
		connection.SetupServer ();
	}

	// for UI buttons
	public void SendConnect ()
	{
		SetupServer ();
		PacketLogin login = new PacketLogin{ Email = loginWindow.getEmail (), Pass = loginWindow.getPass () };
		Byte[] loginBA = login.ToByteArray ();
		sendPacket ((int)Cmd.CmdLogin, loginBA);
		managerUI.SetStateWaiting ();
	}
		
	public void SendSetFamily ()
	{
		PacketSetFamily setFamily = new PacketSetFamily{ Family = setFamilyWindow.getFamily() };
		Byte[] setFamilyBA = setFamily.ToByteArray ();
		sendPacket ((int)Cmd.CmdSetFamily, setFamilyBA);
		managerUI.SetStateWaiting ();
	}

	public void SendSetNewAvatar ()
	{
		PacketSetNewAvatar setNewAvatar = new PacketSetNewAvatar { Name = setNewCharacterWindow.getNewAvatar()};
		Byte[] setNewAvatarBA = setNewAvatar.ToByteArray ();
		sendPacket ((int)Cmd.CmdSetNewAvatar, setNewAvatarBA);
		managerUI.SetStateWaiting ();
	}

	public void SendShoot(Vector3 shootEndPosition)
	{
		PacketPosition packetPosition = new PacketPosition {
			X = shootEndPosition.x,
			Y = shootEndPosition.y,
			Z = shootEndPosition.z
		};

		PacketShoot packetShoot = new PacketShoot { ShootEndPosition = packetPosition };
		Byte[] packetShootBA = packetShoot.ToByteArray ();
		sendPacket ((int)Cmd.CmdShoot, packetShootBA);
	}

	public void SendShootToAvatar (int enemyAvatarId)
	{
		PacketShootToAvatar packetShootToAvatar = new PacketShootToAvatar { AvatarId = enemyAvatarId };
		Byte[] packetShootToAvatarBA = packetShootToAvatar.ToByteArray ();
		sendPacket ((int)Cmd.CmdShootToAvatar, packetShootToAvatarBA);
	}

	public void SendJoin ()
	{
		PacketJoinToChannel joinToServer = new PacketJoinToChannel{ AvatarId = avatarId, ChannelId = 1 };
		Byte[] joinToServerBA = joinToServer.ToByteArray ();
		sendPacket ((int)Cmd.CmdJoinToChannel, joinToServerBA);
		managerUI.SetStateWaiting ();
	}

	// for Network getting
	public void getPacket (PacketMSG packet)
	{
		msgQueue.Enqueue (packet);
	}

	// for Network sending
	public void sendAvatarMove ()
	{
		Vector3 position = players [avatarId].transform.position;
		Vector3 rotation = players [avatarId].transform.eulerAngles;

		PacketPosition packetPosition = new PacketPosition { // only last 4 digis after decimal part
			X = (float)System.Math.Round ((double)position.x, 2),
			Y = (float)System.Math.Round ((double)position.y, 2),
			Z = (float)System.Math.Round ((double)position.z, 2)
		};
		PacketRotation packetRotation = new PacketRotation {
			Pitch = (float)System.Math.Round ((double)rotation.x, 2),
			Yaw = (float)System.Math.Round ((double)rotation.y, 2)
		};

		PacketTransform packetTransform = new PacketTransform { Position = packetPosition, Rotation = packetRotation };
		PacketAvatar packetAvatar = new PacketAvatar { AvatarId = this.avatarId, Transform = packetTransform};
		PacketAvatarMove packetAvatarMove = new PacketAvatarMove { Avatar = packetAvatar };
		Byte[] packetAvatarMoveBA = packetAvatarMove.ToByteArray ();
		sendPacket ((int)Cmd.CmdAvatarMove, packetAvatarMoveBA);
	}

	public void sendAvatarAnimChange()
	{
		int animState = players [avatarId].GetComponent<Avatar> ().AnimState;
		PacketAvatar packetAvatar = new PacketAvatar { AvatarId = this.avatarId, AnimState = animState};
		PacketAvatarAnimChange packetAvatarAnimChange = new PacketAvatarAnimChange { Avatar = packetAvatar };
		Byte[] packetAvatarAnimChangeBA = packetAvatarAnimChange.ToByteArray ();
		sendPacket ((int)Cmd.CmdAvatarAnimChange, packetAvatarAnimChangeBA);
	}

	private void sendPacket (int command, byte[] data)
	{
		PacketMSG packet = new PacketMSG { Cmd = command, Data = ByteString.CopyFrom (data) };
		Byte[] packetBA = packet.ToByteArray ();
		connection.SendData (packetBA);
	}

	void OnApplicationQuit ()
	{
		sendPacket ((int)Cmd.CmdDisconnect, new byte[0]);
	}
}

public enum Cmd
{
	CmdPing			= 1,

	CmdLogin		= 11,
	CmdLoginSuccess	= 12,
	CmdLoginFailed	= 13,

	CmdSetFamilyRequest 	= 21,
	CmdSetFamily 			= 22,
	CmdSetFamilySuccess 	= 23,
	CmdSetFamilyFailed 		= 24,
	CmdPreviewAvatar 		= 25,
	CmdPreviewAvatars 		= 26,
	CmdSetNewAvatar 		= 27,
	CmdSetNewAvatarSuccess	= 28,
	CmdSetNewAvatarFailed	= 29,

	CmdJoinToChannel 		= 31,

	CmdAvatar				= 41,
	CmdAvatars				= 42,
	CmdJoinedAvatar			= 43,
	CmdAvatarMove			= 44,
	CmdAvatarAnimChange		= 45,
	CmdShoot				= 46,
	CmdShootToAvatar		= 47,
	CmdShootSuccess			= 48,
	CmdShootToAvatarSuccess	= 49,
	CmdShootFailed			= 50,
	CmdKilledBy				= 51,
	CmdRespawn				= 52,

	CmdAvatarLeft	= 61,
	CmdBanned 		= 62,
	CmdDisconnect	= 63
}