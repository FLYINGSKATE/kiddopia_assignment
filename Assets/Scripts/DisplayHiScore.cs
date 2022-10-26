using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class DisplayHiScore : MonoBehaviour
{
    public Text hiScoreList;
    public string PlayerPrefString;
    public List<string> ExportList;
    public List<string> PlayerList;
    // Start is called before the first frame update
    void Start()
    {
        displayHiScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void displayHiScore(){
        PlayerPrefString = PlayerPrefs.GetString("ProfileInfo");
        StringToList(PlayerPrefString, "[EndInp123]");
        SortHiScores();

    }


    void SortHiScores()
    {

        Dictionary<string, int> myDict = new Dictionary<string, int>();
        myDict.Clear(); 

        for(int i=0;i<=ExportList.Count-2;i++){
            print(ExportList[i]);
            print(ExportList[i+1]);

            if(myDict.ContainsKey(ExportList[i])){
                myDict[ExportList[i]] = int.Parse(ExportList[i+1]);
            }
            else{
                myDict.Add(ExportList[i],int.Parse(ExportList[i+1]));
            }
            i++;
        }

        var sortedDict = from entry in myDict orderby entry.Value descending select entry;

        foreach(KeyValuePair<string, int> entry in sortedDict)
        {
            hiScoreList.text = hiScoreList.text+entry.Key;
            hiScoreList.text = hiScoreList.text+entry.Value+"\n";
        }


        
    }


    public void RestartGame(){
        SceneManager.LoadScene("MainScene");
    }

    public void GoToHomeScene(){
        SceneManager.LoadScene("Home");
    }

    void StringToList(string message, string seperator)
     {
         ExportList.Clear();
         string tok = "";
         foreach(char character in message)
         {
             tok = tok + character;
             if (tok.Contains(seperator))
             {
                tok = tok.Replace(seperator, "");
                ExportList.Add(tok);
                tok = "";
             }
         }
     }

}
