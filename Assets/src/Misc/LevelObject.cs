using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelObject", menuName = "Scriptable Objects/LevelObject")]
public class LevelObject : ScriptableObject
{
    public String LevelName;
    [Tooltip("or how many grid spaces span from one side of the room to the other on the camera")]
    public float sizeOfRoomX;
    [Tooltip("or how many grid spaces span from the top of the room to the bottom on the camera")]
    public float sizeOfRoomY;
    [SerializeField]
    public TileMapObject[] pastRooms;

    [SerializeField]
    public TileMapObject[] futureRooms;
    [SerializeField]
    public bool[] RoomStartsInPast;
    
}
