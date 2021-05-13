using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    // Pairs with GameManager.cs to deal with anything minigame related

    public Minigame[] allMinigames;

    public List<Minigame> activeMinigames;

    public GameObject minigameScreen;

    private void Start()
    {
        GameBoardConnector.inst?.minigameScreen.SetActive(false);
        ResetActivePool();
    }

    Minigame GetMinigameFromString(string gameName)
    {
        foreach (Minigame m in allMinigames)
        {
            if (m.name == gameName)
            {
                return m;
            }
        }
        Debug.LogError("minigame not found. " + gameName);
        Minigame error = new Minigame();
        error.sceneIndex = -1;
        return error;
    }

    public void AddActiveMinigame(string gameName)
    {
        Minigame tempData = GetMinigameFromString(gameName);

        if (activeMinigames.Contains(tempData)) Debug.Log("game is already in active games...");
        else if (tempData.sceneIndex > 0) activeMinigames.Add(tempData);
        else Debug.LogWarning("error adding game " + gameName);
    }

    public void ResetActivePool()
    {
        activeMinigames = new List<Minigame>();
    }

    public IEnumerator GoToMinigame(string minigame)
    {
        Minigame currentGame = GetMinigameFromString(minigame);
        if (currentGame.sceneIndex < 1)
        {
            Debug.LogError("unable to load minigame " + minigame);
            yield break;
        }

        GameBoardConnector.inst?.EnableMinigameScreen(currentGame.screenshot);

        //minigameScreen.SetActive(true);
        //minigameScreen.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = currentGame.screenshot;

        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(currentGame.sceneIndex);
        //SceneManager.LoadScene(3);      //TEMP UNTIL ALL MINIGAMES FUNCTION
    }
}

[System.Serializable]
public struct Minigame
{
    public string name;
    public int sceneIndex;
    public Sprite screenshot;
}
