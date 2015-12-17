using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;
using UnityEngine.UI;

public class ChatSetup : MonoBehaviour,IChatClientListener {
	
	public InputField playerName;
	public InputField chatText;
	ChatClient chatClient;
	public Text buttonText;

	public Text mainChat;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (chatClient != null)
        {
            chatClient.Service();
        }
		
	}
	
	public void ConnectButtonClicked()
	{
		chatClient = new ChatClient(this);
		chatClient.Connect("34ec6cd9-6860-4767-bcc6-56311304bbda","1.0",new AuthenticationValues(playerName.text.ToString()));
		
	}
	
	public void sendMessageClicked()
	{
		chatClient.PublishMessage( "Global", chatText.text.ToString() );
	}
	
	public void OnConnected()
    {
		chatClient.Subscribe(new string[] {"Global"});
    }
	
	public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            UnityEngine.Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        else
        {
            UnityEngine.Debug.Log(message);
        }
    }
	
	public void OnDisconnected()
    {
		
    }
	public void OnChatStateChange(ChatState state)
	{
		buttonText.text = state.ToString();
	}
	public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
		for ( int i = 0; i < senders.Length; i++ )
		{
			msgs += senders[i] + "=" + messages[i] + ", ";
		}
		//Console.WriteLine( "OnGetMessages: " + channelName + "(" + senders.Length + ") > " + msgs );
		mainChat.text = mainChat.text + " " + msgs;	
	}
	
	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		
	}
	public void OnSubscribed(string[] channels, bool[] results)
	{
		
	}
	
	public void OnUnsubscribed(string[] channels)
	{
		
	}
	
	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		
	}
}
