using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileMapObject", menuName = "Scriptable Objects/TileMapObject")]
public class TileMapObject : ScriptableObject
{
    [SerializeField]
    [Tooltip("This should be the prefab GameObject with Grid + Tilemap components")]
    private GameObject tilemapPrefab;

    [SerializeField]
    private TileMapState sceneState;

    public GameObject TilemapPrefab => tilemapPrefab;
    public TileMapState SceneState => sceneState;
}

public enum TileMapState
{
    futureScene,
    pastScene,
    noSceneType
}