using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MainMenuManager : MonoBehaviour{
    [SerializeField] private TMPro.TMP_InputField playerName;
    [SerializeField] private TMPro.TMP_Text scoresText;
    private bool nameHasChanged = false;

    List<string> SortShrinkScoreList(List<string> scoreList){
        // Remove empty strings
        while(scoreList.Remove(""));

        // Sort the scores
        scoreList.Sort((s1, s2) =>
            int.Parse(s2.Split(',')[0]).CompareTo(int.Parse(s1.Split(',')[0]))
        );

        // Remove all scores worse than the top 10
        if(scoreList.Count > 10){
            scoreList.RemoveRange(10, scoreList.Count - 10);    
        }
        return scoreList;
    }

    string BuildScoreList(List<string> scoreList){
        // Store the storeText in the string builder
        StringBuilder result = new StringBuilder(scoresText.text);

        // Add every score with the expected format
        foreach(string line in scoreList){
            string[] scoreData = line.Split(',');
            result.AppendFormat($"\n{scoreData[0]}, {scoreData[1]}");
        }

        // Return the result as a string
        return result.ToString();
    }

    async void Awake(){
        // Set the filepath and playername
        const string filePath = "scores.csv";
        playerName.text = PlayerPrefs.GetString("name", "");

        // Read the score file and store them as an array of lines.
        // Create it and stop, if there is no score file.
        string[] scores;
        try{
            scores = await File.ReadAllLinesAsync(filePath);
        }catch(FileNotFoundException){
            File.Create(filePath);
            return;
        }

        // Sort and shrink the list
        List<string> scoreList = SortShrinkScoreList(new List<string>(scores));

        // Build the list as a string
        scoresText.text = BuildScoreList(scoreList);

        // Write the score list to the score file, keeps the number of lines <= 10
        await File.WriteAllLinesAsync(filePath, scoreList);
    }

    public void OnNameChange() => nameHasChanged = true;

    public void SavePlayerName(){
        if(nameHasChanged){
            PlayerPrefs.SetString("name", playerName.text);
        }
    }
}
