using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager:MonoBehaviour {


    [SerializeField]private static int coins = 0;
    [SerializeField]public static int score = 0;
    [SerializeField]public static int speed = 2;
    [SerializeField]public static bool shieldPlayer = false;
    [SerializeField]public static bool isGameBoosted = false;

    [SerializeField]public Text coinsText;
    [SerializeField]public Text scoreText;

    public static string PlayerPrefString;
    public static List<string> ExportList= new List<string>();
    public static List<string> PlayerList= new List<string>();

    private static GameManager instance;
    
    public static GameManager Instance {
        get {
            if(instance==null) {
                instance = new GameManager();
            }
 
            return instance;
        }
    }

    private void Awake(){
        instance = this;
        
    }

    private static void Start(){
       if (score == 0){
        print("Start");
        instance.StartCoroutine("AddScores");
       }

    }

    public static void AddCoin(){
      coins++;  
      instance.coinsText.text = coins.ToString()+"x";
    }

    public static void GameOver(){
        CalculateHiScore();
        Handheld.Vibrate();
        print("GameOver");
        SceneManager.LoadScene("HisScore");
    }

    public static void BoostGame(){
        if(!isGameBoosted){
          Time.timeScale = 2.0f;
          isGameBoosted=true;
        }
    }

    public static void UnBoostGame(){
        if(isGameBoosted){
          Time.timeScale = 1.0f;
          isGameBoosted=false;
        }
    }

    public static void CalculateHiScore(){
        //Fetch All Scores
        PlayerPrefString = PlayerPrefs.GetString("ProfileInfo");
        print(PlayerPrefString);
        StringToList(PlayerPrefString, "[EndInp123]");
        string currentPlayerName = PlayerPrefs.GetString("CurrentPlayer");
        //Ashraf[EndInp123]200[EndInp123]Anna[EndInp123]100[EndInp123]Jack[EndInp123]150[EndInp123]

        ///For Old Players
        for(int i=0;i<=ExportList.Count-1;i++){
            print(ExportList[i]);

            if(ExportList[i]==currentPlayerName){
                if(int.Parse(ExportList[i+1])<coins){
                    ExportList[i+1] = coins.ToString();
                    print("Updated Hi Score of "+ExportList[i]);
                    UpdatePlayerPrefs();
                    return;
                }
            }


            print(ExportList[i+1]);
            //Add the options created in the List above
            i++;
        }

        //For New Player
        for(int i=0;i<=ExportList.Count-1;i++){
            print(ExportList[i]);

            if(int.Parse(ExportList[i+1])<=coins){
                ///Rearrange the Score Board
                print("Found A Hi Score to be remembered");
                ExportList.Add(currentPlayerName);
                ExportList.Add(coins.ToString());
                UpdatePlayerPrefs();
                return;
            }

            //Add the options created in the List above
            i++;
        }

        //Just a Score very Low of a New Player.
        ExportList.Add(currentPlayerName);
        ExportList.Add(coins.ToString());

        UpdatePlayerPrefs();

    }

    static void UpdatePlayerPrefs(){
        PlayerPrefString = "";
        print(ExportList);
        ///Save in Player Prefs
        for(int i=0;i<=ExportList.Count-2;i++){
            PlayerPrefString = PlayerPrefString + ExportList[i] + "[EndInp123]" +ExportList[i+1]+"[EndInp123]";
            //Add the options created in the List above
            i++;
        }
        PlayerPrefs.SetString("ProfileInfo",PlayerPrefString);
    }


    static void StringToList(string message, string seperator)
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