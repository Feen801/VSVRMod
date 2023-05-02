using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;
using static RootMotion.FinalIK.RagdollUtility;

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
            new VSButton() { name = "Done1", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Done2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Taunt", buttonNum = 2 },
            new VSButton() { name = "Taunt2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Mercy", buttonNum = 1 },
            new VSButton() { name = "Mercy2", buttonNum = 3 },
            new VSButton() { name = "Mercy3", buttonNum = 2 },
            new VSButton() { name = "MercyNoFavor", buttonNum = 1 },
            new VSButton() { name = "Decline", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Yes", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "No", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed2", buttonNum = 2, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed3", buttonNum = 2, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Failed4", buttonNum = 1, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Sorry", buttonNum = 2 },
            new VSButton() { name = "Sorry2", buttonNum = 1 },
            new VSButton() { name = "Ok", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ok2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Thank You", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Thank You2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Thank You3", buttonNum = 3, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Pay", buttonNum = 2 },
            new VSButton() { name = "Pay2", buttonNum = 3 },
            new VSButton() { name = "Wait", buttonNum = 3 },
            new VSButton() { name = "Continue", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ready", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Ready2", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Beg", buttonNum = 3 },
            new VSButton() { name = "Beg2", buttonNum = 2 },
            new VSButton() { name = "Beg2NoFavor", buttonNum = 2 },
            new VSButton() { name = "Disobey", buttonNum = 2, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobey2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobey3", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobeyed", buttonNum = 2, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Disobeyed2", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Confirm", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Bye", buttonNum = 2 },
            new VSButton() { name = "Spin", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Pass", buttonNum = 2 },
            new VSButton() { name = "EdgeCentered", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "EdgeLeft", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "RhythmEdge", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "EdgeEnding", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "FinishCentered", buttonNum = 2 },
            new VSButton() { name = "ConfirmNo", buttonNum = 1 },
            new VSButton() { name = "Good", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Bad", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "Uh Oh", buttonNum = 2 },
            new VSButton() { name = "Obey", buttonNum = 2, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Accept", buttonNum = 1, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "Cumming", buttonNum = 2 },
            new VSButton() { name = "EndSession", buttonNum = 1, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "ContinueSession", buttonNum = 3, headMovement = VSHeadMovement.Nod },
            new VSButton() { name = "GoEasy", buttonNum = 2 },
            new VSButton() { name = "Refuse", buttonNum = 3, headMovement = VSHeadMovement.Headshake },
            new VSButton() { name = "FavoriteHeart", buttonNum = 2 },
            new VSButton() { name = "OpportunityProvoke", buttonNum = 2 },
            new VSButton() { name = "OpportunityTaunt", buttonNum = 2 },
            new VSButton() { name = "OpportunityEntice", buttonNum = 2 },
            new VSButton() { name = "OpportunityPraise", buttonNum = 2 },

            // Stakes buttons
            new VSButton() { name = "BG1", buttonNum = 1 },
            new VSButton() { name = "BG2", buttonNum = 2 },
            new VSButton() { name = "BG3", buttonNum = 3},

            // Choice buttons
            new VSButton() { name = "Choice1", buttonNum = 1 },
            new VSButton() { name = "Choice2", buttonNum = 3 },

            // new VSButton() { name = "PoTMercy" },

            // 4 - analog stick click
            new VSButton() { name = "Oops", buttonNum = 4 },
            new VSButton() { name = "OopsCentered", buttonNum = 4 },
            new VSButton() { name = "NoEdgeOops", buttonNum = 4 },
            new VSButton() { name = "NoEdgeOopsCentered", buttonNum = 4 },
            new VSButton() { name = "ForcedOops", buttonNum = 4 },
            new VSButton() { name = "OopsNo", buttonNum = 4 },
        };
        
        public void PopulateButtons()
        {
            GameObject buttonParent = GameObject.Find("GeneralCanvas/EventManager/Buttons");
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
