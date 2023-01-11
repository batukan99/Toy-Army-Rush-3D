using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoaderCommand
{
    private GameObject _levelHolder;
    public void Execute(int levelID)
    {
        Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/{levelID}"),
                _levelHolder.transform);
    }
}
