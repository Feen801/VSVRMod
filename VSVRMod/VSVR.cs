using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace VSVRMod
{
    public class VSVR : MelonMod
    {
        private GameObject primaryCameraGameObject;
        private Camera primaryCamera;

        private GameObject worldCameraGameObject;
        private Camera worldCamera;

        public GameObject canvasObject;
        public Canvas canvas;
        public RectTransform canvasRect;
        public GraphicRaycaster canvasRaycaster;

        public GameObject fadeCanvasObject;
        public Canvas fadeCanvas;
        public GraphicRaycaster fadeCanvasRaycaster;

        public GameObject overlayCanvasObject;
        public Canvas overlayCanvas;
        public GraphicRaycaster overlayCanvasRaycaster;

        private GameObject headfollower;
        private PlayMakerFSM headResetter;

        private VRGestureRecognizer vrGestureRecognizer;

        ButtonManager buttonManager;
        UIManager uiManager;
        ControllerManager controllerManager;

        private InputDevice _leftController;
        private InputDevice _rightController;

        public override void OnLateInitializeMelon()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

            if (devices.Count > 0)
            {
                foreach(var device in devices)
                {
                    LoggerInstance.Msg(device.name);
                    if (device.name.Contains("Left"))
                    {
                        _leftController = device;
                    }
                    else if (device.name.Contains("Right"))
                    {
                        _rightController = device;
                    }
                }
                //LoggerInstance.Msg(devices[0]);
                //_targetDevice = devices[0];
            }
            //XRSettings.LoadDeviceByName("OpenVR");
            //XRGeneralSettings.Instance.Manager.InitializeLoader();
            //XRGeneralSettings.Instance.Manager.StartSubsystems();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been initalized!");
            switch (buildIndex)
            {
                case 2:
                    //SubsystemManager.GetInstances(subsystems);
                    //foreach(XRInputSubsystem x in subsystems)
                    //{
                    //LoggerInstance.Msg($"XRInputSubsystem: {x.GetType()}");
                    //x.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
                    //x.TryRecenter();
                    //}

                    //XRInputSubsystem xrInputSubsystem = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();
                    // xrInputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);

                    // Create new classes since a new session instance is starting
                    buttonManager = new ButtonManager();
                    controllerManager = new ControllerManager();
                    uiManager = new UIManager();

                    vrGestureRecognizer = new VRGestureRecognizer();
                    vrGestureRecognizer.Nodded += OnNod;
                    vrGestureRecognizer.HeadShaken += OnHeadshake;
                    LoggerInstance.Msg($"Setup gestures");

                    primaryCameraGameObject = GameObject.Find("PrimaryCamera");
                    primaryCamera = primaryCameraGameObject.GetComponent<Camera>();

                    worldCameraGameObject = GameObject.Find("PrimaryCamera/WorldCamDefault");
                    worldCamera = worldCameraGameObject.GetComponent<Camera>();
                    worldCamera.cullingMask = -1;
                    worldCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                    LoggerInstance.Msg($"Set world camera to VR");
                    var newParent = new GameObject("CameraParent").transform;
                    newParent.position = worldCameraGameObject.transform.position;
                    var cameraParent = worldCameraGameObject.transform.parent;
                    worldCameraGameObject.transform.SetParent(newParent);
                    newParent.SetParent(cameraParent);
                    newParent.rotation = new Quaternion(0, 0, 0, 0);

                    canvasObject = GameObject.Find("GeneralCanvas");
                    canvas = canvasObject.GetComponent<Canvas>();
                    canvas.worldCamera = worldCamera;
                    //canvas3 = canvas.GetComponent<GraphicRaycaster>();
                    //Type camType = typeof(GraphicRaycaster);
                    //PropertyInfo canvasCam = camType.GetProperty("eventCamera");
                    //canvasCam.SetValue(canvas3, worldCam);
                    LoggerInstance.Msg($"Set UI to VR");

                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.scaleFactor = 1;
                    canvas.planeDistance = currentUIdepth;
                    canvasRect = canvasObject.GetComponent<RectTransform>();
                    //canvasrect.anchoredPosition3D = new Vector3(4.7f, 7.5f, 29f);
                    //canvasrect.localEulerAngles = new Vector3(0f, 210f, 0f);
                    LoggerInstance.Msg($"Moved UI");

                    uiManager.GetUIElements();
                    LoggerInstance.Msg($"Moved UI Elements");

                    fadeCanvasObject = UnityEngine.GameObject.Find("FadeCanvas");
                    fadeCanvas = fadeCanvasObject.GetComponent<Canvas>();
                    fadeCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    fadeCanvas.planeDistance = 0.1f;
                    fadeCanvas.worldCamera = worldCamera;
                    //Type fadecamType = typeof(GraphicRaycaster);
                    //PropertyInfo fadecanvasCam = camType.GetProperty("eventCamera");
                    //fadeCanvasRaycaster = fadecanvas.GetComponent<GraphicRaycaster>();
                    //canvasCam.SetValue(fadeCanvasRaycaster, worldCam);
                    LoggerInstance.Msg($"Moved Fade");

                    overlayCanvasObject = UnityEngine.GameObject.Find("OverlayCanvas");
                    overlayCanvas = overlayCanvasObject.GetComponent<Canvas>();
                    overlayCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    overlayCanvas.planeDistance = currentUIdepth;
                    overlayCanvas.worldCamera = worldCamera;
                    //Type fadecamType = typeof(GraphicRaycaster);
                    //PropertyInfo fadecanvasCam = camType.GetProperty("eventCamera");
                    //fadeCanvasRaycaster = fadecanvas.GetComponent<GraphicRaycaster>();
                    //canvasCam.SetValue(fadeCanvasRaycaster, worldCam);
                    LoggerInstance.Msg($"Moved Overlay");

                    headfollower = GameObject.Find("HeadTargetFollower");
                    headfollower.transform.SetParent(worldCamera.transform);
                    headResetter = headfollower.GetComponent<PlayMakerFSM>();
                    headResetter.enabled = false;
                    headfollower.transform.localPosition = new Vector3(0, 0, 0);
                    headfollower.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    LoggerInstance.Msg($"Moved head follower");

                    buttonManager.PopulateButtons();
                    LoggerInstance.Msg($"Referenced buttons");

                    //WIP
                    Slider depthSlider = canvasObject.AddComponent<Slider>();
                    depthSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
                    depthSlider.maxValue = 100;
                    depthSlider.minValue = 0;

                    break;
                default:
                    break;
            }
        }

        public override void OnLateUpdate()
        {
            if (_leftController.TryGetFeatureValue(CommonUsages.grip, out float leftGrip) && controllerManager != null)
            {
                if (leftGrip == 1 && controllerManager.prevLeftGripValue != 1)
                {
                    ResetCamera();
                }
                controllerManager.prevLeftGripValue = leftGrip;
            }

            if (_rightController.TryGetFeatureValue(CommonUsages.grip, out float rightGrip) && controllerManager != null)
            {
                if (rightGrip == 1 && controllerManager.prevRightGripValue != 1)
                {
                    ResetCamera();
                }
                controllerManager.prevRightGripValue = rightGrip;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                ResetCamera();
            }
        }

        public bool waitKeyPress = true;

        public bool UIToggle = true;

        public int currentUIAdjust = 0;

        public float currentUIdepth = 0.2f;

        public override void OnUpdate()
        {
            // Used for button actions
            if (controllerManager != null)
            {
                Vector2 leftAxis;
                Vector2 rightAxis;
                Vector2 axis;
                

                if (buttonManager != null)
                {
                    _leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftAxis);
                    _rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightAxis);
                    if (leftAxis.magnitude > rightAxis.magnitude)
                    {
                        axis = leftAxis;
                    }
                    else
                    {
                        axis = rightAxis;
                    }
                    double radians = Math.Atan2(axis.x, axis.y);
                    double magnitude = axis.magnitude;
                    double angle = radians * (180 / Math.PI);

                    //LoggerInstance.Msg($"Controller input: {magnitude}, {angle}");

                    if (_rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTriggerPressed))
                    {
                        buttonManager.radialMenuInteract(!controllerManager.prevRightTriggerValue && magnitude > 0.7 && rightTriggerPressed, angle);
                        controllerManager.prevRightTriggerValue = rightTriggerPressed;
                    }
                    if (_leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTriggerPressed))
                    {
                        buttonManager.radialMenuInteract(!controllerManager.prevLeftTriggerValue && magnitude > 0.7 && leftTriggerPressed, angle);
                        controllerManager.prevLeftTriggerValue = leftTriggerPressed;
                    }
                    controllerManager.prevLeftAxisValue = leftAxis;
                    controllerManager.prevRightAxisValue = rightAxis;
                }
                if (_leftController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool leftStickPressed))
                {
                    if (leftStickPressed && !controllerManager.prevLeftAxisClickValue && buttonManager != null)
                    {
                        buttonManager.radialMenuExpand();
                    }
                    controllerManager.prevLeftAxisClickValue = leftStickPressed;
                }

                if (_rightController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool rightStickPressed))
                {
                    if (rightStickPressed && !controllerManager.prevRightAxisClickValue && buttonManager != null)
                    {
                        buttonManager.radialMenuExpand();
                    }
                    controllerManager.prevRightAxisClickValue = rightStickPressed;
                }

                
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    if (UIToggle)
                    {
                        UIToggle = false;
                        canvas.worldCamera = primaryCamera;
                    }
                    else
                    {
                        UIToggle = true;
                        canvas.worldCamera = worldCamera;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) && uiManager != null)
            {
                if (waitKeyPress)
                {
                    uiManager.RunSafeword();
                }
            }
            else if ((Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.Y)) && uiManager != null)
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIAdjust += 50;
                    uiManager.MoveUIElements(currentUIAdjust);
                }  
            }
            else if ((Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.H)) && uiManager != null)
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIAdjust -= 50;
                    uiManager.MoveUIElements(currentUIAdjust);
                }
            }
            else if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Insert))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIdepth += 0.05f;
                    canvas.planeDistance = currentUIdepth;
                    overlayCanvas.planeDistance = currentUIdepth;
                }
            }
            else if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Delete))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIdepth -= 0.05f;
                    canvas.planeDistance = currentUIdepth;
                    overlayCanvas.planeDistance = currentUIdepth;
                }
            }
            else
            {
                waitKeyPress = true;
            }

            if (vrGestureRecognizer != null)
            {
                vrGestureRecognizer.Update();
            }
        }

        int recentNods = 0;
        long nodLast = MathHelper.CurrentTimeMillis();
        int recentHeadshakes = 0;
        long headshakeLast = MathHelper.CurrentTimeMillis();

        void OnNod()
        {
            recentHeadshakes = 0;
            if (MathHelper.CurrentTimeMillis() - nodLast > 1000)
            {
                recentNods = 0;
            }
            nodLast = MathHelper.CurrentTimeMillis();
            recentNods++;
            LoggerInstance.Msg($"Partial nod detected, total: {recentNods}");
            if (recentNods > 1)
            {
                LoggerInstance.Msg("Nod detected");
                buttonManager.RunButtonMovement(VSHeadMovement.Nod);
                recentNods = 0;
            }
        }

        void OnHeadshake()
        {
            recentNods = 0;
            if (MathHelper.CurrentTimeMillis() - headshakeLast > 1000)
            {
                recentHeadshakes = 0;
            }
            headshakeLast = MathHelper.CurrentTimeMillis();
            recentHeadshakes++;
            LoggerInstance.Msg($"Partial headshake detected, total: {recentHeadshakes}");
            if (recentHeadshakes > 1) { 
                LoggerInstance.Msg("Headshake detected");
                buttonManager.RunButtonMovement(VSHeadMovement.Headshake);
                recentHeadshakes = 0;
            }
        }

        void ResetCamera()
        {
            if (worldCameraGameObject != null)
            {
                worldCameraGameObject.transform.parent.position = worldCameraGameObject.transform.position;
                worldCameraGameObject.transform.parent.localPosition = -worldCameraGameObject.transform.localPosition;
                worldCameraGameObject.transform.parent.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }
}
