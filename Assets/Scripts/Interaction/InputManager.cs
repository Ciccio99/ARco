using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager> {

	#region DELEGATE FUNCTIONS
	// Event delegate for touch begin phase
	public delegate void ARTouchBeganUpdate (Touch touchEvent);
	public event ARTouchBeganUpdate ARTouchBeganUpdateEvent;

	// Stationary touch event
	public delegate void ARTouchStationaryUpdate (Touch touchEvent);
	public event ARTouchBeganUpdate ARTouchStationaryUpdateEvent;

	// Event delegate for the move touch phase
    public delegate void ARTouchMovedUpdate(Touch touchEvent);
    public event ARTouchMovedUpdate ARTouchMovedUpdateEvent;

    // Event delegate for the touch ended phase
    public delegate void ARTouchEndedUpdate(Touch touchEvent);
    public event ARTouchEndedUpdate ARTouchEndedUpdateEvent;

    // Event delegate for the canceled touch phase
    public delegate void ARTouchCanceledUpdate(Touch touchEvent);
    public event ARTouchCanceledUpdate ARTouchCanceledUpdateEvent;
    #endregion

	void Update () {
		#if UNITY_EDITOR
			//CheckMouse ();
		#else
			CheckTouch ();
		#endif
	}

	private void CheckTouch () {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {
				case TouchPhase.Began:
					if (ARTouchBeganUpdateEvent != null)
						ARTouchBeganUpdateEvent (touch);
					break;
				case TouchPhase.Stationary:
					if (ARTouchStationaryUpdateEvent != null)
						ARTouchStationaryUpdateEvent (touch);
					break;
				case TouchPhase.Moved:
					if (ARTouchMovedUpdateEvent != null) 
						ARTouchMovedUpdateEvent (touch);
					break;
				case TouchPhase.Ended:
					if (ARTouchEndedUpdateEvent != null)
						ARTouchMovedUpdateEvent (touch);
					break;
				case TouchPhase.Canceled:
					if (ARTouchCanceledUpdateEvent != null)
						ARTouchCanceledUpdateEvent (touch);
					break;
				default: break;
			}
		}
	}

	public void Ping () {
		// Function to create instance of this singleton
	}



}
