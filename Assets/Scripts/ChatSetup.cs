using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using UnityEngine.UI;


public class ChatSetup : MonoBehaviour,IChatClientListener {
	
	public InputField chatText;
	ChatClient chatClient;
	public Text buttonText;
	public GameObject chatBlockObject;
	public GameObject scrollableChat;
	int numberOfMessages = 0;
	float gapSize = 42.0f;
	List<GameObject> chatBlocks;
	public Sprite[] sprites;
	public GameObject connectButtons;
	public Text statusText;

	// Use this for initialization
	void Start () {
		
		chatBlocks = new List<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (chatClient != null)
        {
            chatClient.Service();
        }
		
	}
	
	public void connectAsBMoney()
	{
		chatClient = new ChatClient(this);
		chatClient.Connect("34ec6cd9-6860-4767-bcc6-56311304bbda","1.0",new AuthenticationValues("BMoney"));
	}
	
	public void connectAsReyles()
	{
		chatClient = new ChatClient(this);
		chatClient.Connect("34ec6cd9-6860-4767-bcc6-56311304bbda","1.0",new AuthenticationValues("Reyles"));
	}
	
	public void connectAsSaintKW()
	{
		chatClient = new ChatClient(this);
		chatClient.Connect("34ec6cd9-6860-4767-bcc6-56311304bbda","1.0",new AuthenticationValues("SaintKW"));
	}
	
	void hideConnectButtons()
	{
		connectButtons.SetActive(false);
	}
	
	public void sendMessageClicked()
	{
		chatClient.PublishMessage( "Global", chatText.text.ToString() );
		chatText.text = "";
		chatText.interactable = false;
	}
	
	public void OnConnected()
    {
		chatClient.Subscribe(new string[] {"Global"},3);
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
		statusText.text = state.ToString();
		if(state == ChatState.ConnectedToFrontEnd)
		{
			hideConnectButtons();
		}
	}
	public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
		for ( int i = 0; i < senders.Length; i++ )
		{
			PopTextInChat(senders[i],messages[i].ToString());
		}
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
	
	void PopTextInChat(string playerName,string message)
	{
		
		// Move the old Chat down to make way for the new one.
		if(chatBlocks.Count > 0)
		{
			for(int i=0;i<chatBlocks.Count;i++)
			{
				Vector3 chatBlockPosition = chatBlocks[i].GetComponent<RectTransform>().anchoredPosition3D;
				chatBlockPosition.x = 0.0f;
				chatBlockPosition.y -= gapSize;
				LeanTween.move(chatBlocks[i].GetComponent<RectTransform>(),chatBlockPosition,0.5f);
			}
		}
		
		//Create Chat Block
		GameObject tempChatBlock = Instantiate(chatBlockObject,Vector3.zero,Quaternion.identity) as GameObject;
		ChatBlock cb = tempChatBlock.GetComponent<ChatBlock>();
		cb.setAlphaAsZero();
		
		//I got slightly confused with Player names... 
		//Since I saw hardcoded names on images...so for testing purposes..
		//Hence, I Hardcoded it!
		if(playerName == "BMoney")
		{
			cb.setImage(sprites[0]);
		}
		else if(playerName == "Reyles")
		{
			cb.setImage(sprites[1]);
		}
		else if(playerName == "SaintKW")
		{
			cb.setImage(sprites[2]);
		}
		
		cb.setText(message);
		cb.fadeInBlock();

		//Reset
		tempChatBlock.transform.SetParent(scrollableChat.transform);
		tempChatBlock.GetComponent<RectTransform>().localScale = Vector3.one;
		tempChatBlock.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

		//Scrollable Chat Size
		Vector2 tempSize = scrollableChat.GetComponent<RectTransform>().sizeDelta;
		tempSize.y += gapSize;
		scrollableChat.GetComponent<RectTransform>().sizeDelta = tempSize;
		
		//Add block to the list
		chatBlocks.Add(tempChatBlock);
		
		chatText.interactable = true;
		numberOfMessages++;
			
	}
	
}
