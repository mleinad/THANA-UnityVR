using System;
using System.Collections.Generic;
using Eflatun.SceneReference;

[Serializable]
public class SceneGroup
{
    public string GroupName = "New Scene Group";
    public List<SceneData> Scenes;

    public string FindSceneNameByType(SceneType sceneType)
    {
        return Scenes.Find(scene => scene.SceneType == sceneType).Name;
    }
}

[Serializable]
public class SceneData
{
    public SceneReference Refrence;
    public string Name => Refrence.Name;
    public SceneType SceneType;
}

public enum SceneType
{
    ActiveScene,
    MainMenu,
    Room
}