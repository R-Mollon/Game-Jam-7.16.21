using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_Structures : MonoBehaviour {
}


public class RoomDefinition {

    public RoomDefinition(GameObject room, bool up, bool left, bool down, bool right) {
        this.room = room;

        openings = new[] {
            up, left, down, right
        };
    }

    public GameObject room;

    public bool[] openings;

}


public class PositionedRoom {

    public PositionedRoom(int x, int y) {
        xPosition = x;
        yPosition = y;
    }

    public int xPosition;
    public int yPosition;

}


public class PlacedRoom {

    public PlacedRoom(int x, int y, bool[] openings) {
        xPosition = x;
        yPosition = y;

       this.openings = openings;
    }

    public bool hasOpenings() {
        return (openings[0] || openings[1] || openings[2] || openings[3]);
    }

    public int xPosition;
    public int yPosition;

    public bool[] openings;

}