using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace VSVRMod
{
    public class VSVR : MelonMod
    {
        private GameObject primaryCameraGameObject;
        private Camera primaryCamera;

        private GameObject worldCameraGameObject;
        private Camera worldCamera;

        private GameObject UICanvasGameObject;
        private Canvas UICanvas;

        private GameObject fadeCanvasGameObject;
        private Canvas fadeCanvas;

        private GameObject headfollower;
        private PlayMakerFSM headResetter;

        private VRGestureRecognizer vrGestureRecognizer;

        private readonly List<String> nodButtonNames = new List<string> { "Ok", "Ok2", "Done1", "Done2", "Yes", "Ready", "Thank You", "Thank You2", "Confirm", "Good", "Accept" };
        private readonly List<String> headshakButtonNames = new List<string> { "No", "Failed", "Failed2", "Failed3", "Failed4", "Disobey", "Disobey2", "Disobey3", "Disobeyed", "Disobeyed2", "Bad", "Decline", "Refuse" };

        private readonly List<PlayMakerFSM> nodButtons = new List<PlayMakerFSM>();
        private readonly List<PlayMakerFSM> headshakeButtons = new List<PlayMakerFSM>();

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
            if (buildIndex == 2) {
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

                UICanvasGameObject = GameObject.Find("GeneralCanvas");
                UICanvas = UICanvasGameObject.GetComponent<Canvas>();
                UICanvas.worldCamera = worldCamera;
                UICanvas.renderMode = RenderMode.ScreenSpaceCamera;
                UICanvas.scaleFactor = 1;
                UICanvas.planeDistance = currentUIdepth;
                LoggerInstance.Msg($"Set UI to VR");

                MoveUIElements(0);
                LoggerInstance.Msg($"Moved UI Elements");

                fadeCanvasGameObject = UnityEngine.GameObject.Find("FadeCanvas");
                fadeCanvas = fadeCanvasGameObject.GetComponent<Canvas>();
                fadeCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                fadeCanvas.planeDistance = 0.1f;
                fadeCanvas.worldCamera = worldCamera;
                LoggerInstance.Msg($"Moved Fade to VR");

                headfollower = GameObject.Find("HeadTargetFollower");
                headfollower.transform.SetParent(worldCamera.transform);
                headResetter = headfollower.GetComponent<PlayMakerFSM>();
                headResetter.enabled = false;
                headfollower.transform.localPosition = new Vector3(0, 0, 0);
                headfollower.transform.localRotation = new Quaternion(0, 0, 0, 0);
                LoggerInstance.Msg($"Moved head follower");

                findButtons();
                LoggerInstance.Msg($"Referenced buttons for guestures");
            }
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                if(worldCameraGameObject != null)
                {
                    worldCameraGameObject.transform.parent.position = worldCameraGameObject.transform.position;
                    worldCameraGameObject.transform.parent.localPosition = -worldCameraGameObject.transform.localPosition;
                    worldCameraGameObject.transform.parent.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }

        public bool waitKeyPress = true;

        public bool UIToggle = false;

        public int currentUIAdjust = 0;
        public float currentUIdepth = 0.4f;

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
                        UICanvas.worldCamera = primaryCamera;
                    }
                    else
                    {
                        UIToggle = true;
                        UICanvas.worldCamera = worldCamera;
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
            LoggerInstance.Msg($"Partial nod detected, total: " + recentNods);
            if (recentNods > 1)
            {
                LoggerInstance.Msg($"Nod detected");
                NnodResult();
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
                HeadshakeResult();
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

        private void findButtons()
        {
            PlayMakerFSM currentButton;
            foreach (String buttonName in nodButtonNames)
            {
                LoggerInstance.Msg($"--Finding nod button: {buttonName}");
                currentButton = GameObject.Find($"GeneralCanvas/EventManager/Buttons/{buttonName}/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
                nodButtons.Add(currentButton);
            }
            foreach (String buttonName in headshakButtonNames)
            {
                LoggerInstance.Msg($"--Finding headshake button: {buttonName}");
                currentButton = GameObject.Find($"GeneralCanvas/EventManager/Buttons/{buttonName}/DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>();
                headshakeButtons.Add(currentButton);
            }
        }
    }
}
