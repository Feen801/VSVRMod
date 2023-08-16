using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;

namespace VSVRMod
{
    public enum VSHeadMovement
    {
        Nod,
        Headshake
    }

    public class VSButton
    {
        public struct VSButtonObject
        {
            public PlayMakerFSM buttonFsm;
            public GameObject buttonObject;
        }

        // Normal - A normal button on the bottom bar
        // Choice - Stakes and Choice UI buttons
        public enum VSButtonType
        {
            Normal,
            Stakes,
            Choice,
        }

        public string name;
        public List<VSButtonObject> buttonObjects = new List<VSButtonObject>();
        public VSHeadMovement headMovement;
        public int buttonNum;
        public VSButtonType buttonType;
    }

    public class ButtonManager
    {
        GameObject stakesUIObject;
        GameObject choiceUIObject;      

        // Mappings for all VS buttons
        List<VSButton> Buttons = new List<VSButton>()
        {
            //Positives/left side
            new VSButton() { name = "Thank You3", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Accept", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Obey", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ready", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Thank You2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ok2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ready", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Continue", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Done1", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Done2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Yes", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ok", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Thank You", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Confirm", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Sorry", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Pay2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Pay", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Sorry2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Bye", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Spin", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Good", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Uh Oh", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ready2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Cumming", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "RhythmEdge", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "EdgeEnding", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "EdgeLeft", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "EdgeCentered", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            //new VSButton() { name = "PoTMercy", buttonNum = 1, headMovement = VSHeadMovement.Nod }, ???
            new VSButton() { name = "MercyNoFavor", buttonNum = 1, headMovement = VSHeadMovement.Nod },

            //Negatives/right side
            new VSButton() { name = "Disobey", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobey2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobeyed2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobeyed", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobey3", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Decline", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Bad", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "No", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed3", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Refuse", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed4", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Decline", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "ConfirmNo", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Beg", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Beg2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Beg2NoFavor", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
        };

        GameObject radialMenuButton1;
        GameObject radialMenuButton2;

        GameObject level1Ring;
        GameObject level2Ring;

        GameObject edgeRadialButton;
        GameObject tauntRadialButton;
        GameObject mercyRadialButton;

        GameObject edgeRadialButtonReact;
        GameObject tauntRadialButtonReact;
        GameObject mercyRadialButtonReact;

        GameObject timeOutRadialButton;
        GameObject hideUIRadialButton;
        GameObject safeWordRadialButton;
        GameObject OopsWordRadialButton;

        GameObject timeOutRadialButtonReact;
        GameObject hideUIRadialButtonReact;
        GameObject safeWordRadialButtonReact;
        GameObject oopsWordRadialButtonReact;

        GameObject exitButtonRadial;

        GameObject arousalPlusButton;
        GameObject arousalMinusButton;
        public void PopulateButtons()
        {
            GameObject buttonParent = GameObject.Find("GeneralCanvas/EventManager/Buttons/Positives ------------");
            foreach (Transform child in buttonParent.transform)
            {
                UpdateVSButton(child, "DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            }

            buttonParent = GameObject.Find("GeneralCanvas/EventManager/Buttons/Negatives ------------");
            foreach (Transform child in buttonParent.transform)
            {
                UpdateVSButton(child, "DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            }

            GameObject stakesObject = GameObject.Find("GeneralCanvas/EventManager/StakesUI");
            stakesUIObject = stakesObject;
            foreach (Transform child in stakesObject.transform)
            {
                UpdateVSButton(child, "Collider", VSButton.VSButtonType.Stakes);
            }

            GameObject choiceObject = GameObject.Find("GeneralCanvas/EventManager/ChoiceUI");
            choiceUIObject = choiceObject;
            foreach (Transform child in choiceObject.transform)
            {
                UpdateVSButton(child, "Collider", VSButton.VSButtonType.Choice);
            }

            radialMenuButton1 = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Circle");
            radialMenuButton2 = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/Grow");

            level1Ring = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1");
            level2Ring = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2");

            edgeRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/EdgeBG");
            tauntRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/TauntBG");
            mercyRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/MercyBG");

            edgeRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/EdgeBG/Collider/ButtonReact");
            tauntRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/TauntBG/Collider/ButtonReact");
            mercyRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/OtherButtons/MercyBG/Collider/ButtonReact");

            timeOutRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject/Time Out");
            hideUIRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject/Hide UI"); ;
            safeWordRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject (1)/Safe Word"); ;
            OopsWordRadialButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject (1)/Oops");

            timeOutRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject/Time Out/Collider (1)/ButtonReact");
            hideUIRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject/Hide UI/Collider/ButtonReact");
            safeWordRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject (1)/Safe Word/Collider/ButtonReact");
            oopsWordRadialButtonReact = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level2/GameObject (1)/Oops/Collider/ButtonReact");

            exitButtonRadial = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/Exit");

            arousalPlusButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/ArousalMeter/Overlays/Plus");
            arousalMinusButton = GameObject.Find("GeneralCanvas/EventManager/NewButtons/Center/Level1/ArousalMeter/Overlays/Minus");
        }

        public void radialMenuInteract(bool isClick, double angle)
        {
            int level = 0;
            if(level1Ring.activeSelf)
            {
                level = 1;
            }
            if (level2Ring.activeSelf)
            {
                level = 2;
            }
            if (level == 1)
            {
                timeOutRadialButtonReact.SetActive(false);
                hideUIRadialButtonReact.SetActive(false);
                safeWordRadialButtonReact.SetActive(false);
                oopsWordRadialButtonReact.SetActive(false);
                if (angle > -20 && angle < 20)
                {
                    edgeRadialButtonReact.SetActive(true);
                    tauntRadialButtonReact.SetActive(false);
                    mercyRadialButtonReact.SetActive(false);
                    if(isClick)
                    {
                        edgeRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else if (angle > 20 && angle < 60)
                {
                    edgeRadialButtonReact.SetActive(false);
                    tauntRadialButtonReact.SetActive(true);
                    mercyRadialButtonReact.SetActive(false);
                    if (isClick)
                    {
                        tauntRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else if (angle < -20 && angle > -60)
                {
                    edgeRadialButtonReact.SetActive(false);
                    tauntRadialButtonReact.SetActive(false);
                    mercyRadialButtonReact.SetActive(true);
                    if (isClick)
                    {
                        mercyRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else
                {
                    edgeRadialButtonReact.SetActive(false);
                    tauntRadialButtonReact.SetActive(false);
                    mercyRadialButtonReact.SetActive(false);
                }
            }
            else if (level == 2)
            {
                edgeRadialButtonReact.SetActive(false);
                tauntRadialButtonReact.SetActive(false);
                mercyRadialButtonReact.SetActive(false);
                if (angle > -30 && angle < 0)
                {
                    timeOutRadialButtonReact.SetActive(true);
                    hideUIRadialButtonReact.SetActive(false);
                    safeWordRadialButtonReact.SetActive(false);
                    oopsWordRadialButtonReact.SetActive(false);
                    if (isClick)
                    {
                        timeOutRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else if (angle > 0 && angle < 30)
                {
                    timeOutRadialButtonReact.SetActive(false);
                    hideUIRadialButtonReact.SetActive(false);
                    safeWordRadialButtonReact.SetActive(true);
                    oopsWordRadialButtonReact.SetActive(false);
                    if (isClick)
                    {
                        safeWordRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else if (angle > 30 && angle < 60)
                {
                    timeOutRadialButtonReact.SetActive(false);
                    hideUIRadialButtonReact.SetActive(false);
                    safeWordRadialButtonReact.SetActive(false);
                    oopsWordRadialButtonReact.SetActive(true);
                    if (isClick)
                    {
                        OopsWordRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else if (angle < -30 && angle > -60)
                {
                    timeOutRadialButtonReact.SetActive(false);
                    hideUIRadialButtonReact.SetActive(true);
                    safeWordRadialButtonReact.SetActive(false);
                    oopsWordRadialButtonReact.SetActive(false);
                    if (isClick)
                    {
                        hideUIRadialButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    }
                }
                else
                {
                    timeOutRadialButtonReact.SetActive(false);
                    hideUIRadialButtonReact.SetActive(false);
                    safeWordRadialButtonReact.SetActive(false);
                    oopsWordRadialButtonReact.SetActive(false);
                }
            }

            if(angle < -60 && isClick)
            {
                arousalMinusButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
            }
            if (angle > 60 && isClick)
            {
                arousalPlusButton.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
            }
        }

        public void radialMenuExpand()
        {
            if(!level2Ring.activeSelf && level1Ring.activeSelf)
            {
                radialMenuButton2.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
            }
            else if (!level1Ring.activeSelf)
            {
                radialMenuButton1.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
            }
            else
            {
                exitButtonRadial.GetComponent<PlayMakerFSM>().SendEvent("Click");
            }
        }

        public bool isRadialMenuOpen()
        {
            return level2Ring.activeSelf || level1Ring.activeSelf;
        }

        public void UpdateVSButton(Transform child, string colliderPath, VSButton.VSButtonType buttonType)
        {
            int buttonIndex = Buttons.FindIndex(p => p.name == child.name);

            if (buttonIndex != -1)
            {
                GameObject buttonObject = child.gameObject;
                PlayMakerFSM buttonFsm = child.Find(colliderPath).GetComponent<PlayMakerFSM>();
                VSButton.VSButtonObject buttonObjectWrapper = new VSButton.VSButtonObject()
                {
                    buttonObject = buttonObject,
                    buttonFsm = buttonFsm,
                };
                Buttons[buttonIndex].buttonType = buttonType;

                Buttons[buttonIndex].buttonObjects.Add(buttonObjectWrapper);
                Melon<VSVR>.Logger.Msg($"Adding button {Buttons[buttonIndex].name} with object count {Buttons[buttonIndex].buttonObjects.Count}");
            }
        }

        public void RunButtonNum(int index)
        {
            if (choiceUIObject.activeSelf || stakesUIObject.activeSelf)
            {
                if (index == 4)
                {
                    VSButton selectedButton = Buttons.Find(p => p.buttonType == VSButton.VSButtonType.Normal && p.buttonObjects.Any(q => q.buttonObject.activeSelf));

                    if (selectedButton != null)
                    {
                        PlayMakerFSM activeFsm = selectedButton?.buttonObjects.Find(p => p.buttonObject.activeSelf).buttonFsm;
                        activeFsm.SendEvent("Click");
                    }
                } 
                else if (choiceUIObject.activeSelf)
                {
                    VSButton selectedButton = Buttons.Find(
                        p => 
                            p.buttonNum == index &&
                            (p.buttonType == VSButton.VSButtonType.Choice)
                    );

                    if (selectedButton != null)
                    {
                        PlayMakerFSM activeFsm = selectedButton?.buttonObjects.Find(p => p.buttonObject.activeSelf).buttonFsm;
                        activeFsm.SendEvent("Click");
                    }
                }
                else if (stakesUIObject.activeSelf)
                {
                    VSButton selectedButton = Buttons.Find(
                        p =>
                            p.buttonNum == index &&
                            (p.buttonType == VSButton.VSButtonType.Stakes)
                    );

                    if (selectedButton != null)
                    {
                        PlayMakerFSM activeFsm = selectedButton?.buttonObjects.Find(p => p.buttonObject.activeSelf).buttonFsm;
                        activeFsm.SendEvent("Click");
                    }
                }
            } 
            else
            {
                VSButton selectedButton = Buttons.Find(p => p.buttonNum == index && p.buttonObjects.Any(q => q.buttonObject.activeSelf));

                if (selectedButton != null)
                {
                    PlayMakerFSM activeFsm = selectedButton?.buttonObjects.Find(p => p.buttonObject.activeSelf).buttonFsm;
                    activeFsm.SendEvent("Click");
                }
            }
        }

        public void RunButtonMovement(VSHeadMovement movement)
        {
            VSButton selectedButton = Buttons.Find(p => p.headMovement == movement && p.buttonObjects.Any(q => q.buttonObject.activeSelf));
            PlayMakerFSM activeFsm = selectedButton?.buttonObjects.Find(p => p.buttonObject.activeSelf).buttonFsm;
            
            if (activeFsm != null) 
            {
                activeFsm.SendEvent("Click");
            }
        }
    }
}
