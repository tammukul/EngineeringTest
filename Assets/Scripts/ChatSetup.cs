using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using ExitGames.Client.Photon.Chat;
using UnityEngine.UI;


public class ChatSetup : MonoBehaviour,IChatClientListener {
	
	public InputField chatText;
	ChatClient chatClient;
	public Text buttonText;
	public GameObject chatBlockObject;
	public GameObject scrollableChat;
	float gapSize = 42.0f;
	List<GameObject> chatBlocks;
	public Sprite[] sprites;
	public GameObject connectButtons;
	public Text statusText;
	public GameObject chatBackground;
	Vector3 originalPosScrollPanel,originalPosChatBackground;
	public GameObject scrollPanel;
	public GameObject hiddenChatBoxButton;
	public int maximumNumberOfMsgs = 100;
	// Use this for initialization
	void Start () {
		
		chatBlocks = new List<GameObject>();
		originalPosScrollPanel = scrollPanel.GetComponent<RectTransform>().anchoredPosition3D;
		originalPosChatBackground = chatBackground.GetComponent<RectTransform>().anchoredPosition3D;
		
		Vector3 movePositionTo = chatBackground.GetComponent<RectTransform>().anchoredPosition3D;
		movePositionTo.x = -200.0f;
		chatBackground.GetComponent<RectTransform>().anchoredPosition3D = movePositionTo;
		movePositionTo = originalPosScrollPanel;
		movePositionTo.y = -159.0f;
		scrollPanel.GetComponent<RectTransform>().anchoredPosition3D = movePositionTo;
		scrollPanel.GetComponent<ScrollRect>().vertical = false;
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
		
		if(!chatText.wasCanceled)
		{
			chatClient.PublishMessage( "Global", chatText.text.ToString() );
			chatText.text = "";
			chatText.interactable = false;
		}
		else
		{
			chatText.text = "";
		}
	}
	
	IEnumerator ShowChat(string[] senders, object[] messages)
	{
		for ( int i = 0; i < senders.Length; i++ )
		{
			StartCoroutine(PopText(senders[i],messages[i].ToString()));
			yield return new WaitForSeconds(0.6f);
		}
	}
	IEnumerator PopText(string playerName,string message)
	{
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
		// Wait for the leantween animation to complete..
		yield return new WaitForSeconds(0.5f);
		
		//Create Chat Block if the maximum number of msgs is not met otherwise resuse the oldest chat block.
		GameObject tempChatBlock;
		if(chatBlocks.Count != maximumNumberOfMsgs)
		{
			tempChatBlock = Instantiate(chatBlockObject,Vector3.zero,Quaternion.identity) as GameObject;
		}
		else
		{
			tempChatBlock = chatBlocks[0];
			chatBlocks.RemoveAt(0);
		}
		ChatBlock cb = tempChatBlock.GetComponent<ChatBlock>();
		cb.setAlphaAsZero();

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
		if(chatBlocks.Count != maximumNumberOfMsgs)
		{
			Vector2 tempSize = scrollableChat.GetComponent<RectTransform>().sizeDelta;
			tempSize.y += gapSize;
			scrollableChat.GetComponent<RectTransform>().sizeDelta = tempSize;
		}
		
		//Add block to the list
		chatBlocks.Add(tempChatBlock);
		
		chatText.interactable = true;
	}
	
	public void openChat()
	{
		hiddenChatBoxButton.SetActive(false);
		scrollPanel.GetComponent<ScrollRect>().vertical = true;
		LeanTween.move(scrollPanel.GetComponent<RectTransform>(),originalPosScrollPanel,0.5f);
		LeanTween.move(chatBackground.GetComponent<RectTransform>(),originalPosChatBackground,0.5f);
	}
	
	public void closeChat()
	{
		hiddenChatBoxButton.SetActive(true);
		
		Vector3 movePositionTo = chatBackground.GetComponent<RectTransform>().anchoredPosition3D;
		movePositionTo.x = -200.0f;
		LeanTween.move(chatBackground.GetComponent<RectTransform>(),movePositionTo,1.0f);
		
		movePositionTo = originalPosScrollPanel;
		movePositionTo.y = -159.0f;
		LeanTween.move(scrollPanel.GetComponent<RectTransform>(),movePositionTo,1.0f);
	}



	// Listener Functions for the chat module
	public void OnConnected()
    {
		chatClient.Subscribe(new string[] {"Global"},10);
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
		if(state == ChatState.Disconnected)
		{
			connectButtons.SetActive(true);
		}
	}
	public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
		StartCoroutine(ShowChat(senders,messages));
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
