using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneObject", menuName = "Scriptable Objects/SceneObject")]
[Serializable]
public class SceneObject : ScriptableObject
{
/*
    [SerializeField]
    public SceneAsset scene; 
    [SerializeField]
    SceneType sceneState;
    */
}

public enum SceneType{
    futureScene,
    pastScene,
    noSceneType
}
