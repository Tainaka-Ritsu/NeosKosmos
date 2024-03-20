using KinglandStudio.NeosKosmos.Data.BasicValue;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KinglandStudio.NeosKosmos.Data.Manager
{
    public class SettingManager : MonoBehaviour
    {
        public void ResetValueInGame(string target, object obj)
        {
            GetComponent<ResoucesManager>().basicValues.SetArg(target, obj);
            Write();
            GetComponent<ResoucesManager>().ResetInformation();
        }
        public void ReturnToGameScene()
        {
            SceneManager.LoadScene("BasicInGame");
            GetComponent<UserOperatingManager>().enabled = true;
        }
        public void Quit()
        {
            Application.Quit();
        }
        public void Write()
        {
            String stringJson = JsonUtility.ToJson(GameObject.Find("VisualManager").GetComponent<ResoucesManager>().basicValues);
            StreamWriter streamWriter = new(Application.dataPath + "/setting.data");
            streamWriter.Write(stringJson);
            streamWriter.Close();
        }
        private void Read()
        {
            StreamReader streamReader = new StreamReader(Application.dataPath + "/setting.data");
            String stringJson = streamReader.ReadToEnd();
            streamReader.Close();
            BasicValueInGame game = JsonUtility.FromJson<BasicValueInGame>(stringJson);
            GameObject.Find("VisualManager").GetComponent<ResoucesManager>().basicValues = game;
            GameObject.Find("VisualManager").GetComponent<ResoucesManager>().SetValueBasic();
            GameObject.Find("VisualManager").GetComponent<ResoucesManager>().ResetInformation();
        }
        private void Start()
        {
            if (File.Exists(Application.dataPath + "/setting.data"))
            {
                Read();
            }
            else
            {
                String stringJson = JsonUtility.ToJson(GameObject.Find("VisualManager").GetComponent<ResoucesManager>().basicValues = new BasicValueInGame());
                StreamWriter streamWriter = new(Application.dataPath + "/setting.data");
                streamWriter.Write(stringJson);
                streamWriter.Close();
            }
        }
    }
}