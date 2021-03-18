using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] LevelSelectionCube cubePrefab;
    [SerializeField] List<LevelIndexCubeInfoHolder> cubes = new List<LevelIndexCubeInfoHolder>();
    int inFokus = 0;

    private void Start()
    {
        int i = 0;
        foreach (int index in Game.Settings.playableLevels)
        {
            LevelIndexCubeInfoHolder info = new LevelIndexCubeInfoHolder(index, Instantiate(cubePrefab));
            info.cube.Init(this, i, Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(info.sceneIndex)).Split('.')[0]);
            cubes.Add(info);
            i++;
        }
    }

    internal void ClickOn(LevelSelectionCube cube)
    {
        int fokus = 0;

        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i].cube == cube)
                fokus = i;
        }

        if (fokus == inFokus)
            Game.LevelHandler.TryLoad(cubes[fokus].sceneIndex);

        inFokus = fokus;
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].cube.LerpToNewIndex(i - inFokus);
        }
    }
}

[System.Serializable]
public class LevelIndexCubeInfoHolder
{
    public LevelIndexCubeInfoHolder(int index, LevelSelectionCube c)
    {
        sceneIndex = index;
        cube = c;
    }

    public int sceneIndex;
    public LevelSelectionCube cube;
}
