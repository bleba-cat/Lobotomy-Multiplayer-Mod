using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TestMod
{
    [RequireComponent(typeof(Text))]
    public class LoadText : MonoBehaviour
    {
        static GlobalGameManager gameManager;
        static XDocument localizationXML;
        Text text;
        string textID;

        public static void Init()
        {
            gameManager = GameObject.Find("/GlobalGameManager").GetComponent<GlobalGameManager>();
            using (XmlReader reader = XmlReader.Create(Application.dataPath + "/BaseMods/Test_MOD/localization.xml", new XmlReaderSettings { ProhibitDtd = true }))
            {
                localizationXML = XDocument.Load(reader);
            }
        }

        void Awake()
        {
            text = GetComponent<Text>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode Mode)
        {
            SetText(textID);
        }

        public void SetText(string ID)
        {
            textID = ID;
            if (textID == "")
            {
                text.text = "";
            }
            else if (localizationXML.Root?.Element(gameManager.language)?.Element(textID)?.Value == null)
            {
                text.text = textID;
            }
            else text.text = localizationXML.Root?.Element(gameManager.language)?.Element(textID)?.Value;
        }
    }
}