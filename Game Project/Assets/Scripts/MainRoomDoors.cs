using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRoomDoors : MonoBehaviour
{
    private RoomTemplates roomTemplates;
    public List<GameObject> Doors;

    void Start()
    {
        roomTemplates = FindObjectOfType<RoomTemplates>();
        Invoke("OpenDoors", roomTemplates.waitTime);
    }

    private void OpenDoors()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Animation animation = Doors[i].GetComponent<Animation>();
            animation.clip = animation.GetClip("DoorOpening");
            animation.Play();
        }
    }
}
