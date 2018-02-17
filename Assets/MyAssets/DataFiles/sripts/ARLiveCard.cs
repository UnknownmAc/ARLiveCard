using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.Video;
using System;

public class ARLiveCard : MonoBehaviour, IVirtualButtonEventHandler {
	public GameObject videoPlaneGo;
	public GameObject playButton;
	public GameObject gmail_cube, facebook_cube, map_cube, calendar_cube;


	static bool bIsFacebookOpened;
	static bool bIsGmailOpened;
	static bool bIsMapOpened;
	static bool bIsCalendarOpened;

	bool playButtonClicked = false;

	RaycastHit hit;
	Ray ray;

	void SendEmail()
	{
		string email = "random@gmail.com";
		string subject = MyEscapeURL("Congratulations!");
		string body = MyEscapeURL("Heartiest congratulations! I would love to be a part of your special day.\nRegards");
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}

	string MyEscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}

	void updateVideoPlayState(bool isPlaying) {
		playButton.gameObject.SetActive (isPlaying);
		videoPlaneGo.SetActive (!isPlaying);
	}

	// Use this for initialization
	void Start () {
		VirtualButtonBehaviour[] vrb = GetComponentsInChildren<VirtualButtonBehaviour> ();

		for (int i = 0; i < vrb.Length; i++) {
			vrb [i].RegisterEventHandler (this);
		}

		videoPlaneGo.SetActive (false);
		gmail_cube.SetActive (false);
		facebook_cube.SetActive (false);
		map_cube.SetActive (false);
		calendar_cube.SetActive (false);

		bIsFacebookOpened = false;
		bIsGmailOpened = false;
		bIsMapOpened = false;
		bIsCalendarOpened = false;
	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out hit))
		{
			Debug.Log(" you clicked on " + hit.collider.gameObject.name);

			bool targetHit = false;

			if (hit.collider.gameObject.name == "facebook_cube" && !bIsFacebookOpened) {
				Application.OpenURL ("https://www.facebook.com");
				bIsFacebookOpened = true;
				targetHit = true;
			} else if (hit.collider.gameObject.name == "gmail_cube" && !bIsGmailOpened) {
				SendEmail ();
				bIsGmailOpened = true;
				targetHit = true;
			} else if (hit.collider.gameObject.name == "map_cube" && !bIsMapOpened) {
				targetHit = true;
				bIsMapOpened = true;
				Application.OpenURL("https://www.google.co.in/maps/dir//Balrampur+Gardens,+23,+Ashok+Marg,+Hazratganj,+Lucknow,+Uttar+Pradesh+226001/@26.8549521,80.8804845,12z/data=!4m8!4m7!1m0!1m5!1m1!1s0x399bfd0c3ba16e03:0x3255312237e4f1cc!2m2!1d80.9505259!2d26.8548162");
			} else if(hit.collider.gameObject.name == "calendar_cube" && !bIsCalendarOpened) {
				bIsCalendarOpened = true;
				targetHit = true;
				Application.OpenURL("https://calendar.google.com/calendar/r/day/2012/11/29");
			}
		}

		VideoPlayer videoPlayer = videoPlaneGo.GetComponentInChildren <VideoPlayer> ();
		if (playButtonClicked && (ulong)videoPlayer.frame >= (videoPlayer.frameCount - 1)) {
			videoPlaneGo.SetActive (false); 
			gmail_cube.SetActive (true);
			facebook_cube.SetActive (true);
			map_cube.SetActive (true);
			calendar_cube.SetActive (true);
		}
	}

	public void OnButtonPressed(VirtualButtonBehaviour vb) {
		if (vb.VirtualButtonName == "VideoButton") {
			Debug.Log ("VideoButton pressed");
			videoPlaneGo.SetActive (true);

		} 
	}

	public void OnButtonReleased(VirtualButtonBehaviour vb) {

	}

	public void HandlePlayButtonClick() {
		if (playButton.gameObject.active) {
			playButtonClicked = true;
			playButton.gameObject.SetActive (false);
			videoPlaneGo.SetActive (true);
		} 
	}
}
