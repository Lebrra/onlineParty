using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    // Pairs with GameManager.cs to deal with anything minigame related

    public Minigame[] allMinigames;

    public List<Minigame> activeMinigames;

    private void Start()
    {
        ResetActivePool();
    }

    public void AddActiveMinigame(string gameName)
    {
        foreach(Minigame m in allMinigames)
        {
            if(m.name == gameName)
            {
                // game found
                if (activeMinigames.Contains(m)) Debug.Log("game is already in active games...");
                else activeMinigames.Add(m);

                return;
            }
        }

        Debug.LogWarning("minigame " + gameName + " not found.");
    }

    public void ResetActivePool()
    {
        activeMinigames = new List<Minigame>();
    }
}

[System.Serializable]
public struct Minigame
{
    public string name;
    public int sceneIndex;
    public Sprite screenshot;
}
