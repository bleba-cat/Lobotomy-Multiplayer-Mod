using Assets.Scripts.UI.Utils;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TestMod
{
    public static class UIAdd
    {
        public static void AddAlterUI()
        {
            MultiplayerWindowController.Init();
            CategoriesButtonControl.Init();
            ChatController.instance.SendButton.interactable = false;
            GameObject.Find("/AlterTitleRoot/Canvas/MainButtonPanel/ButtonLayout").GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 100f);
            Button ButtonToCopy = GameObject.Find("/AlterTitleRoot").transform.Find("Canvas/MainButtonPanel/ButtonLayout/Button_Tutorial").GetComponent<Button>();
            GameObject MultiplayerButtonObj = new GameObject("Button_Multiplayer");
            RectTransform MultiplayerButtonRT = MultiplayerButtonObj.AddComponent<RectTransform>();
            MultiplayerButtonRT.pivot = new Vector2(1f, 1f);
            MultiplayerButtonObj.AddComponent<CanvasRenderer>();
            Button MultiplayerButton = MultiplayerButtonObj.AddComponent<Button>();
            MultiplayerButton.animationTriggers = ButtonToCopy.animationTriggers;
            MultiplayerButton.transition = ButtonToCopy.transition;
            MultiplayerButton.onClick.AddListener(MultiplayerWindowController.instance.OpenMenu);
            Animator animator = MultiplayerButtonObj.AddComponent<Animator>();
            animator.runtimeAnimatorController = ButtonToCopy.GetComponent<Animator>().runtimeAnimatorController;
            EventTrigger eventTrigger = MultiplayerButtonObj.AddComponent<EventTrigger>();
            eventTrigger.triggers = ButtonToCopy.GetComponent<EventTrigger>().triggers;
            MultiplayerButtonObj.transform.SetParent(GameObject.Find("/AlterTitleRoot").transform.Find("Canvas/MainButtonPanel/ButtonLayout").transform, false);
            MultiplayerButtonObj.transform.SetSiblingIndex(2);
            GameObject ButtonTextObj = new GameObject("Text");
            Text ButtonText = ButtonTextObj.AddComponent<Text>();
            ButtonText.alignment = TextAnchor.MiddleRight;
            ButtonText.resizeTextForBestFit = true;
            ButtonText.resizeTextMaxSize = 70;
            RectTransform ButtonTextRT = ButtonTextObj.GetComponent<RectTransform>();
            ButtonTextObj.AddComponent<LoadText>().SetText("MULTIPLAYER");
            ButtonTextObj.transform.SetParent(MultiplayerButtonObj.transform, false);
            ButtonTextRT.anchorMax = new Vector2(1f, 1f);
            ButtonTextRT.anchorMin = new Vector2(0f, 0f);
            ButtonTextRT.offsetMax = new Vector2(0f, 0f);
            ButtonTextRT.offsetMin = new Vector2(0f, 0f);
            ButtonTextRT.sizeDelta = new Vector2(0f, 0f);
            ButtonTextObj.AddComponent<FontLoadScript>().neededType = FontType.CONTEXT;
        }

        public static void AddNewUI()
        {
            MultiplayerWindowController.Init();
            CategoriesButtonControl.Init();
            GameObject MultiplayerButton = Object.Instantiate(GameObject.Find("/TitleCanvas/InnerLayer/MenuFrame/ButtonArea/Tutorial"), GameObject.Find("/TitleCanvas/InnerLayer/MenuFrame/ButtonArea").transform);
            MultiplayerButton.name = "MultiplayerButton";
            MultiplayerButton.GetComponent<RectTransform>().localRotation = new Quaternion(0f, 0f, 0.3007f, -0.9537f);
            Object.Destroy(MultiplayerButton.transform.Find("Text").GetComponent<LocalizeTextLoadScript>());
            MultiplayerButton.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("MULTIPLAYER");
            MultiplayerButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1257f, 660f);
            EventTrigger ButtonEventTrigger = MultiplayerButton.GetComponent<EventTrigger>();
            ButtonEventTrigger.triggers.Clear();
            EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
            triggerEvent.AddListener((a) => MultiplayerWindowController.instance.OpenMenu());
            ButtonEventTrigger.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.PointerClick, callback = triggerEvent });
            GameObject EmptyImage = new GameObject("ForTrigger");
            RectTransform EmptyImageRect = EmptyImage.AddComponent<RectTransform>();
            EmptyImageRect.anchoredPosition = new Vector2(0f, 40f);
            EmptyImageRect.sizeDelta = new Vector2(400f, 200f);
            EmptyImage.transform.SetParent(MultiplayerButton.transform, false);
            EmptyImage.transform.SetSiblingIndex(0);
            EmptyImage.AddComponent<Image>().color = new Color(0, 0, 0, 0);
            GameObject MultiplayerButtonImage = new GameObject("MultiplayerButtonImage");
            MultiplayerButtonImage.SetActive(false);
            triggerEvent = new EventTrigger.TriggerEvent();
            triggerEvent.AddListener((a) => MultiplayerButtonImage.SetActive(true));
            ButtonEventTrigger.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter, callback = triggerEvent });
            triggerEvent = new EventTrigger.TriggerEvent();
            triggerEvent.AddListener((a) => MultiplayerButtonImage.SetActive(false));
            ButtonEventTrigger.triggers.Add(new EventTrigger.Entry { eventID = EventTriggerType.PointerExit, callback = triggerEvent });
            MultiplayerButtonImage.transform.SetParent(GameObject.Find("/TitleCanvas/InnerLayer").transform);
            MultiplayerButtonImage.transform.SetSiblingIndex(1);
            byte[] MultiplayerButtonTextureBytes = File.ReadAllBytes(Application.dataPath + "/BaseMods/Test_MOD/MultiplayerButton.png");
            Texture2D MultiplayerButtonTexture = new Texture2D(1, 1);
            MultiplayerButtonTexture.LoadImage(MultiplayerButtonTextureBytes);
            RawImage image = MultiplayerButtonImage.AddComponent<RawImage>();
            image.texture = MultiplayerButtonTexture;
            MultiplayerButtonImage.GetComponent<RectTransform>().sizeDelta = new Vector2(15.45f, 11.33f);
            MultiplayerButtonImage.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 1f);
            MultiplayerButtonImage.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            MultiplayerButtonImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-382f, -279f);
        }
    }
}
