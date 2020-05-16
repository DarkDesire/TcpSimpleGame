
 
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
 
public class Connection : MonoBehaviour {
 
	public bool isLocal = false;
	public string localIP = "127.0.0.1";
	public string publicIP = "82.146.34.100";
	public int port = 8081;
	private NetworkManager networkManager;

	void Awake() {
		networkManager = FindObjectOfType<NetworkManager>();
	}

    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
    private byte[] _recieveBuffer = new byte[8142];
 
	public void SetupServer()
    {
        try
        {
			var ip = (isLocal) ? localIP : publicIP;
			_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip),port));
        }
        catch(SocketException ex)
        {
            Debug.Log(ex.Message);
        }
 
        _clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
 
    }
 
    private void ReceiveCallback(IAsyncResult AR)
    {
        //Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = _clientSocket.EndReceive(AR);
 
        if(recieved <= 0)
            return;
 
        //Copy the recieved data into new buffer , to avoid null bytes
        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_recieveBuffer,0,recData,0,recieved);
 
        //Process data here the way you want , all your bytes will be stored in recData
		ProccessReceivedData(recData);
        //Start receiving again
        _clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
    }
 
	public void SendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data,0,data.Length);
        _clientSocket.SendAsync(socketAsyncData);
    }

	public void ProccessReceivedData(byte[] data)
	{
		PacketMSG packet = PacketMSG.Parser.ParseFrom(ByteString.CopyFrom(data));
		networkManager.getPacket (packet);
	}
}
 