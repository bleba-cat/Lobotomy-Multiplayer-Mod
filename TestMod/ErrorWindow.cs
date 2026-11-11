using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TestMod
{
    public class ErrorWindow : MonoBehaviour
    {
        private static ErrorWindow _instance;
        public static ErrorWindow instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject errorWindow = new GameObject("ErrorWindow");
                    _instance = errorWindow.AddComponent<ErrorWindow>();
                    instance.transform.SetParent(MultiplayerWindowController.instance.SubCategoryWindow);
                    RectTransform ErrorWindowRect = errorWindow.AddComponent<RectTransform>();
                    ErrorWindowRect.anchoredPosition = new Vector2(-600f, 350f);
                    GameObject background = Instantiate(MultiplayerWindowController.instance.Background, errorWindow.transform);
                    background.name = "Background";
                    background.GetComponent<RectTransform>().sizeDelta = new Vector2(1555f, 915f);
                    background.GetComponent<RectTransform>().anchoredPosition = new Vector2(732.5f, -382.5f);
                    GameObject ErrorTextObj = new GameObject("Text");
                    _instance.ErrorText = ErrorTextObj.AddComponent<Text>();
                    RectTransform ErrorTextRT = ErrorTextObj.GetComponent<RectTransform>();
                    ErrorTextObj.AddComponent<FontLoadScript>().neededType = FontType.TITLE;
                    _instance.ErrorText.alignment = TextAnchor.MiddleRight;
                    _instance.ErrorText.resizeTextForBestFit = true;
                    _instance.ErrorText.resizeTextMaxSize = 70;
                    _instance.ErrorText.color = new Color(0.1176f, 1f, 0.6784f, 1f);
                    ErrorTextObj.transform.SetParent(errorWindow.transform, false);
                    ErrorTextRT.anchorMax = new Vector2(1f, 1f);
                    ErrorTextRT.anchorMin = new Vector2(0f, 0f);
                    ErrorTextRT.offsetMax = new Vector2(0f, 0f);
                    ErrorTextRT.offsetMin = new Vector2(0f, 0f);
                    ErrorTextRT.sizeDelta = new Vector2(800f, -60f);
                    ErrorTextRT.anchoredPosition = new Vector2(420f, 0f);
                    GameObject InfoErrorText = MultiplayerWindowController.AddText("Text", "ErrorHowClose", errorWindow.transform, ErrorTextObj.transform, 70);
                    InfoErrorText.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, -60f);
                    InfoErrorText.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -700f);
                }
                return _instance;
            }
        }
        public Text ErrorText;
        XDocument localizationXML;
        GlobalGameManager gameManager;

        void Awake()
        {
            gameManager = GameObject.Find("/GlobalGameManager").GetComponent<GlobalGameManager>();
            using (XmlReader reader = XmlReader.Create(Application.dataPath + "/BaseMods/Test_MOD/localization.xml", new XmlReaderSettings { ProhibitDtd = true }))
            {
                localizationXML = XDocument.Load(reader);
            }
        }

        public void GetError(string Error)
        {
            if (Error == "")
            {
                ErrorText.text = "";
            }
            else if (localizationXML.Root?.Element(gameManager.language)?.Element(Error)?.Value == null)
            {
                ErrorText.text = Error;
            }
            else
            {
                ErrorText.text = localizationXML.Root?.Element(gameManager.language)?.Element(Error)?.Value;
            }
            gameObject.SetActive(true);
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
