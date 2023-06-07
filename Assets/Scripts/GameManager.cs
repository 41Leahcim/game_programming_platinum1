using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour{
    private const string scoreFile = "scores.csv";

    public void LoadScene(string scene){
        SceneManager.LoadSceneAsync(scene);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void SaveScore(int score){
        string name = PlayerPrefs.GetString("name", "Player");
        try{
            File.AppendAllTextAsync(scoreFile, string.Format("{0},{1}", score, name));
        }catch(FileNotFoundException){
            File.Create(scoreFile);
            File.WriteAllTextAsync(scoreFile, string.Format("{0},{1}", score, name));
        }
    }
}
