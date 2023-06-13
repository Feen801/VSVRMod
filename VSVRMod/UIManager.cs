using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;

namespace VSVRMod
{
    class VSUIElement
    {
        public string objectName;
        public GameObject uiObject;

        public Canvas uiCanvas;
        public RectTransform uiRectTransform;
        public Vector2 anchorPosition;
        public Vector3 anchorPosition3D;
    }

    public class UIManager
    {
        List<VSUIElement> UIElements = new List<VSUIElement>()
        {
            new VSUIElement() { objectName = "InstructionBorder" },
            new VSUIElement() { objectName = "StrokeCount" },
            new VSUIElement() { objectName = "BeatManager2" },
            new VSUIElement() { objectName = "TimedEvent" },
            new VSUIElement() { objectName = "Buttons" },
            new VSUIElement() { objectName = "ButtonLabels" },
            new VSUIElement() { objectName = "TradeOfferUI" },
            new VSUIElement() { objectName = "SpinWheelUI" },
            new VSUIElement() { objectName = "Urges" }
        };

        public void GetUIElements()
        {
            Melon<VSVR>.Logger.Msg("Adding UI elements");
            // Grab canvas objects
            // Possibility: Maybe find parents of the adjusted UI elements and see if they have a Canvas?
            GameObject generalCanvasObject = GameObject.Find("GeneralCanvas");
            Canvas generalCanvas = generalCanvasObject.GetComponent<Canvas>();
            VSUIElement generalCanvasElement = new VSUIElement()
            {
                objectName = generalCanvasObject.name,
                uiObject = generalCanvasObject,
                uiCanvas = generalCanvas,
            };
            UIElements.Add(generalCanvasElement);

            GameObject overlayCanvasObject = GameObject.Find("OverlayCanvas");
            Canvas overlayCanvas = overlayCanvasObject.GetComponent<Canvas>();
            VSUIElement overlayCanvasElement = new VSUIElement()
            {
                objectName = overlayCanvasObject.name,
                uiObject = overlayCanvasObject,
                uiCanvas = overlayCanvas,
            };
            UIElements.Add(overlayCanvasElement);

            // Grab individual UI objects
            foreach (VSUIElement element in UIElements)
            {
                if (element.uiCanvas == null)
                {
                    GameObject uiObject = GameObject.Find($"GeneralCanvas/EventManager/{element.objectName}");
                    RectTransform uiRectTransform = uiObject.GetComponent<RectTransform>();

                    if (uiObject != null && uiRectTransform != null)
                    {
                        element.uiObject = uiObject;
                        element.uiRectTransform = uiRectTransform;
                        DelegatePositionAdjust(element);
                    }
                }
            }
        }

        void DelegatePositionAdjust(VSUIElement element, int adjust = 0)
        {
            switch (element.objectName)
            {
                case "Buttons":
                    SetUIPosition(element, new Vector2(-405, 345 + adjust), new Vector3(-405, 345 + adjust, 0));
                    break;
                case "ButtonLabels":
                    SetUIPosition(element, new Vector2(-583, 253 + adjust), new Vector3(-583, 253 + adjust, 0));
                    break;
                case "TradeOfferUI":
                    SetUIPosition(element, new Vector2(0, 1421), new Vector3(0, 1421, 0));
                    break;
                case "SpinWheelUI":
                    SetUIPosition(element, new Vector2(0, 1200), new Vector3(0, 1200, 0));
                    break;
                case "Urges":
                    SetUIPosition(element, new Vector2(0, 497 + adjust), new Vector3(0, 497 + adjust, 0));
                    break;
                default:
                    SetUIPosition(element, new Vector2(0, 500 + adjust), new Vector3(0, 500 + adjust, 0));
                    break;
            }
        }

        void SetUIPosition(VSUIElement element, Vector2 anchorPosition, Vector3 anchorPosition3D)
        {
            element.uiRectTransform.anchoredPosition = anchorPosition;
            element.uiRectTransform.anchoredPosition3D = anchorPosition3D;

            element.anchorPosition = anchorPosition;
            element.anchorPosition3D = anchorPosition3D;
        }

        public void MoveUIElements(int adjust)
        {
            foreach (VSUIElement element in UIElements.FindAll(p => p.uiCanvas == null))
            {
                DelegatePositionAdjust(element, adjust);
            }
        }

        // Only hides instructions and buttons
        public void HideUIElements()
        {
            Melon<VSVR>.Logger.Msg("Triggered UI hide");
            foreach (VSUIElement element in UIElements.FindAll(p => p.uiCanvas != null))
            {
                element.uiCanvas.enabled = !element.uiCanvas.enabled;
            }
        }

        public void RunSafeword()
        {
            Melon<VSVR>.Logger.Msg("Triggered safeword");
            GameObject safeWordButton = GameObject.Find("OverlayCanvas/GameMenu/Image/Safeword/Button");
            Button safeWordUIButton = safeWordButton.GetComponent<Button>();
            safeWordUIButton.onClick.Invoke();
        }
    }
}
