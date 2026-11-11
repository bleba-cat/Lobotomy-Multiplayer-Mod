using Manual;
using UnityEngine;
using UnityEngine.UI;

namespace TestMod
{
    public class MultiplayerWindowController : MonoBehaviour
    {
        private static MultiplayerWindowController _instance;
        public static MultiplayerWindowController instance
        {
            get
            {
                if (_instance == null)
                {
                    Init();
                }
                return _instance;
            }
        }
        public GameObject WindowCanvas;
        public InputField CreateCodeField;
        public InputField ConnectCodeField;
        public InputField HostNickname;
        public InputField ClientNickname;
        public GameObject Background;
        public Transform SubCategoryWindow;
        public LoadText CreateButtonText;
        public LoadText ConnectButtonText;

        public static void Init()
        {
            try
            {
                if (_instance != null) return;
                GameObject MultiplayerUIElements = new GameObject("MultiplayerUIElements");
                //Наиболее вероятно, что загрузка кешированных данных будет более затратная чем поддержание выключенного неизменного gameobject, так что для того, чтобы избежать этих расходов окно помечается как DontDestroyOnLoad
                DontDestroyOnLoad(MultiplayerUIElements); 
                _instance = MultiplayerUIElements.AddComponent<MultiplayerWindowController>();
                //Дабы не выбиваться из стиля целевой игры, копируется руководство, чей интерфейс имеет все необходимые элементы. Поиск по transform используется из-за того, что RootCanvas выключен по умолчанию, а выключенные объекты нельзя найти по GameObject.Find
                GameObject WindowCanvas = Instantiate(GameObject.Find("/GlobalGameManager/ManualUI").transform.Find("RootCanvas").gameObject, MultiplayerUIElements.transform); 
                WindowCanvas.name = "MultiplayerWindowCanvas";
                WindowCanvas.GetComponent<Canvas>().sortingOrder = 0;
                //Устанавливается активное состояние на время выстраивания интерфейса для того, чтобы у всех компонентов могли сработать все методы, предоставляемые жизненного цикла Unity
                WindowCanvas.SetActive(true); 
                _instance.WindowCanvas = WindowCanvas;
                Transform Pivot = WindowCanvas.transform.Find("Pivot");
                Transform VerticalLabel = Pivot.Find("Vertical/VerticalLabel");
                Destroy(Pivot.Find("Title/TitleText").gameObject);
                Pivot.Find("CloseButton/Text").gameObject.AddComponent<LoadText>().SetText("Close");
                //Было определено, что более эффективно заменить существующий текстовый компонент на новый
                GameObject TitleText = AddText("TitleText", "Multiplayer", Pivot.Find("Title"), Pivot.Find("CloseButton/Text"), 60); 
                TitleText.transform.rotation = Quaternion.Euler(0f, 0f, 16f);
                TitleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-225f, -35f);
                //Удаление элементов, которые не были задействованы
                Destroy(Pivot.Find("DataRenderArea").gameObject);
                Destroy(Pivot.Find("CurrentRendererd").gameObject);
                Destroy(Pivot.Find("TextArea").gameObject);
                Destroy(Pivot.Find("Vertical/NodeList_Disabled").gameObject);
                Destroy(VerticalLabel.Find("NodeList").gameObject);
                Destroy(Pivot.Find("Vertical/VerticalLabel_Disabled").gameObject);
                Destroy(Pivot.Find("Frame").gameObject);
                Destroy(Pivot.Find("Horizontal/HorizontalLabel/CategorySlot 2").gameObject);
                Destroy(Pivot.Find("Horizontal/HorizontalLabel/CategorySlot 3").gameObject);
                Destroy(Pivot.Find("Horizontal/HorizontalLabel/CategorySlot 4").gameObject); 
                _instance.Background = Pivot.Find("Block_Background").gameObject;
                Pivot.Find("CloseButton").GetComponent<Button>().onClick.RemoveAllListeners();
                Pivot.Find("CloseButton").GetComponent<Button>().onClick.AddListener(() => { _instance.CloseMenu(); MultiplayerController.instance.DisconnectFromServer(); });
                //Настройка кнопок переключения категорий
                GameObject CreateCategory = Pivot.Find("Horizontal/HorizontalLabel/CategorySlot 0").gameObject;
                CreateCategory.name = "CreateCategory";
                CreateCategory.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("CreateRoom");
                Destroy(CreateCategory.GetComponent<CategorySlot>());
                CreateCategory.GetComponent<Button>().onClick.RemoveAllListeners();
                CreateCategory.GetComponent<Button>().onClick.AddListener(CategoriesButtonControl.SwitchToCreateCategory);
                GameObject ConnectCategory = Pivot.Find("Horizontal/HorizontalLabel/CategorySlot 1").gameObject;
                ConnectCategory.name = "ConnectCategory";
                ConnectCategory.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("JoinRoom");
                Destroy(ConnectCategory.GetComponent<CategorySlot>());
                ConnectCategory.GetComponent<Button>().onClick.RemoveAllListeners();
                ConnectCategory.GetComponent<Button>().onClick.AddListener(CategoriesButtonControl.SwitchhToConnetCategory);
                //Настройка кнопок подразделов
                GameObject CreateSubCategory = VerticalLabel.Find("SubCategorySlot(Clone)").gameObject;
                CreateSubCategory.name = "CreateRoom";
                CreateSubCategory.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("Create");
                Destroy(CreateSubCategory.GetComponent<CategorySlot>());
                CreateSubCategory.GetComponent<Button>().onClick.RemoveAllListeners();
                CreateSubCategory.GetComponent<Button>().onClick.AddListener(() => CategoriesButtonControl.ChangeSubCategory(0));
                GameObject HostPlayersList = VerticalLabel.Find("SubCategorySlot(Clone)").gameObject;
                HostPlayersList.name = "HostPlayersList";
                HostPlayersList.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("ListOfPlayers");
                Destroy(HostPlayersList.GetComponent<CategorySlot>());
                HostPlayersList.GetComponent<Button>().onClick.RemoveAllListeners();
                HostPlayersList.GetComponent<Button>().onClick.AddListener(() => CategoriesButtonControl.ChangeSubCategory(1));
                GameObject StartButton = VerticalLabel.Find("SubCategorySlot(Clone)").gameObject;
                StartButton.name = "StartButton";
                StartButton.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("StartGame");
                Destroy(StartButton.GetComponent<CategorySlot>());
                StartButton.GetComponent<Button>().onClick.RemoveAllListeners();
                StartButton.GetComponent<Button>().onClick.AddListener(() => MultiplayerController.instance.StartGame());
                GameObject ConnectSubCategory = VerticalLabel.Find("SubCategorySlot(Clone)").gameObject;
                ConnectSubCategory.name = "ConnectRoom";
                ConnectSubCategory.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("Join");
                Destroy(ConnectSubCategory.GetComponent<CategorySlot>());
                ConnectSubCategory.GetComponent<Button>().onClick.RemoveAllListeners();
                ConnectSubCategory.GetComponent<Button>().onClick.AddListener(() => CategoriesButtonControl.ChangeSubCategory(0));
                GameObject ClientPlayerList = VerticalLabel.Find("SubCategorySlot(Clone)").gameObject;
                ClientPlayerList.name = "ClientPlayerList";
                ClientPlayerList.transform.Find("Text").gameObject.AddComponent<LoadText>().SetText("ListOfPlayers");
                Destroy(ClientPlayerList.GetComponent<CategorySlot>());
                ClientPlayerList.GetComponent<Button>().onClick.RemoveAllListeners();
                ClientPlayerList.GetComponent<Button>().onClick.AddListener(() => CategoriesButtonControl.ChangeSubCategory(1));
                //Удаление неиспользуемой кнопки
                Destroy(VerticalLabel.Find("SubCategorySlot(Clone)").gameObject);
                //Реализация области, в котором будут располагаться окна в иерархии сцены
                GameObject SubCategoryWindow = new GameObject("SubCategoryWindow");
                RectTransform SubCategoryWindowRT = SubCategoryWindow.AddComponent<RectTransform>();
                SubCategoryWindow.transform.SetParent(Pivot, false);
                SubCategoryWindowRT.anchorMax = Vector2.one;
                SubCategoryWindowRT.anchorMin = Vector2.zero;
                SubCategoryWindowRT.sizeDelta = Vector2.zero;
                _instance.SubCategoryWindow = SubCategoryWindow.transform;
                //Реализация окна, в котором будут расположены элементы управления сессией
                GameObject CreateWindow = new GameObject("CreateWindow");
                RectTransform CreateWindowRT = CreateWindow.AddComponent<RectTransform>();
                CreateWindow.transform.SetParent(SubCategoryWindow.transform, false);
                CreateWindowRT.anchorMax = Vector2.one;
                CreateWindowRT.anchorMin = Vector2.zero;
                CreateWindowRT.sizeDelta = Vector2.zero;
                //Создание надписи "Никнейм"
                GameObject NicknameTextObj = new GameObject("NicknameText");
                NicknameTextObj.transform.SetParent(CreateWindow.transform, false);
                Text NicknameText = NicknameTextObj.AddComponent<Text>();
                NicknameTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                NicknameTextObj.AddComponent<LoadText>().SetText("Nickname");
                NicknameText.alignment = TextAnchor.MiddleLeft;
                NicknameText.resizeTextMaxSize = 35;
                NicknameText.color = new Color(0.6029f, 1f, 0.8553f, 1f);
                NicknameText.resizeTextForBestFit = true;
                RectTransform NicknameTextRT = NicknameTextObj.GetComponent<RectTransform>();
                NicknameTextRT.pivot = new Vector2(0f, 1f);
                NicknameTextRT.anchorMax = new Vector2(0.29f, 0.83f);
                NicknameTextRT.anchorMin = new Vector2(0.19f, 0.77f);
                NicknameTextRT.sizeDelta = Vector2.zero;
                //Создание поля для ввода имени пользователя
                GameObject NicknameInputObject = new GameObject("NicknameInput");
                NicknameInputObject.transform.SetParent(CreateWindow.transform);
                GameObject FrameNicknameInput = Instantiate(VerticalLabel.Find("CreateRoom/Normal").gameObject, NicknameInputObject.transform);
                FrameNicknameInput.name = "Frame";
                RectTransform FrameNicknameInputRT = FrameNicknameInput.GetComponent<RectTransform>();
                FrameNicknameInputRT.sizeDelta = Vector2.zero;
                FrameNicknameInputRT.anchorMax = Vector2.one;
                FrameNicknameInputRT.anchorMin = Vector2.zero;
                Image FrameNicknameInputImage = FrameNicknameInput.GetComponent<Image>();
                Sprite origSprite = FrameNicknameInputImage.sprite;
                //У оригинальных спрайтов не настроены границы для 9-slice компоновки, что мешает масштабированию изображения, так что на основе значений оригинального спрайта рамки создаётся новый с заданными границами
                FrameNicknameInputImage.sprite = Sprite.Create(origSprite.texture, origSprite.rect, origSprite.pivot, origSprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4(30f, 30f, 30f, 30f));
                FrameNicknameInputImage.type = Image.Type.Sliced;
                RectTransform RectNicknameInput = NicknameInputObject.AddComponent<RectTransform>();
                RectNicknameInput.sizeDelta = Vector2.zero;
                RectNicknameInput.anchoredPosition = Vector2.zero;
                RectNicknameInput.pivot = new Vector2(0f, 1f);
                RectNicknameInput.anchorMax = new Vector2(0.415f, 0.8f);
                RectNicknameInput.anchorMin = new Vector2(0.177f, 0.7f);
                InputField NicknameInputField = NicknameInputObject.AddComponent<InputField>();
                NicknameInputField.characterLimit = 20;
                NicknameInputField.onEndEdit.AddListener(MultiplayerController.instance.OnInputEndNickname);
                GameObject NicknameInputTextObj = new GameObject("Text");
                NicknameInputTextObj.transform.SetParent(NicknameInputObject.transform, false);
                Text NicknameInputText = NicknameInputTextObj.AddComponent<Text>();
                NicknameInputTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                NicknameInputText.alignment = TextAnchor.MiddleLeft;
                NicknameInputText.resizeTextForBestFit = true;
                NicknameInputText.resizeTextMaxSize = 35;
                NicknameInputText.color = new Color(0.6029f, 1f, 0.8553f, 1f);
                RectTransform NicknameInputTextRT = NicknameInputTextObj.GetComponent<RectTransform>();
                NicknameInputTextRT.anchorMax = new Vector2(0.945f, 0.75f);
                NicknameInputTextRT.anchorMin = new Vector2(0.065f, 0.25f);
                NicknameInputTextRT.sizeDelta = Vector2.zero;
                NicknameInputField.textComponent = NicknameInputText;
                //Создание кнопки подключения и отключения от сервера
                GameObject CreateRoomButton = Instantiate(StartButton, CreateWindow.transform);
                CreateRoomButton.name = "CreateRoomButton";
                LoadText createLoadText = CreateRoomButton.transform.Find("Text").gameObject.GetComponent<LoadText>();
                createLoadText.SetText("CreateRoom");
                _instance.CreateButtonText = createLoadText;
                RectTransform CreateRoomButtonRT = CreateRoomButton.GetComponent<RectTransform>();
                CreateRoomButtonRT.pivot = new Vector2(0f, 1f);
                CreateRoomButtonRT.anchorMax = new Vector2(0.189f, 0.715f);
                CreateRoomButtonRT.anchorMin = new Vector2(0.189f, 0.715f);
                Destroy(CreateRoomButton.GetComponent<CategorySlot>());
                CreateRoomButton.GetComponent<Button>().onClick.RemoveAllListeners();
                //Создание надписи "Код подключения"
                GameObject CodeTextObj = new GameObject("CodeText");
                CodeTextObj.transform.SetParent(CreateWindow.transform, false);
                Text CodeText = CodeTextObj.AddComponent<Text>();
                CodeTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                CodeTextObj.AddComponent<LoadText>().SetText("ConnectionCode");
                CodeText.alignment = TextAnchor.LowerLeft;
                CodeText.resizeTextForBestFit = true;
                CodeText.resizeTextMaxSize = 35;
                CodeText.color = new Color(0.6029f, 1f, 0.8553f, 1f);
                RectTransform CodeTextRT = CodeTextObj.GetComponent<RectTransform>();
                CodeTextRT.pivot = new Vector2(0f, 1f);
                CodeTextRT.anchorMax = new Vector2(0.34f, 0.66f);
                CodeTextRT.anchorMin = new Vector2(0.19f, 0.62f);
                CodeTextRT.sizeDelta = Vector2.zero;
                //Создание не редактируемого поля ввода для вывода кода сессии
                GameObject CodeField = Instantiate(CreateWindow.transform.Find("NicknameInput").gameObject, CreateWindow.transform);
                CodeField.name = "CodeField";
                RectTransform CodeFieldRect = CodeField.GetComponent<RectTransform>();
                CodeFieldRect.anchorMax = new Vector2(0.525f, 0.69f);
                CodeFieldRect.anchorMin = new Vector2(0.33f, 0.59f);
                InputField CodeInputField = CodeField.GetComponent<InputField>();
                CodeInputField.readOnly = true;
                CodeInputField.characterLimit = 16;
                CodeField.transform.Find("Text").GetComponent<RectTransform>().anchorMin = new Vector2(0.083f, 0.25f);
                //Создание блока с кратким руководством
                GameObject InfoTextObj = new GameObject("InfoText");
                Text InfoText = InfoTextObj.AddComponent<Text>();
                InfoTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                InfoTextObj.AddComponent<LoadText>().SetText("HostInfo");
                InfoText.alignment = TextAnchor.UpperLeft;
                InfoText.resizeTextForBestFit = true;
                InfoText.resizeTextMaxSize = 35;
                InfoText.color = new Color(0.6029f, 1f, 0.8553f, 1f);
                RectTransform InfoTextRT = InfoTextObj.GetComponent<RectTransform>();
                InfoTextObj.transform.SetParent(CreateWindow.transform, false);
                InfoTextRT.pivot = new Vector2(1f, 1f);
                InfoTextRT.anchorMax = new Vector2(0.99f, 0.83f);
                InfoTextRT.anchorMin = new Vector2(0.53f, 0.1f);
                InfoTextRT.sizeDelta = Vector2.zero;
                //На этом этапе реализации окно подключения выглядит примерно одинаково окну созданию, поэтому здесь происходит копирование
                GameObject ConnectWindow = Instantiate(CreateWindow, SubCategoryWindow.transform);
                ConnectWindow.name = "ConnectWindow";
                GameObject ConnectRoomButton = ConnectWindow.transform.Find("CreateRoomButton").gameObject;
                ConnectRoomButton.gameObject.name = "ConnectRoomButton";
                ConnectWindow.transform.Find("NicknameInput").GetComponent<InputField>().onEndEdit.AddListener(MultiplayerController.instance.OnInputEndNickname);
                Destroy(ConnectRoomButton.GetComponent<CategorySlot>());
                RectTransform ConnectRoomButtonRT = ConnectRoomButton.GetComponent<RectTransform>();
                ConnectRoomButtonRT.anchorMax = new Vector2(0.189f, 0.655f);
                ConnectRoomButtonRT.anchorMin = new Vector2(0.189f, 0.655f);
                LoadText connectButtonText = ConnectRoomButton.transform.Find("Text").gameObject.GetComponent<LoadText>();
                connectButtonText.SetText("Join");
                _instance.ConnectButtonText = connectButtonText;
                RectTransform ConnectWindowCodeTextRT = ConnectWindow.transform.Find("CodeText").GetComponent<RectTransform>();
                ConnectWindowCodeTextRT.anchorMax = new Vector2(0.34f, 0.71f);
                ConnectWindowCodeTextRT.anchorMin = new Vector2(0.19f, 0.67f);
                GameObject ConnectCodeField = ConnectWindow.transform.Find("CodeField").gameObject;
                ConnectCodeField.GetComponent<InputField>().readOnly = false;
                ConnectCodeField.GetComponent<InputField>().onEndEdit.AddListener(MultiplayerController.instance.OnInputEndRoomCode);
                RectTransform ConnectCodeFieldRT = ConnectCodeField.GetComponent<RectTransform>();
                ConnectCodeFieldRT.anchorMax = new Vector2(0.525f, 0.74f);
                ConnectCodeFieldRT.anchorMin = new Vector2(0.33f, 0.64f);
                ConnectWindow.transform.Find("InfoText").gameObject.GetComponent<LoadText>().SetText("ClientInfo");
                //Создание надписи "Подключение по подтверждении"
                GameObject TextOfToggle = Instantiate(CodeTextObj, CreateWindow.transform);
                TextOfToggle.name = "Text";
                TextOfToggle.GetComponent<LoadText>().SetText("ConnectionOnConfirm");
                RectTransform TextOfToggleRT = TextOfToggle.GetComponent<RectTransform>();
                TextOfToggleRT.anchorMax = new Vector2(0.46f, 0.597f);
                TextOfToggleRT.anchorMin = new Vector2(0.19f, 0.557f);
                //
                GameObject ConnectionTypeToggleObj = new GameObject("ConnectionOnConfirm");
                ConnectionTypeToggleObj.transform.SetParent(CreateWindow.transform);
                Toggle ConnectionTypeToggle = ConnectionTypeToggleObj.AddComponent<Toggle>();
                ConnectionTypeToggle.transition = Selectable.Transition.None;
                ConnectionTypeToggle.onValueChanged.AddListener(MultiplayerController.instance.NeededConfirm);
                GameObject ConnectionTypeToggleBackground = Instantiate(FrameNicknameInput, ConnectionTypeToggleObj.transform);
                ConnectionTypeToggle.targetGraphic = ConnectionTypeToggleBackground.GetComponent<Image>();
                ConnectionTypeToggleBackground.name = "Background 1";
                RectTransform ConnectionTypeToggleBackgroundRT = ConnectionTypeToggleBackground.GetComponent<RectTransform>();
                ConnectionTypeToggleBackgroundRT.anchorMax = Vector2.one;
                ConnectionTypeToggleBackgroundRT.anchorMin = Vector2.zero;
                ConnectionTypeToggleBackgroundRT.sizeDelta = Vector2.zero;
                GameObject checkmark = new GameObject("Checkmark");
                checkmark.transform.SetParent(ConnectionTypeToggleObj.transform);
                Image checkmarkImage = checkmark.AddComponent<Image>();
                ConnectionTypeToggle.graphic = checkmarkImage;
                checkmarkImage.color = VerticalLabel.transform.Find("HostPlayersList/Text").GetComponent<Text>().color;
                checkmark.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 20f);
                RectTransform ConnectionTypeToggleObjRT = ConnectionTypeToggleObj.GetComponent<RectTransform>();
                ConnectionTypeToggleObjRT.anchoredPosition = Vector2.zero;
                ConnectionTypeToggleObjRT.pivot = new Vector2(0f, 1f);
                ConnectionTypeToggleObjRT.anchorMax = new Vector2(0.45f, 0.612f);
                ConnectionTypeToggleObjRT.anchorMin = new Vector2(0.45f, 0.612f);
                ConnectionTypeToggleObjRT.sizeDelta = new Vector2(80f, 80f);
                Pivot.Find("Title").SetSiblingIndex(10);
                //Сохранение ключевых элеменетов в поля классов
                PlayerList.Init();
                PlayerList.instance.FrameToCopy = FrameNicknameInput;
                PlayerList.instance.ButtonToCopy = StartButton;
                PlayerList.instance.Background = _instance.Background;
                PlayerList.instance.CloseButton = Pivot.Find("CloseButton").gameObject;
                _instance.CreateCodeField = CodeInputField;
                _instance.ConnectCodeField = ConnectCodeField.GetComponent<InputField>();
                _instance.HostNickname = NicknameInputField;
                _instance.ClientNickname = ConnectWindow.transform.Find("NicknameInput").GetComponent<InputField>();
                CreateRoomButton.GetComponent<Button>().onClick.AddListener(() => { if (MultiplayerController.instance.IsConnected) { MultiplayerController.instance.DisconnectFromServer(); } else { MultiplayerController.instance.ConnectWithServer(true); } });
                ConnectRoomButton.GetComponent<Button>().onClick.AddListener(() => { if (MultiplayerController.instance.IsConnected) { MultiplayerController.instance.DisconnectFromServer(); } else { MultiplayerController.instance.ConnectWithServer(false); } });
                _instance.CloseMenu();
            }
            catch (System.Exception ex)
            {
                System.IO.File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", ex.ToString());
            }
        }

        public void OpenMenu()
        {
            WindowCanvas.SetActive(true);
        }

        public void CloseMenu()
        {
            WindowCanvas.SetActive(false);
        }

        public static GameObject AddText(string name, string textID, Transform Parent, Transform TextToCopy, int MaxSize)
        {
            GameObject ObjectText = new GameObject(name);
            Text text = ObjectText.AddComponent<Text>();
            RectTransform rectTransform = ObjectText.GetComponent<RectTransform>();
            FontLoadScript fontLoadScript = ObjectText.AddComponent<FontLoadScript>();
            fontLoadScript.neededType = FontType.TITLE;
            text.alignment = TextAnchor.MiddleRight;
            text.resizeTextForBestFit = true;
            text.resizeTextMaxSize = MaxSize;
            text.color = TextToCopy.GetComponent<Text>().color;
            ObjectText.AddComponent<LoadText>().SetText(textID);
            ObjectText.transform.SetParent(Parent, false);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.offsetMax = new Vector2(0f, 0f);
            rectTransform.offsetMin = new Vector2(0f, 0f);
            rectTransform.sizeDelta = new Vector2(0f, 0f);
            return ObjectText;
        }
    }
}
