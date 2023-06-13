using System.Collections.Generic;
using MelonLoader;
using Michsky.UI.ModernUIPack;
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

                    primary = GameObject.Find("PrimaryCamera");
                    primaryCamera = primary.GetComponent<Camera>();

                    world = GameObject.Find("PrimaryCamera/WorldCamDefault");
                    worldCam = world.GetComponent<Camera>();
                    worldCam.cullingMask = -1;
                    worldCam.stereoTargetEye = StereoTargetEyeMask.Both;
                    LoggerInstance.Msg($"Set world camera to VR");
                    var newParent = new GameObject("CameraParent").transform;
                    newParent.position = world.transform.position;
                    var cameraParent = world.transform.parent;
                    world.transform.SetParent(newParent);
                    newParent.SetParent(cameraParent);
                    newParent.rotation = new Quaternion(0, 0, 0, 0);

                    canvasObject = GameObject.Find("GeneralCanvas");
                    canvas = canvasObject.GetComponent<Canvas>();
                    canvas.worldCamera = worldCam;
                    //canvas3 = canvas.GetComponent<GraphicRaycaster>();
                    //Type camType = typeof(GraphicRaycaster);
                    //PropertyInfo canvasCam = camType.GetProperty("eventCamera");
                    //canvasCam.SetValue(canvas3, worldCam);
                    LoggerInstance.Msg($"Set UI to VR");

                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.scaleFactor = 1;
                    canvas.planeDistance = 0.2f;
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
                    fadeCanvas.worldCamera = worldCam;
                    //Type fadecamType = typeof(GraphicRaycaster);
                    //PropertyInfo fadecanvasCam = camType.GetProperty("eventCamera");
                    //fadeCanvasRaycaster = fadecanvas.GetComponent<GraphicRaycaster>();
                    //canvasCam.SetValue(fadeCanvasRaycaster, worldCam);
                    LoggerInstance.Msg($"Moved Fade");

                    headfollower = GameObject.Find("HeadTargetFollower");
                    headfollower.transform.SetParent(worldCam.transform);
                    headResetter = headfollower.GetComponent<PlayMakerFSM>();
                    headResetter.enabled = false;
                    headfollower.transform.localPosition = new Vector3(0, 0, 0);
                    headfollower.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    LoggerInstance.Msg($"Moved head follower");

                    buttonManager.PopulateButtons();
                    LoggerInstance.Msg($"Referenced buttons");

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

        public bool UIToggle = false;

        public int currentUIAdjust = 0;

        public override void OnUpdate()
        {
            // Used for button actions
            if (controllerManager != null)
            {
                if (_leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftAxis))
                {
                    if (buttonManager != null)
                    {
                        if (leftAxis.x < -0.5)
                        {
                            buttonManager.RunButtonNum(1);
                        }
                        else if (leftAxis.y > 0.5)
                        {
                            buttonManager.RunButtonNum(2);
                        }
                        else if (leftAxis.x > 0.5)
                        {
                            buttonManager.RunButtonNum(3);
                        }
                    }
                    controllerManager.prevLeftAxisValue = leftAxis;
                }

                if (_rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightAxis))
                {
                    if (buttonManager != null)
                    {
                        if (rightAxis.x < -0.5)
                        {
                            buttonManager.RunButtonNum(1);
                        }
                        else if (rightAxis.y > 0.5)
                        {
                            buttonManager.RunButtonNum(2);
                        }
                        else if (rightAxis.x > 0.5)
                        {
                            buttonManager.RunButtonNum(3);
                        }
                    }
                    controllerManager.prevRightAxisValue = rightAxis;
                }

                if (_leftController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool leftStickPressed))
                {
                    if (leftStickPressed && !controllerManager.prevLeftAxisClickValue && buttonManager != null)
                    {
                        buttonManager.RunButtonNum(4);
                    }
                    controllerManager.prevLeftAxisClickValue = leftStickPressed;
                }

                if (_rightController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool rightStickPressed))
                {
                    if (rightStickPressed && !controllerManager.prevRightAxisClickValue && buttonManager != null)
                    {
                        buttonManager.RunButtonNum(4);
                    }
                    controllerManager.prevRightAxisClickValue = rightStickPressed;
                }

                if (_leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimaryPressed))
                {
                    if (leftPrimaryPressed)
                    {
                        LoggerInstance.Msg(uiManager == null);
                    }

                    if (leftPrimaryPressed && !controllerManager.prevLeftPrimaryValue && uiManager != null)
                    {
                        uiManager.RunSafeword();
                    }
                    controllerManager.prevLeftPrimaryValue = leftPrimaryPressed;
                }

                if (_rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimaryPressed))
                {
                    if (rightPrimaryPressed && !controllerManager.prevRightPrimaryValue && uiManager != null)
                    {
                        uiManager.RunSafeword();
                    }
                    controllerManager.prevRightPrimaryValue = rightPrimaryPressed;
                }

                // Hide UI
                if (_leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool leftSecondaryPressed))
                {
                    if (leftSecondaryPressed && !controllerManager.prevLeftSecondaryValue && uiManager != null)
                    {
                        uiManager.HideUIElements();
                    }
                    controllerManager.prevLeftSecondaryValue = leftSecondaryPressed;
                }

                if (_rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightSecondaryPressed))
                {
                    if (rightSecondaryPressed && !controllerManager.prevRightSecondaryValue && uiManager != null)
                    {
                        uiManager.HideUIElements();
                    }
                    controllerManager.prevRightSecondaryValue = rightSecondaryPressed;
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
                        canvas.worldCamera = worldCam;
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
                    currentUIdepth += 0.1f;
                    UICanvas.planeDistance = currentUIdepth;
                }
            }
            else if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Delete))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIdepth -= 0.1f;
                    UICanvas.planeDistance = currentUIdepth;
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
        private void NnodResult()
        {
            foreach (PlayMakerFSM button in nodButtons)
            {
                if (button.gameObject.transform.parent.parent.parent.gameObject.activeSelf)
                {
                    button.SendEvent("Click");
                    return;
                }
            }
        }
        private void HeadshakeResult()
        {
            foreach (PlayMakerFSM button in headshakeButtons)
            {
                if (button.gameObject.transform.parent.parent.parent.gameObject.activeSelf)
                {
                    button.SendEvent("Click");
                    return;
                }
            }
        }

        private void MoveUIElements(int adjust)
        {
            GameObject UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/InstructionBorder");
            RectTransform UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 500 + adjust);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/StrokeCount");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 500 + adjust);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/BeatManager2");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 500 + adjust);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/TimedEvent");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 500 + adjust);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/Buttons");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(-405, 345 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(-405, 345 + adjust);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/ButtonLabels");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(-583, 253 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(-583, 253 + adjust);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/TradeOfferUI");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 1421, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 1421);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/SpinWheelUI");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 1200, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 1200);

            UIElementToMove = GameObject.Find("GeneralCanvas/EventManager/Urges");
            UIElementToMoveTransform = UIElementToMove.GetComponent<RectTransform>();
            UIElementToMoveTransform.anchoredPosition3D = new Vector3(0, 497 + adjust, 0);
            UIElementToMoveTransform.anchoredPosition = new Vector2(0, 497 + adjust);
        }

        void ResetCamera()
        {
            if (world != null)
            {
                world.transform.parent.position = world.transform.position;
                world.transform.parent.localPosition = -world.transform.localPosition;
                world.transform.parent.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }
}
