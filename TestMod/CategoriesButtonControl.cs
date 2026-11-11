using UnityEngine;
using UnityEngine.UI;

namespace TestMod
{
    public static class CategoriesButtonControl
    {
        static Button CreateButtonSwitch;
        static Button ConnectButtonSwitch;
        static GameObject CreateCategory;
        static GameObject HostPlayerListCategory;
        static GameObject StartCategory;
        static GameObject ConnectCategory;
        static GameObject ClientPlayerListCategory;
        static GameObject CreateRoomCategory;
        static GameObject ConnectRoomCategory;
        static GameObject PlayerListCategory;
        static int Category;

        public static void Init()
        {
            if (CreateButtonSwitch != null) return;
            Transform Pivot = GameObject.Find("/MultiplayerUIElements").transform.Find("MultiplayerWindowCanvas/Pivot");
            CreateButtonSwitch = Pivot.Find("Horizontal/HorizontalLabel/CreateCategory").GetComponent<Button>();
            ConnectButtonSwitch = Pivot.Find("Horizontal/HorizontalLabel/ConnectCategory").GetComponent<Button>();
            CreateCategory = Pivot.Find("Vertical/VerticalLabel/CreateRoom").gameObject;
            HostPlayerListCategory = Pivot.Find("Vertical/VerticalLabel/HostPlayersList").gameObject;
            StartCategory = Pivot.Find("Vertical/VerticalLabel/StartButton").gameObject;
            ConnectCategory = Pivot.Find("Vertical/VerticalLabel/ConnectRoom").gameObject;
            ClientPlayerListCategory = Pivot.Find("Vertical/VerticalLabel/ClientPlayerList").gameObject;
            CreateRoomCategory = Pivot.Find("SubCategoryWindow/CreateWindow").gameObject;
            ConnectRoomCategory = Pivot.Find("SubCategoryWindow/ConnectWindow").gameObject;
            PlayerListCategory = PlayerList.instance.gameObject;
            SwitchToCreateCategory();
        }

        public static void SwitchToCreateCategory()
        {
            Category = 0;
            CreateButtonSwitch.interactable = false;
            ConnectButtonSwitch.interactable = true;
            CreateCategory.SetActive(true);
            HostPlayerListCategory.SetActive(true);
            StartCategory.SetActive(true);
            ConnectCategory.SetActive(false);
            ClientPlayerListCategory.SetActive(false);
            ChangeSubCategory(0);
        }

        public static void SwitchhToConnetCategory()
        {
            Category = 1;
            CreateButtonSwitch.interactable = true;
            ConnectButtonSwitch.interactable = false;
            CreateCategory.SetActive(false);
            HostPlayerListCategory.SetActive(false);
            StartCategory.SetActive(false);
            ConnectCategory.SetActive(true);
            ClientPlayerListCategory.SetActive(true);
            ChangeSubCategory(0);
        }

        public static void ChangeSubCategory(int PressedButton)
        {
            if (Category == 0)
            {
                switch (PressedButton)
                {
                    case 0:
                        {
                            CreateCategory.GetComponent<Button>().interactable = false;
                            HostPlayerListCategory.GetComponent<Button>().interactable = true;
                            CreateRoomCategory.SetActive(true);
                            ConnectRoomCategory.SetActive(false);
                            PlayerListCategory.SetActive(false);
                            break;
                        }
                    case 1:
                        {
                            CreateCategory.GetComponent<Button>().interactable = true;
                            HostPlayerListCategory.GetComponent<Button>().interactable = false;
                            CreateRoomCategory.SetActive(false);
                            ConnectRoomCategory.SetActive(false);
                            PlayerListCategory.SetActive(true);
                            break;
                        }
                }
            }
            else if (Category == 1)
            {
                switch (PressedButton)
                {
                    case 0:
                        {
                            ConnectCategory.GetComponent<Button>().interactable = false;
                            ClientPlayerListCategory.GetComponent<Button>().interactable = true;
                            CreateRoomCategory.SetActive(false);
                            ConnectRoomCategory.SetActive(true);
                            PlayerListCategory.SetActive(false);
                            break;
                        }
                    case 1:
                        {
                            ConnectCategory.GetComponent<Button>().interactable = true;
                            ClientPlayerListCategory.GetComponent<Button>().interactable = false;
                            CreateRoomCategory.SetActive(false);
                            ConnectRoomCategory.SetActive(false);
                            PlayerListCategory.SetActive(true);
                            break;
                        }
                }
            }
        }
    }
}