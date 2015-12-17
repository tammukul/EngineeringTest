using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatBlock : MonoBehaviour {
	
	public Image profileImage;
	public Text writtenText;
	
	// Use this for initialization
	void Awake () {
		
	}
	
	public void setImage(Sprite tempImage)
	{
		profileImage.sprite = tempImage;
	}
	
	public void setText(string tempText)
	{
		writtenText.text = tempText;
	}
	
	public void setAlphaAsZero()
	{
		profileImage.CrossFadeAlpha(0.0f,0.0f,true);
		writtenText.CrossFadeAlpha(0.0f,0.0f,true); 
	}
	
	public void fadeInBlock()
	{
		profileImage.CrossFadeAlpha(1.0f,0.6f,true);
		writtenText.CrossFadeAlpha(1.0f,0.6f,true);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
