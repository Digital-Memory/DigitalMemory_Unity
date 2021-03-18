using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : Singleton<LevelHandler>
{
    void OnGUI()
    {
        if (Input.GetKey(KeyCode.L))
        {
            int i = 0;

            foreach (int index in Game.Settings.playableLevels)
            {
                if (GUI.Button(new Rect(50, 50 + 60 * i, 400, 50), "Load: " + Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(index)).Split('.')[0]))
                {
                    TryLoad(index);
                }
                i++;
            }
        }
    }

    public void TryLoad(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
