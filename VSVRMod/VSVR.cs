using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Windows.Forms;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace VSVRMod
{
    public class VSVR : MelonMod
    {
        public GameObject primary;
        public Camera primaryCamera;

        public GameObject world;
        public Camera worldCam;

        public GameObject canvas;
        public Canvas canvas2;
        public RectTransform canvasrect;
        public GraphicRaycaster canvas3;

        public GameObject fadecanvas;
        public Canvas fadecanvas2;
        public GraphicRaycaster fadecanvas3;

        public GameObject headfollower;
        public PlayMakerFSM headResetter;

        VRGestureRecognizer vrGestureRecognizer;

        PlayMakerFSM OkButton;
        PlayMakerFSM Ok2Button;
        PlayMakerFSM Done1Button;
        PlayMakerFSM Done2Button;
        PlayMakerFSM YesButton;
        PlayMakerFSM ReadyButton;
        PlayMakerFSM ThankYouButton;
        PlayMakerFSM ThankYou2Button;
        PlayMakerFSM ConfirmButton;
        PlayMakerFSM GoodButton;
        PlayMakerFSM AcceptButton;

        PlayMakerFSM NoButton;
        PlayMakerFSM FailedButton;
        PlayMakerFSM Failed2Button;
        PlayMakerFSM Failed3Button;
        PlayMakerFSM Failed4Button;
        PlayMakerFSM DisobeyButton;
        PlayMakerFSM Disobey2Button;
        PlayMakerFSM Disobey3Button;
        PlayMakerFSM DisobeyedButton;
        PlayMakerFSM Disobeyed2Button;
        PlayMakerFSM BadButton;
        PlayMakerFSM DeclineButton;
        PlayMakerFSM RefuseButton;

        List<PlayMakerFSM> nodButtons = new List<PlayMakerFSM>();
        List<PlayMakerFSM> headshakeButtons = new List<PlayMakerFSM>();

        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();

        private void findButtons()
        {
            LoggerInstance.Msg($"-Ok Button");
            OkButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Ok/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(OkButton);

            LoggerInstance.Msg($"-Ok2 Button");
            Ok2Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Ok2/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(Ok2Button);

            LoggerInstance.Msg($"-Done1 Button");
            Done1Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Done1/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(Done1Button);

            LoggerInstance.Msg($"-Done2 Button");
            Done2Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Done2/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(Done2Button);

            LoggerInstance.Msg($"-Yes Button");
            YesButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Yes/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(YesButton);

            LoggerInstance.Msg($"-Ready Button");
            ReadyButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Ready/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(ReadyButton);

            LoggerInstance.Msg($"-Thank You Button");
            ThankYouButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Thank You/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(ThankYouButton);

            LoggerInstance.Msg($"-Thank You 2 Button");
            ThankYou2Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Thank You2/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(ThankYou2Button);

            LoggerInstance.Msg($"-Confirm Button");
            ConfirmButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Confirm/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(ConfirmButton);

            LoggerInstance.Msg($"-Good Button");
            GoodButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Good/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(GoodButton);

            LoggerInstance.Msg($"-Accept Button");
            AcceptButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Accept/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            nodButtons.Add(AcceptButton);





            LoggerInstance.Msg($"-No Button");
            NoButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/No/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(NoButton);

            LoggerInstance.Msg($"-Failed Button");
            FailedButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Failed/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(FailedButton);

            LoggerInstance.Msg($"-Failed2 Button");
            Failed2Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Failed2/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(Failed2Button);

            LoggerInstance.Msg($"-Failed3 Button");
            Failed3Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Failed3/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(Failed3Button);

            LoggerInstance.Msg($"-Failed4 Button");
            Failed4Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Failed4/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(Failed4Button);

            LoggerInstance.Msg($"-Disobey Button");
            DisobeyButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Disobey/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(DisobeyButton);

            LoggerInstance.Msg($"-Disobey2 Button");
            Disobey2Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Disobey2/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(Disobey2Button);

            LoggerInstance.Msg($"-Disobey3 Button");
            Disobey3Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Disobey3/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(Disobey3Button);

            LoggerInstance.Msg($"-Disobeyed Button");
            DisobeyedButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Disobeyed/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(DisobeyedButton);

            LoggerInstance.Msg($"-Disobeyed2 Button");
            Disobeyed2Button = GameObject.Find("GeneralCanvas/EventManager/Buttons/Disobeyed2/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(Disobeyed2Button);

            LoggerInstance.Msg($"-Bad Button");
            BadButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Bad/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(BadButton);

            LoggerInstance.Msg($"-Decline Button");
            DeclineButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Decline/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(DeclineButton);

            LoggerInstance.Msg($"-Refuse Button");
            RefuseButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/Refuse/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
            headshakeButtons.Add(RefuseButton);
        }

        private void nodResult()
        {
            foreach (PlayMakerFSM button in nodButtons)
            {
                if(button.gameObject.transform.parent.parent.parent.gameObject.activeSelf)
                {
                    button.SendEvent("Click");
                    return;
                }
            }
        }
        private void headshakeResult()
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

        public override void OnLateInitializeMelon()
        {
            //XRSettings.LoadDeviceByName("OpenVR");
            //XRGeneralSettings.Instance.Manager.InitializeLoader();
            //XRGeneralSettings.Instance.Manager.StartSubsystems();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
            if (buildIndex == 2) {
                //SubsystemManager.GetInstances(subsystems);
                //foreach(XRInputSubsystem x in subsystems)
                //{
                    //LoggerInstance.Msg($"XRInputSubsystem: {x.GetType()}");
                    //x.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
                    //x.TryRecenter();
                //}

                //XRInputSubsystem xrInputSubsystem = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();
                // xrInputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);

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

                canvas = GameObject.Find("GeneralCanvas");
                canvas2 = canvas.GetComponent<Canvas>();
                canvas2.worldCamera = worldCam;
                //canvas3 = canvas.GetComponent<GraphicRaycaster>();
                //Type camType = typeof(GraphicRaycaster);
                //PropertyInfo canvasCam = camType.GetProperty("eventCamera");
                //canvasCam.SetValue(canvas3, worldCam);
                LoggerInstance.Msg($"Set UI to VR");

                canvas2.renderMode = RenderMode.ScreenSpaceCamera;
                canvas2.scaleFactor = 1;
                canvas2.planeDistance = 0.2f;
                canvasrect = canvas.GetComponent<RectTransform>();
                //canvasrect.anchoredPosition3D = new Vector3(4.7f, 7.5f, 29f);
                //canvasrect.localEulerAngles = new Vector3(0f, 210f, 0f);
                LoggerInstance.Msg($"Moved UI");

                MoveUIElements(0);
                LoggerInstance.Msg($"Moved UI Elements");

                fadecanvas = UnityEngine.GameObject.Find("FadeCanvas");
                fadecanvas2 = fadecanvas.GetComponent<Canvas>();
                fadecanvas2.renderMode = RenderMode.ScreenSpaceCamera;
                fadecanvas2.planeDistance = 0.1f;
                fadecanvas2.worldCamera = worldCam;
                //Type fadecamType = typeof(GraphicRaycaster);
                //PropertyInfo fadecanvasCam = camType.GetProperty("eventCamera");
                //fadecanvas3 = fadecanvas.GetComponent<GraphicRaycaster>();
                //canvasCam.SetValue(fadecanvas3, worldCam);
                LoggerInstance.Msg($"Moved Fade");

                headfollower = GameObject.Find("HeadTargetFollower");
                headfollower.transform.SetParent(worldCam.transform);
                headResetter = headfollower.GetComponent<PlayMakerFSM>();
                headResetter.enabled = false;
                headfollower.transform.localPosition = new Vector3(0, 0, 0);
                headfollower.transform.localRotation = new Quaternion(0, 0, 0, 0);
                LoggerInstance.Msg($"Moved head follower");

                findButtons();
                LoggerInstance.Msg($"Referenced buttons");
            }
        }

        private void MoveUIElements(int adjust)
        {
            GameObject instructionBorder = GameObject.Find("GeneralCanvas/EventManager/InstructionBorder");
            RectTransform instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(0, 500 + adjust);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/StrokeCount");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(0, 500 + adjust);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/BeatManager2");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(0, 500 + adjust);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/TimedEvent");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 500 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(0, 500 + adjust);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/Buttons");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(-405, 345 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(-405, 345 + adjust);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/ButtonLabels");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(-583, 253 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(-583, 253 + adjust);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/TradeOfferUI");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 1421, 0);
            instructionRect.anchoredPosition = new Vector2(0, 1421);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/SpinWheelUI");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 1200, 0);
            instructionRect.anchoredPosition = new Vector2(0, 1200);

            instructionBorder = GameObject.Find("GeneralCanvas/EventManager/Urges");
            instructionRect = instructionBorder.GetComponent<RectTransform>();
            instructionRect.anchoredPosition3D = new Vector3(0, 497 + adjust, 0);
            instructionRect.anchoredPosition = new Vector2(0, 497 + adjust);
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                if(world != null)
                {
                    world.transform.parent.position = world.transform.position;
                    world.transform.parent.localPosition = -world.transform.localPosition;
                    world.transform.parent.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }

        public bool waitKeyPress = true;

        public bool UIToggle = false;

        public int currentUIAdjust = 0;

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    if (UIToggle)
                    {
                        UIToggle = false;
                        canvas2.worldCamera = primaryCamera;
                    }
                    else
                    {
                        UIToggle = true;
                        canvas2.worldCamera = worldCam;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.Y))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIAdjust += 50;
                    MoveUIElements(currentUIAdjust);
                }  
            }
            else if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.H))
            {
                if (waitKeyPress)
                {
                    waitKeyPress = false;
                    currentUIAdjust -= 50;
                    MoveUIElements(currentUIAdjust);
                }
            }
            else
            {
                waitKeyPress = true;
            }
            if(vrGestureRecognizer != null)
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
            LoggerInstance.Msg($"Partial nod detected, total: " + recentNods);
            if (recentNods > 1)
            {
                LoggerInstance.Msg($"Nod detected");
                nodResult();
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
            LoggerInstance.Msg($"Partial headshake detected, total: " + recentHeadshakes);
            if (recentHeadshakes > 1) { 
                LoggerInstance.Msg($"Headshake detected");
                headshakeResult();
                recentHeadshakes = 0;
            }
        }
        
    }
}
