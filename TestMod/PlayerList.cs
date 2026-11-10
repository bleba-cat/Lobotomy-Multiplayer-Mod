using TestMod.PacketConvertation;
using Manual;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TestMod
{
    public class PlayerList : MonoBehaviour
    {
        private static PlayerList _instance;
        public static PlayerList instance
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
        public static void Init()
        {
            if (_instance != null) return;
            GameObject playerList = new GameObject("PlayerList");
            playerList.transform.SetParent(MultiplayerWindowController.instance.SubCategoryWindow, false);
            _instance = playerList.AddComponent<PlayerList>();
            GameObject PermissionsWindow = new GameObject("PermissionsWindow");
            PermissionsWindow.transform.SetParent(MultiplayerWindowController.instance.SubCategoryWindow, false);
            RectTransform PermissionsWindowRT = PermissionsWindow.AddComponent<RectTransform>();
            PermissionsWindowRT.anchorMax = new Vector2(1f, 0.94f);
            PermissionsWindowRT.anchorMin = new Vector2(0.1372f, 0f);
            PermissionsWindowRT.sizeDelta = Vector2.zero;
            PermissionsWindowRT.pivot = new Vector2(0f, 1f);
            _instance.PermissionsWindow = PermissionsWindow.transform;
            VerticalLayoutGroup PlayerGroup = playerList.AddComponent<VerticalLayoutGroup>();
            PlayerGroup.childControlHeight = false;
            PlayerGroup.childControlWidth = true;
            PlayerGroup.childForceExpandHeight = false;
            PlayerGroup.childForceExpandWidth = true;
            PlayerGroup.spacing = -35f;
            RectTransform playerListRT = playerList.GetComponent<RectTransform>();
            playerListRT.sizeDelta = Vector2.zero;
            playerListRT.pivot = new Vector2(0f, 1f);
            playerListRT.anchorMax = new Vector2(0.87f, 0.9f);
            playerListRT.anchorMin = new Vector2(0.17f, 0.05f);
            GameObject ListTextObj = new GameObject("Text");
            Text ListText = ListTextObj.AddComponent<Text>();
            ListTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
            ListTextObj.AddComponent<LoadText>().SetText("Players");
            ListText.alignment = TextAnchor.MiddleLeft;
            ListText.resizeTextForBestFit = true;
            ListText.resizeTextMaxSize = 35;
            ListText.color = new Color(0.6029f, 1f, 0.8553f, 1f);
            RectTransform ListTextRT = ListTextObj.GetComponent<RectTransform>();
            ListTextObj.transform.SetParent(playerList.transform, false);
            ListTextRT.sizeDelta = new Vector2(100f, 100f);
        }

        List<PlayerInList> players = new List<PlayerInList>();
        XDocument localizationXML;
        public GameObject FrameToCopy;
        public Transform PermissionsWindow;
        public GameObject ButtonToCopy;
        public GameObject Background;
        public GameObject CloseButton;

        public void CreateNewPlayerList(List<Player> Players, bool IsClient)
        {
            try
            {
                ClearList();
                foreach (Player player in Players)
                {
                    (GameObject listObject, GameObject playerPermissions) = AddPlayerInList(player, IsClient);
                    players.Add(new PlayerInList { Player = player, ListObject = listObject, PermissionsWindow = playerPermissions });
                }
                players.Sort((p1, p2) => p1.Player.Nickname.CompareTo(p2.Player.Nickname));
                PlayerInList Host = players.Find(p => p.Player.IsHost);
                players.Remove(Host);
                players.Insert(0, Host);
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].ListObject.GetComponent<Transform>().SetSiblingIndex(i + 1);
                }
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(Application.dataPath + "/BaseMods/Blebas_Multiplayer_MOD/Bleba_Log.txt", ex.ToString());
            }
        }

        public void ClearList()
        {
            foreach (PlayerInList playerInList in players)
            {
                Destroy(playerInList.ListObject);
                if (playerInList.PermissionsWindow != null) Destroy(playerInList.PermissionsWindow);
            }
            players.Clear();
        }


        string GetText(string textID)
        {
            string textRes = "";
            try
            {
                if (localizationXML == null)
                {
                    using (XmlReader reader = XmlReader.Create(Application.dataPath + "/BaseMods/Blebas_Multiplayer_MOD/localization.xml", new XmlReaderSettings { ProhibitDtd = true }))
                    {
                        localizationXML = XDocument.Load(reader);
                    }
                }
                if (textID == "")
                {
                    textRes = "";
                }
                else if (localizationXML.Root?.Element(GlobalGameManager.instance.language)?.Element(textID)?.Value == null)
                {
                    textRes = textID;
                }
                else
                {
                    textRes = localizationXML.Root?.Element(GlobalGameManager.instance.language)?.Element(textID)?.Value;
                }
                return textRes;
            }
            catch (System.Exception ex)
            {
                System.IO.File.WriteAllText(Application.dataPath + "/BaseMods/Blebas_Multiplayer_MOD/Bleba_Log.txt", ex.ToString());
                return textRes;
            }
        }

        public GameObject AddToggle(string name, Transform parent, bool IsOn, UnityAction<bool> call)
        {
            GameObject ObjectToggle = new GameObject(name);
            ObjectToggle.transform.SetParent(parent);
            Toggle ConnectionTypeToggle = ObjectToggle.AddComponent<Toggle>();
            ConnectionTypeToggle.transition = Selectable.Transition.None;
            ConnectionTypeToggle.onValueChanged.AddListener(MultiplayerController.instance.NeededConfirm);
            GameObject ConnectionTypeToggleBackground = Instantiate(FrameToCopy, ObjectToggle.transform);
            ConnectionTypeToggle.targetGraphic = ConnectionTypeToggleBackground.GetComponent<Image>();
            ConnectionTypeToggleBackground.name = "Background 1";
            RectTransform ConnectionTypeToggleBackgroundRT = ConnectionTypeToggleBackground.GetComponent<RectTransform>();
            ConnectionTypeToggleBackgroundRT.anchorMax = Vector2.one;
            ConnectionTypeToggleBackgroundRT.anchorMin = Vector2.zero;
            ConnectionTypeToggleBackgroundRT.sizeDelta = Vector2.zero;
            GameObject checkmark = new GameObject("Checkmark");
            checkmark.transform.SetParent(ObjectToggle.transform);
            Image checkmarkImage = checkmark.AddComponent<Image>();
            ConnectionTypeToggle.graphic = checkmarkImage;
            checkmarkImage.color = new Color(0.1176f, 1f, 0.6784f, 1f);
            checkmark.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 20f);
            return ObjectToggle;
        }
        public (GameObject ListObject, GameObject PermissionWindow) AddPlayerInList(Player player, bool IsClient)
        {
            //Создание элемента, в котором будет располагаться рамка, имя пользователя и, если текущий пользователь владелец сессии, элементы управления статусом пользователя
            GameObject ListObject = new GameObject(player.Nickname);
            ListObject.transform.SetParent(transform);
            RectTransform ListObjectRT = ListObject.AddComponent<RectTransform>();
            ListObjectRT.sizeDelta = new Vector2(0f, 101f);
            ListObjectRT.pivot = new Vector2(0f, 1f);
            GameObject Frame = Instantiate(FrameToCopy, ListObject.transform);
            Frame.name = "Frame";
            RectTransform FrameRT = Frame.GetComponent<RectTransform>();
            FrameRT.anchorMax = Vector2.one;
            FrameRT.anchorMin = Vector2.zero;
            GameObject Nickname = new GameObject("Text");
            Text NicknameText = Nickname.AddComponent<Text>();
            NicknameText.alignment = TextAnchor.MiddleLeft;
            NicknameText.resizeTextForBestFit = true;
            NicknameText.resizeTextMaxSize = 35;
            NicknameText.color = new Color(0.1176f, 1f, 0.6784f, 1f);
            NicknameText.text = player.IsHost ? player.Nickname + " - Host" : player.IsConfirmed ? player.Nickname : player.Nickname + " - ?";
            Nickname.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
            RectTransform NicknameRT = Nickname.GetComponent<RectTransform>();
            Nickname.transform.SetParent(ListObject.transform, false);
            NicknameRT.sizeDelta = Vector2.zero;
            NicknameRT.anchorMax = new Vector2(0.2f, 1f);
            NicknameRT.anchorMin = new Vector2(0.03f, 0f);
            GameObject PlayerPermissions = null;
            if (!player.IsHost && !IsClient)
            {
                //Окно для ранее запланированных разрешений, но по итогу реализовано не было
                PlayerPermissions = new GameObject(player.Nickname + "Permissions");
                PlayerPermissions.transform.SetParent(PermissionsWindow);
                PlayerPermissions.SetActive(false);
                RectTransform PlayerPermissionsRT = PlayerPermissions.AddComponent<RectTransform>();
                PlayerPermissionsRT.anchoredPosition = Vector2.zero;
                PlayerPermissionsRT.anchorMax = Vector2.one;
                PlayerPermissionsRT.anchorMin = Vector2.zero;
                PlayerPermissionsRT.sizeDelta = Vector2.zero;
                PlayerPermissionsRT.pivot = new Vector2(0f, 1f);
                GameObject background = Instantiate(Background, PlayerPermissions.transform);
                background.name = "Background";
                RectTransform backgroundRT = background.GetComponent<RectTransform>();
                GameObject perm1 = AddToggle("perm1", PlayerPermissions.transform, player.Permissions.perm1, (state) => player.Permissions.perm1 = state);
                RectTransform perm1RT = perm1.GetComponent<RectTransform>();
                perm1RT.anchoredPosition = Vector2.zero;
                perm1RT.pivot = new Vector2(0f, 1f);
                perm1RT.anchorMax = new Vector2(0.45f, 0.612f);
                perm1RT.anchorMin = new Vector2(0.45f, 0.612f);
                perm1RT.sizeDelta = new Vector2(80f, 80f);
                GameObject closeButton = Instantiate(CloseButton, PlayerPermissions.transform);
                closeButton.name = "CloseButton";
                RectTransform closeButtonRT = closeButton.GetComponent<RectTransform>();
                closeButtonRT.anchorMax = new Vector2(1f, 0f);
                closeButtonRT.anchorMin = new Vector2(1f, 0f);
                closeButtonRT.offsetMax = new Vector2(27f, 140f);
                closeButtonRT.offsetMin = new Vector2(-439f, -28f);
                closeButton.GetComponent<Button>().onClick.AddListener(() => { SerializePacket.PlayerPermissions(MultiplayerController.SharedWriter, player); MultiplayerController.instance.SendPacket(MultiplayerController.SharedWriter, 1); PlayerPermissions.SetActive(false); });
                //Управляющие кнопки в элементе списка
                GameObject PermissionsButton = Instantiate(ButtonToCopy, ListObject.transform);
                PermissionsButton.name = "PermissionsButton";
                RectTransform PermissionButtonRect = PermissionsButton.GetComponent<RectTransform>();
                GameObject PermissionText = PermissionsButton.transform.Find("Text").gameObject;
                PermissionButtonRect.sizeDelta = new Vector2(160f, 60f);
                PermissionButtonRect.anchoredPosition = new Vector2(865f, -20.8f);
                PermissionText.GetComponent<RectTransform>().sizeDelta = new Vector2(420f, 104f);
                PermissionText.GetComponent<RectTransform>().anchoredPosition = new Vector2(58f, 0f);
                Destroy(PermissionsButton.transform.Find("Text").GetComponent<LoadText>());
                PermissionText.GetComponent<Text>().text = GetText("Permissions");
                PermissionsButton.transform.Find("Normal").GetComponent<RectTransform>().sizeDelta = new Vector2(212.5f, 101f);
                PermissionsButton.transform.Find("Selected").GetComponent<RectTransform>().sizeDelta = new Vector2(175f, 53f);
                Destroy(PermissionsButton.GetComponent<CategorySlot>());
                PermissionsButton.SetActive(player.IsConfirmed);
                GameObject CickButton = Instantiate(PermissionsButton, ListObject.transform);
                CickButton.name = "CickButton";
                CickButton.transform.Find("Text").GetComponent<Text>().text = GetText("Cick");
                CickButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1075f, -20.8f);
                CickButton.transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(65f, 0f);
                Destroy(CickButton.GetComponent<CategorySlot>());
                CickButton.SetActive(player.IsConfirmed);
                if (!player.IsConfirmed)
                {
                    GameObject ConfirmButton = Instantiate(PermissionsButton, ListObject.transform);
                    ConfirmButton.name = "ConfirmButton";
                    ConfirmButton.transform.Find("Text").GetComponent<Text>().text = GetText("Confirm");
                    ConfirmButton.transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(54f, 0f);
                    Destroy(ConfirmButton.GetComponent<CategorySlot>());
                    ConfirmButton.SetActive(true);
                    GameObject RejectButton = Instantiate(CickButton, ListObject.transform);
                    RejectButton.name = "RejectButton";
                    RejectButton.transform.Find("Text").GetComponent<Text>().text = GetText("Reject");
                    RejectButton.transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(69f, 0f);
                    Destroy(RejectButton.GetComponent<CategorySlot>());
                    ConfirmButton.GetComponent<Button>().onClick.AddListener(() => { SerializePacket.ConfirmUser(MultiplayerController.SharedWriter, player.Nickname); MultiplayerController.instance.SendPacket(MultiplayerController.SharedWriter, 1); });
                    RejectButton.GetComponent<Button>().onClick.AddListener(() => { SerializePacket.KickUser(MultiplayerController.SharedWriter, player.Nickname); MultiplayerController.instance.SendPacket(MultiplayerController.SharedWriter, 1); });
                    RejectButton.SetActive(true);
                }
                PermissionsButton.GetComponent<Button>().onClick.AddListener(() => PlayerPermissions.SetActive(true));
                CickButton.GetComponent<Button>().onClick.AddListener(() => { SerializePacket.KickUser(MultiplayerController.SharedWriter, player.Nickname); MultiplayerController.instance.SendPacket(MultiplayerController.SharedWriter, 1); });
            }

            return (ListObject, PlayerPermissions);
        }
    }

    class PlayerInList
    {
        public Player Player { get; set; }
        public GameObject ListObject;
        public GameObject PermissionsWindow;
    }
}
