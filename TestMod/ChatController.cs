using TestMod.PacketConvertation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TestMod
{
    public class ChatController : MonoBehaviour
    {
        private static ChatController _instance;
        public static ChatController instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject ChatCanvas = new GameObject("ChatCanvas");
                    ChatCanvas.transform.SetParent(MultiplayerWindowController.instance.transform);
                    ChatCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    ChatCanvas.GetComponent<Canvas>().sortingOrder = 1;
                    ChatCanvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    ChatCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920f, 1080f);
                    ChatCanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
                    ChatCanvas.AddComponent<GraphicRaycaster>();
                    _instance = ChatCanvas.AddComponent<ChatController>(); ;
                    GameObject Chat = new GameObject("Chat");
                    Chat.transform.SetParent(ChatCanvas.transform);
                    _instance.Chat = Chat;
                    RectTransform ChatRT = Chat.AddComponent<RectTransform>();
                    ChatRT.anchorMin = new Vector2(0, 0.5f);
                    ChatRT.anchorMax = new Vector2(0, 0.5f);
                    ChatRT.pivot = new Vector2(0, 0.5f);
                    ChatRT.sizeDelta = new Vector2(576f, 324f);
                    ChatRT.anchoredPosition = new Vector2(20f, 250f);
                    Image ChatImage = Chat.AddComponent<Image>();
                    ChatImage.color = new Color(0, 0, 0, 0.45f);
                    GameObject ChatTextObj = new GameObject("Text");
                    ChatTextObj.transform.SetParent(Chat.transform, false);
                    Text ChatText = ChatTextObj.AddComponent<Text>();
                    ChatText.fontSize = 25;
                    ChatText.alignment = TextAnchor.MiddleLeft;
                    ChatText.color = Color.white;
                    RectTransform ChatTextRT = ChatTextObj.GetComponent<RectTransform>();
                    ChatTextRT.pivot = new Vector2(0f, 1f);
                    ChatTextRT.anchorMin = new Vector2(0f, 1f);
                    ChatTextRT.anchorMax = new Vector2(0f, 1.25f);
                    ChatTextRT.offsetMin = new Vector2(10f, -105f);
                    ChatTextRT.offsetMax = new Vector2(110f, -5f);
                    ChatTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                    ChatTextObj.AddComponent<LoadText>().SetText("Chat");
                    GameObject ScrollView = new GameObject("ScrollView");
                    ScrollView.transform.SetParent(Chat.transform);
                    RectTransform scrollRT = ScrollView.AddComponent<RectTransform>();
                    scrollRT.anchorMin = new Vector2(0, 0);
                    scrollRT.anchorMax = new Vector2(1, 1);
                    scrollRT.offsetMin = new Vector2(10, 60);
                    scrollRT.offsetMax = new Vector2(-10, -30);
                    ScrollRect scrollRect = ScrollView.AddComponent<ScrollRect>();
                    scrollRect.horizontal = false;
                    scrollRect.movementType = ScrollRect.MovementType.Elastic;
                    scrollRect.elasticity = 0.1f;
                    scrollRect.scrollSensitivity = 25f;
                    _instance.scrollRect = scrollRect;
                    GameObject viewport = new GameObject("Viewport");
                    viewport.transform.SetParent(ScrollView.transform);
                    RectTransform viewportRT = viewport.AddComponent<RectTransform>();
                    viewportRT.anchorMin = Vector2.zero;
                    viewportRT.anchorMax = Vector2.one;
                    viewportRT.offsetMin = Vector2.zero;
                    viewportRT.offsetMax = Vector2.zero;
                    Image viewportImage = viewport.AddComponent<Image>();
                    viewportImage.color = new Color(0, 0, 0, 0.3f);
                    viewport.AddComponent<Mask>();
                    scrollRect.viewport = viewportRT;
                    GameObject Сontent = new GameObject("Content");
                    Сontent.transform.SetParent(viewport.transform);
                    RectTransform contentRT = Сontent.AddComponent<RectTransform>();
                    contentRT.anchorMin = new Vector2(0, 1);
                    contentRT.anchorMax = new Vector2(1, 1);
                    contentRT.offsetMin = new Vector2(0f, -20f);
                    contentRT.offsetMax = new Vector2(13f, 0f);
                    contentRT.pivot = new Vector2(0.5f, 1);
                    VerticalLayoutGroup vlg = Сontent.AddComponent<VerticalLayoutGroup>();
                    vlg.childControlHeight = false;
                    vlg.childControlWidth = true;
                    vlg.childForceExpandHeight = false;
                    vlg.childForceExpandWidth = false;
                    vlg.spacing = 5;
                    vlg.padding = new RectOffset(10, 10, 10, 10);
                    ContentSizeFitter csf = Сontent.AddComponent<ContentSizeFitter>();
                    csf.verticalFit = ContentSizeFitter.FitMode.MinSize;
                    scrollRect.content = contentRT;
                    _instance.content = Сontent.transform;
                    GameObject inputObj = new GameObject("InputField");
                    inputObj.transform.SetParent(Chat.transform);
                    RectTransform inputFieldRT = inputObj.AddComponent<RectTransform>();
                    inputFieldRT.pivot = new Vector2(0f, 0f);
                    inputFieldRT.anchorMin = new Vector2(0, 0);
                    inputFieldRT.anchorMax = new Vector2(0.3f, 0.1f);
                    inputFieldRT.offsetMin = new Vector2(10f, 10f);
                    inputFieldRT.offsetMax = new Vector2(250f, 15f);
                    InputField inputField = inputObj.AddComponent<InputField>();
                    _instance.MessageInput = inputField;
                    inputField.characterLimit = 50;
                    GameObject Placeholder = new GameObject("Text");
                    Placeholder.transform.SetParent(inputObj.transform, false);
                    Text PlaceholderText = Placeholder.AddComponent<Text>();
                    PlaceholderText.fontSize = 25;
                    PlaceholderText.alignment = TextAnchor.MiddleLeft;
                    PlaceholderText.color = Color.gray;
                    RectTransform PlaceholderRT = Placeholder.GetComponent<RectTransform>();
                    PlaceholderRT.pivot = Vector2.zero;
                    PlaceholderRT.anchorMin = Vector2.zero;
                    PlaceholderRT.anchorMax = Vector2.zero;
                    PlaceholderRT.offsetMin = new Vector2(5f, 5f);
                    PlaceholderRT.offsetMax = new Vector2(420f, 35f);
                    Placeholder.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                    Placeholder.AddComponent<LoadText>().SetText("Placeholder");
                    inputField.placeholder = Placeholder.GetComponent<Graphic>();
                    Image inputImage = inputObj.AddComponent<Image>();
                    inputImage.color = new Color(0f, 0f, 0f, 0.6f);
                    GameObject inputTextObj = new GameObject("Text");
                    inputTextObj.transform.SetParent(inputObj.transform, false);
                    Text inputText = inputTextObj.AddComponent<Text>();
                    inputText.alignment = TextAnchor.MiddleLeft;
                    inputText.color = Color.white;
                    RectTransform inputTextObjRT = inputTextObj.GetComponent<RectTransform>();
                    inputTextObjRT.pivot = Vector2.zero;
                    inputTextObjRT.anchorMax = Vector2.zero;
                    inputTextObjRT.anchorMin = Vector2.zero;
                    inputTextObjRT.offsetMax = new Vector2(420f, 35f);
                    inputTextObjRT.offsetMin = new Vector2(5f, 5f);
                    inputTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                    inputField.textComponent = inputText;
                    GameObject SendButton = new GameObject("SendButton");
                    SendButton.transform.SetParent(Chat.transform);
                    SendButton.AddComponent<Image>();
                    RectTransform SendButtonRT = SendButton.GetComponent<RectTransform>();
                    SendButtonRT.pivot = new Vector2(1f, 0f);
                    SendButtonRT.anchorMin = new Vector2(1f, 0f);
                    SendButtonRT.anchorMax = new Vector2(1f, 0f);
                    SendButtonRT.offsetMin = new Vector2(-140f, 10f);
                    SendButtonRT.offsetMax = new Vector2(-10f, 47f);
                    Button SendButtonBtn = SendButton.AddComponent<Button>();
                    ColorBlock colors = SendButtonBtn.colors;
                    colors.normalColor = new Color(0f, 0f, 0f, 0.6f);
                    colors.highlightedColor = new Color(0.1f, 0.1f, 0.1f, 0.6f);
                    colors.pressedColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
                    colors.disabledColor = new Color(1f, 0f, 0f, 0.4f);
                    colors.fadeDuration = 0.3f;
                    SendButtonBtn.colors = colors;
                    SendButtonBtn.navigation = new Navigation();
                    SendButtonBtn.interactable = false;
                    SendButtonBtn.onClick.AddListener(_instance.SendMessage);
                    _instance.SendButton = SendButtonBtn;
                    GameObject SendButtonTextObj = new GameObject("Text");
                    SendButtonTextObj.transform.SetParent(SendButton.transform, false);
                    Text SendButtonText = SendButtonTextObj.AddComponent<Text>();
                    SendButtonText.fontSize = 23;
                    SendButtonText.alignment = TextAnchor.MiddleCenter;
                    SendButtonText.color = Color.white;
                    RectTransform SendTextRT = SendButtonTextObj.GetComponent<RectTransform>();
                    SendTextRT.pivot = new Vector2(0.5f, 0.5f);
                    SendTextRT.anchorMin = new Vector2(0.5f, 0.5f);
                    SendTextRT.anchorMax = new Vector2(0.5f, 0.5f);
                    SendTextRT.offsetMin = new Vector2(-54f, -25f);
                    SendTextRT.offsetMax = new Vector2(54f, 25f);
                    SendButtonTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                    SendButtonTextObj.AddComponent<LoadText>().SetText("Send");
                    Chat.SetActive(false);
                }
                return _instance;
            }
        }
        Transform content;
        ScrollRect scrollRect;
        InputField MessageInput;
        public GameObject Chat;
        List<GameObject> Messages = new List<GameObject>();
        public Button SendButton;

        public void AddMessage(string sender, string message)
        {
            bool NeedScroll = scrollRect.verticalNormalizedPosition <= 0.05f;
            GameObject msgObj = new GameObject("Message");
            msgObj.transform.SetParent(content, false);
            RectTransform msgRT = msgObj.AddComponent<RectTransform>();
            msgRT.localScale = Vector3.one;
            msgRT.localPosition = Vector3.zero;
            msgRT.anchoredPosition = Vector2.zero;
            Text msgText = msgObj.AddComponent<Text>();
            msgObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
            msgText.fontSize = 25;
            msgText.color = Color.white;
            msgText.alignment = TextAnchor.UpperLeft;
            msgText.raycastTarget = false;
            msgText.horizontalOverflow = HorizontalWrapMode.Wrap;
            msgText.verticalOverflow = VerticalWrapMode.Overflow;
            LayoutElement layoutElement = msgObj.AddComponent<LayoutElement>();
            layoutElement.flexibleWidth = 1f;
            ContentSizeFitter csf = msgObj.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            msgText.text = sender == "Server" ? $"<color=yellow>{sender}</color>" + " : " + message : sender + " : " + message;
            Messages.Add(msgObj);
            while (Messages.Count > 50)
            {
                Messages.RemoveAt(0);
            }
            if (NeedScroll) StartCoroutine(ScrollToBottom());
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                InputField inputField = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
                if (inputField != MessageInput) return;
            }
            if (Input.GetKeyDown(KeyCode.T)) Chat.SetActive(!Chat.activeSelf);
        }

        public void SendMessage()
        {
            if (MessageInput.text == null) return;
            SerializePacket.ChatMessage(MultiplayerController.SharedWriter, MultiplayerController.instance.nickname, MessageInput.text);
            MultiplayerController.instance.SendPacket(MultiplayerController.SharedWriter, 1);
            MessageInput.text = "";
        }

        IEnumerator ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
