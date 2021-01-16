using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TelloLib;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;

public class TelloController : SingletonMonoBehaviour<TelloController> {

	private static bool isLoaded = false;

	private TelloVideoTexture telloVideoTexture;

	GameObject HandModels;
	LeapPositionGet leappositionget;

	public float handdis = 1000;

	// FlipType is used for the various flips supported by the Tello.
	public enum FlipType
	{

		// FlipFront flips forward.
		FlipFront = 0,

		// FlipLeft flips left.
		FlipLeft = 1,

		// FlipBack flips backwards.
		FlipBack = 2,

		// FlipRight flips to the right.
		FlipRight = 3,

		// FlipForwardLeft flips forwards and to the left.
		FlipForwardLeft = 4,

		// FlipBackLeft flips backwards and to the left.
		FlipBackLeft = 5,

		// FlipBackRight flips backwards and to the right.
		FlipBackRight = 6,

		// FlipForwardRight flips forewards and to the right.
		FlipForwardRight = 7,
	};

	// VideoBitRate is used to set the bit rate for the streaming video returned by the Tello.
	public enum VideoBitRate
	{
		// VideoBitRateAuto sets the bitrate for streaming video to auto-adjust.
		VideoBitRateAuto = 0,

		// VideoBitRate1M sets the bitrate for streaming video to 1 Mb/s.
		VideoBitRate1M = 1,

		// VideoBitRate15M sets the bitrate for streaming video to 1.5 Mb/s
		VideoBitRate15M = 2,

		// VideoBitRate2M sets the bitrate for streaming video to 2 Mb/s.
		VideoBitRate2M = 3,

		// VideoBitRate3M sets the bitrate for streaming video to 3 Mb/s.
		VideoBitRate3M = 4,

		// VideoBitRate4M sets the bitrate for streaming video to 4 Mb/s.
		VideoBitRate4M = 5,

	};

	override protected void Awake()
	{
		if (!isLoaded) {
			DontDestroyOnLoad(this.gameObject);
			isLoaded = true;
		}
		base.Awake();

		Tello.onConnection += Tello_onConnection;
		Tello.onUpdate += Tello_onUpdate;
		Tello.onVideoData += Tello_onVideoData;

		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

	}

	private void OnEnable()
	{
		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();
	}

	private void Start()
	{
		if (telloVideoTexture == null)
			telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

		Tello.startConnecting();
		HandModels = GameObject.Find("HandModels");
		leappositionget = HandModels.GetComponent<LeapPositionGet>();
	}

	void OnApplicationQuit()
	{
		Tello.stopConnecting();
	}

	// Update is called once per frame
	void Update () {
		//forLeap Motion
		float distance = leappositionget.hand;
		double thetaUP = leappositionget.ThetaUP;
		double thetaLR = leappositionget.ThetaLR;
		handdis = distance;
		//Debug.Log(theta);

		//forVR
		//Vector2 PrimaryThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
		//Vector2 SecondaryThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

		//if (Input.GetKeyDown(KeyCode.T) || OVRInput.GetDown(OVRInput.RawButton.A)) {
		//	Tello.takeOff();
		//} else if (Input.GetKeyDown(KeyCode.L) || OVRInput.GetDown(OVRInput.RawButton.B)) {
		//	Tello.land();
		//}

		//for Leap Motion
		if (Input.GetKeyDown(KeyCode.T))
		{
			Tello.takeOff();
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			Tello.land();
		}

		float lx = 0f;
		float ly = 0f;
		float rx = 0f;
		float ry = 0f;

		//forVR
		//if (Input.GetKey(KeyCode.UpArrow) || PrimaryThumbstick.y > 0.8)
		//{
		//    ry = 1;
		//}
		//if (Input.GetKey(KeyCode.DownArrow) || PrimaryThumbstick.y < -0.8)
		//{
		//    ry = -1;
		//}
		//if (Input.GetKey(KeyCode.RightArrow) || PrimaryThumbstick.x > 0.8)
		//{
		//    rx = 1;
		//}
		//if (Input.GetKey(KeyCode.LeftArrow) || PrimaryThumbstick.x < -0.8)
		//{
		//    rx = -1;
		//}
		//if (Input.GetKey(KeyCode.W) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || SecondaryThumbstick.y > 0.8)
		//{
		//    ly = 1;
		//}
		//if (Input.GetKey(KeyCode.S) || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) || SecondaryThumbstick.y < -0.8)
		//{
		//    ly = -1;
		//}
		//if (Input.GetKey(KeyCode.D) || SecondaryThumbstick.x > 0.8)
		//{
		//    lx = 1;
		//}
		//if (Input.GetKey(KeyCode.A) || SecondaryThumbstick.x < -0.8)
		//{
		//    lx = -1;
		//}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			ry = 1;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			ry = -1;
		}
		if (Input.GetKey(KeyCode.RightArrow) || thetaLR > 45)
		{
			rx = 1;
		}
		if (Input.GetKey(KeyCode.LeftArrow) || thetaLR < -45)
		{
			rx = -1;
		}
		if (Input.GetKey(KeyCode.W) || thetaUP > 45)
		{
			ly = 1;
		}
		if (Input.GetKey(KeyCode.S) || thetaUP < -45)
		{
			ly = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			lx = 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			lx = -1;
		}
		Tello.controllerState.setAxis(lx, ly, rx, ry);

	}

	private void Tello_onUpdate(int cmdId)
	{
		//throw new System.NotImplementedException();
		Debug.Log("Tello_onUpdate : " + Tello.state);
	}

	private void Tello_onConnection(Tello.ConnectionState newState)
	{
		//throw new System.NotImplementedException();
		//Debug.Log("Tello_onConnection : " + newState);
		if (newState == Tello.ConnectionState.Connected) {
            Tello.queryAttAngle();
            Tello.setMaxHeight(50);

			Tello.setPicVidMode(1); // 0: picture, 1: video
			Tello.setVideoBitRate((int)VideoBitRate.VideoBitRateAuto);
			//Tello.setEV(0);
			Tello.requestIframe();
		}
	}

	private void Tello_onVideoData(byte[] data)
	{
		//Debug.Log("Tello_onVideoData: " + data.Length);
		if (telloVideoTexture != null)
			telloVideoTexture.PutVideoData(data);
	}

}
