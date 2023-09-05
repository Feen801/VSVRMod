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
        GameObject choiceUIObject;
        GameObject choiceLeftOption;
        GameObject choiceLeftOptionReact;
        GameObject choiceRightOption;
        GameObject choiceRightOptionReact;
        GameObject favoriteButton;
        GameObject favoriteButtonReact;

        GameObject stakesUIObject;
        GameObject stakesTopOption;
        GameObject stakesMiddleOption;
        GameObject stakesBottomOption;
        GameObject stakesTopOptionReact;
        GameObject stakesMiddleOptionReact;
        GameObject stakesBottomOptionReact;

        // Mappings for all VS buttons
        // Order matters, first in list takes priority
        readonly List<VSButton> Buttons = new List<VSButton>()
        {
            //Opportunities
            new VSButton() { name = "OpportunityProvoke", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "OpportunityTaunt", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "OpportunityEntice", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "OpportunityPraise", buttonNum = 1, headMovement = VSHeadMovement.Nod },

            //Urges
            //new VSButton() { name = "IgnoreButton", buttonNum = 1, headMovement = VSHeadMovement.Headshake },
            //new VSButton() { name = "GiveIn", buttonNum = 1, headMovement = VSHeadMovement.Headshake },

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
            new VSButton() { name = "GoFaster", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Goon", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Edged", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            
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

        GameObject ignoreButton;
        GameObject giveInButton;
        GameObject urgeActionDisplay;
        public void PopulateButtons()
        {
            //For opportunities
            GameObject buttonParent = GameObject.Find("GeneralCanvas/EventManager/Buttons");
            foreach (Transform child in buttonParent.transform)
            {
                UpdateVSButton(child, "DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            }

            buttonParent = GameObject.Find("GeneralCanvas/EventManager/Buttons/Positives ------------");
            foreach (Transform child in buttonParent.transform)
            {
                UpdateVSButton(child, "DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            }

            buttonParent = GameObject.Find("GeneralCanvas/EventManager/Buttons/Negatives ------------");
            foreach (Transform child in buttonParent.transform)
            {
                UpdateVSButton(child, "DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            }

            ignoreButton = GameObject.Find("GeneralCanvas/EventManager/Urges/ActionTextContainer/IgnoreButton");
            //UpdateVSButton(buttonParent.transform, "DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            giveInButton = GameObject.Find("GeneralCanvas/EventManager/Urges/ActionTextContainer/GiveIn");
            //UpdateVSButton(buttonParent.transform, "GiveInButton/DoneBG/DoneText/Collider", VSButton.VSButtonType.Normal);
            urgeActionDisplay = GameObject.Find("GeneralCanvas/EventManager/Urges/ActionTextContainer");

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


            stakesUIObject = GameObject.Find("GeneralCanvas/EventManager/StakesUI");
            stakesTopOption = GameObject.Find("GeneralCanvas/EventManager/StakesUI/BG1");
            stakesMiddleOption = GameObject.Find("GeneralCanvas/EventManager/StakesUI/BG2");
            stakesBottomOption = GameObject.Find("GeneralCanvas/EventManager/StakesUI/BG3");
            stakesTopOptionReact = GameObject.Find("GeneralCanvas/EventManager/StakesUI/BG1");
            stakesMiddleOptionReact = GameObject.Find("GeneralCanvas/EventManager/StakesUI/BG2");
            stakesBottomOptionReact = GameObject.Find("GeneralCanvas/EventManager/StakesUI/BG3");


            choiceUIObject = GameObject.Find("GeneralCanvas/EventManager/ChoiceUI");
            choiceLeftOption = GameObject.Find("GeneralCanvas/EventManager/ChoiceUI/Choice1");
            choiceLeftOptionReact = GameObject.Find("GeneralCanvas/EventManager/ChoiceUI/Choice1/Image (1)/Borders/LightBorder");
            choiceRightOption = GameObject.Find("GeneralCanvas/EventManager/ChoiceUI/Choice2");
            choiceRightOptionReact = GameObject.Find("GeneralCanvas/EventManager/ChoiceUI/Choice2/Image (1)/Borders/LightBorder");

            favoriteButton = GameObject.Find("GeneralCanvas/EventManager/Buttons/FavoriteHeart");
            favoriteButtonReact = GameObject.Find("GeneralCanvas/EventManager/Buttons/FavoriteHeart/DoneBG/DoneText/Collider/ButtonPressReact1");
        }

        public void RadialMenuInteract(bool isClick, double angle)
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

        public void RadialMenuExpand()
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

        public bool IsRadialMenuOpen()
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

        public void RunButtonMovement(VSHeadMovement movement)
        {
            if(urgeActionDisplay.activeSelf)
            {
                if(movement == VSHeadMovement.Headshake)
                {
                    ignoreButton.GetComponent<PlayMakerFSM>().SendEvent("Click");
                }
                else if (movement == VSHeadMovement.Nod)
                {
                    giveInButton.GetComponent<PlayMakerFSM>().SendEvent("Click");
                }
            }
            VSButton selectedButton = Buttons.Find(p => p.headMovement == movement && p.buttonObjects.Any(q => q.buttonObject.activeSelf));
            PlayMakerFSM activeFsm = selectedButton?.buttonObjects.Find(p => p.buttonObject.activeSelf).buttonFsm;
            
            if (activeFsm != null) 
            {
                activeFsm.SendEvent("Click");
            }
        }

        public void StakesUIInteract(bool isClick, double height)
        {
            if(height > 0.3)
            {
                stakesTopOptionReact.SetActive(true);
                stakesMiddleOptionReact.SetActive(false);
                stakesBottomOptionReact.SetActive(false);
                if (isClick)
                {
                    stakesTopOption.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    RunButtonMovement(VSHeadMovement.Nod);
                }
            }
            else if (height > -0.3)
            {
                stakesTopOptionReact.SetActive(false);
                stakesMiddleOptionReact.SetActive(true);
                stakesBottomOptionReact.SetActive(false);
                if (isClick)
                {
                    stakesMiddleOption.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    RunButtonMovement(VSHeadMovement.Nod);
                }
            }
            else
            {
                stakesTopOptionReact.SetActive(false);
                stakesMiddleOptionReact.SetActive(false);
                stakesBottomOptionReact.SetActive(true);
                if (isClick)
                {
                    stakesBottomOption.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    RunButtonMovement(VSHeadMovement.Nod);
                }
            }
        }

        public bool StakesUIActive()
        {
            return stakesUIObject.activeSelf;
        }

        public void ChoiceUIInteract(bool isClick, double width)
        {
            if (width > 0.3)
            {
                choiceRightOptionReact.SetActive(true);
                favoriteButtonReact.SetActive(false);
                choiceLeftOptionReact.SetActive(false);
                if (isClick)
                {
                    choiceRightOption.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    RunButtonMovement(VSHeadMovement.Nod);
                }
            }
            else if (width > -0.3)
            {
                choiceRightOptionReact.SetActive(false);
                favoriteButtonReact.SetActive(true);
                choiceLeftOptionReact.SetActive(false);
                if (isClick && favoriteButton.activeSelf)
                {
                    favoriteButton.transform.Find("DoneBG/DoneText/Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    RunButtonMovement(VSHeadMovement.Nod);
                }
            }
            else
            {
                favoriteButtonReact.SetActive(false);
                stakesMiddleOptionReact.SetActive(false);
                choiceLeftOptionReact.SetActive(true);
                if (isClick)
                {
                    choiceLeftOption.transform.Find("Collider").GetComponent<PlayMakerFSM>().SendEvent("Click");
                    RunButtonMovement(VSHeadMovement.Nod);
                }
            }
        }

        public bool ChoiceUIActive()
        {
            return choiceUIObject.activeSelf;
        }
    }
}
