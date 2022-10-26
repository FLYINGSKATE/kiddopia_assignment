using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitiateGameInfo : MonoBehaviour
{

    public string PlayerPrefString;
    public List<string> ExportList;
    public List<string> PlayerList;
    public Dropdown m_Dropdown;
    public InputField newProfileUserNameInputField;
    public string currentPlayerName = "";
    public Text welcomeName;
    

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

    // Start is called before the first frame update
    void Start()
    {
        
        //Clear the old options of the Dropdown menu
        m_Dropdown.ClearOptions();

        PlayerPrefString = PlayerPrefs.GetString("ProfileInfo");
        
        if(PlayerPrefString==""){
            PlayerPrefs.SetString("ProfileInfo", "Ashraf[EndInp123]200[EndInp123]Anna[EndInp123]100[EndInp123]");
            PlayerPrefString = PlayerPrefs.GetString("ProfileInfo");
        }
        print("PlayerPrefString");
        print(PlayerPrefString);
        StringToList(PlayerPrefString, "[EndInp123]");
        for(int i=0;i<=ExportList.Count-1;i++){
            print(ExportList[i]);
            PlayerList.Add(ExportList[i]);
            //Add the options created in the List above
            
            i++;
        }
        m_Dropdown.AddOptions(PlayerList);
        welcomeName.text = "Welcome "+PlayerList[m_Dropdown.value];

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayBtn(){



        if(currentPlayerName=="")
        {
            PlayerPrefs.SetString("CurrentPlayer",PlayerList[m_Dropdown.value]);
        }
        else
        {
            PlayerPrefs.SetString("CurrentPlayer",currentPlayerName);
        }
        SceneManager.LoadScene("MainScene");
    }

    public void AddNewProfile(){
        currentPlayerName = newProfileUserNameInputField.text;
        PlayerPrefs.SetString("CurrentPlayer",currentPlayerName);
        SceneManager.LoadScene("MainScene");
    }

    public void LoadProfileScene(){
        SceneManager.LoadScene("Profile");
    }

    



    


}
