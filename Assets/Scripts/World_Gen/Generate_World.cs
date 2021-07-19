using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generate_World : MonoBehaviour {
    
    public int roomsToBoss;
    public int minOffshootSize;
    public int maxOffshootSize;

    private int roomCount = 1;

    private Dictionary<string, RoomDefinition> rooms;
    private List<PositionedRoom> positionedRooms;
    private List<PlacedRoom> placedRooms;
    private List<string> listOfRooms;

    private Transform roomsParent;

    void Start() {

        rooms = new Dictionary<string, RoomDefinition>();
        positionedRooms = new List<PositionedRoom>();
        placedRooms = new List<PlacedRoom>();
        roomsParent = GameObject.Find("World").transform;
        
        rooms.Add("Up", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Up"), true, false, false, false));
        rooms.Add("Left", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Left"), false, true, false, false));
        rooms.Add("Down", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Down"), false, false, true, false));
        rooms.Add("Right", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Right"), false, false, false, true));

        rooms.Add("Up-Left", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Up-Left"), true, true, false, false));
        rooms.Add("Left-Down", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Left-Down"), false, true, true, false));
        rooms.Add("Down-Right", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Down-Right"), false, false, true, true));
        rooms.Add("Right-Up", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Right-Up"), true, false, false, true));

        rooms.Add("Up-Left-Down", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Up-Left-Down"), true, true, true, false));
        rooms.Add("Left-Down-Right", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Left-Down-Right"), false, true, true, true));
        rooms.Add("Down-Right-Up", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Down-Right-Up"), true, false, true, true));
        rooms.Add("Right-Up-Left", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Right-Up-Left"), true, true, false, true));

        rooms.Add("Up-Down", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Up-Down"), true, false, true, false));
        rooms.Add("Left-Right", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Left-Right"), false, true, false, true));

        rooms.Add("Up-Left-Down-Right", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Junction"), true, true, true, true));

        listOfRooms = new List<string>();
        string[] roomList = new[] {
            "Up", "Left", "Down", "Right", "Up-Left", "Left-Down", "Down-Right", "Right-Up",
            "Up-Left-Down", "Left-Down-Right", "Down-Right-Up", "Right-Up-Left", "Up-Down",
            "Left-Right", "Up-Left-Down-Right",
        };
        listOfRooms.AddRange(roomList);


        StartCoroutine("BuildWorld");

    }


    IEnumerator BuildWorld() {

        // Add starting room
        positionedRooms.Add(new PositionedRoom(0, 0));

        // Build a path to boss;
        //      Pass one: Determine path to boss
        for(int i = 1; i < roomsToBoss; i++) {

            PositionedRoom currentRoom = positionedRooms[i - 1];

            // Pick a direction for next room
            int nextDirection;
            bool pickingDirection = true;
            int pickAttempts = 0;
            int newX = 0;
            int newY = 0;
            do {
                // Bias the direction picked towards increasing magnitude of coordinates
                nextDirection = UnityEngine.Random.Range(0, 4);

                switch(nextDirection) {
                    case 0:
                        newX = currentRoom.xPosition;
                        newY = currentRoom.yPosition + 1;
                        break;
                    case 1:
                        newX = currentRoom.xPosition - 1;
                        newY = currentRoom.yPosition;
                        break;
                    case 2:
                        newX = currentRoom.xPosition;
                        newY = currentRoom.yPosition - 1;
                        break;
                    case 3:
                        newX = currentRoom.xPosition + 1;
                        newY = currentRoom.yPosition;
                        break;
                }

                pickingDirection = false;

                // Make sure the picked direction is not an already existing room
                if(positionedRooms.Exists(match => match.xPosition == newX && match.yPosition == newY)) {
                    pickingDirection = true;
                } else {

                    if(pickAttempts < 4) {
                        if(Mathf.Abs(newX) < Mathf.Abs(currentRoom.xPosition) || Mathf.Abs(newY) < Mathf.Abs(currentRoom.yPosition)) {
                            // Try to pick again
                            pickingDirection = true;
                            pickAttempts++;
                        }
                    }

                }

                yield return null;
            } while(pickingDirection);

            positionedRooms.Add(new PositionedRoom(newX, newY));

            yield return null;
        }


        // Build a path to boss;
        //      Pass two: Give path directional rooms and create rooms on map
        for(int i = 1; i < roomsToBoss - 1; i++) {

            PositionedRoom previousRoom = positionedRooms[i - 1];
            PositionedRoom currentRoom = positionedRooms[i];
            PositionedRoom nextRoom = positionedRooms[i + 1];

            // Remove dead ends from possible rooms
            List<string> filteredRooms = listOfRooms.Where(name => name.Contains("-")).ToList();

            // Get direction of next room based on coordinates and filter to that direction
            if(nextRoom.xPosition == currentRoom.xPosition && nextRoom.yPosition == currentRoom.yPosition + 1) {
                // Next direction is up, filter to only rooms containing up
                filteredRooms = filteredRooms.Where(name => name.Contains("Up")).ToList();
            } else if(nextRoom.xPosition == currentRoom.xPosition - 1 && nextRoom.yPosition == currentRoom.yPosition) {
                // Next direction is left, filter to only rooms containing left
                filteredRooms = filteredRooms.Where(name => name.Contains("Left")).ToList();
            } else if(nextRoom.xPosition == currentRoom.xPosition && nextRoom.yPosition == currentRoom.yPosition - 1) {
                // Next direction is down, filter to only rooms containing down
                filteredRooms = filteredRooms.Where(name => name.Contains("Down")).ToList();
            } else if(nextRoom.xPosition == currentRoom.xPosition + 1 && nextRoom.yPosition == currentRoom.yPosition) {
                // Next direction is right, filter to only rooms containing right
                filteredRooms = filteredRooms.Where(name => name.Contains("Right")).ToList();
            }


            // Get direction of previous room based on coordinates and filter to that direction
            if(previousRoom.xPosition == currentRoom.xPosition && previousRoom.yPosition == currentRoom.yPosition + 1) {
                // Previous direction was up, filter to only rooms containing up
                filteredRooms = filteredRooms.Where(name => name.Contains("Up")).ToList();
            } else if(previousRoom.xPosition == currentRoom.xPosition - 1 && previousRoom.yPosition == currentRoom.yPosition) {
                // Previous direction was left, filter to only rooms containing left
                filteredRooms = filteredRooms.Where(name => name.Contains("Left")).ToList();
            } else if(previousRoom.xPosition == currentRoom.xPosition && previousRoom.yPosition == currentRoom.yPosition - 1) {
                // Previous direction was down, filter to only rooms containing down
                filteredRooms = filteredRooms.Where(name => name.Contains("Down")).ToList();
            } else if(previousRoom.xPosition == currentRoom.xPosition + 1 && previousRoom.yPosition == currentRoom.yPosition) {
                // Previous direction was right, filter to only rooms containing right
                filteredRooms = filteredRooms.Where(name => name.Contains("Right")).ToList();
            }


            // Find any adjacent rooms that aren't next or previous, disallow connecting to them
            if(positionedRooms.Exists(room => room.xPosition == currentRoom.xPosition && room.yPosition == currentRoom.yPosition + 1
            && (room.xPosition != previousRoom.xPosition && room.yPosition != previousRoom.yPosition)
            && (room.xPosition != nextRoom.xPosition && room.yPosition != nextRoom.yPosition))) {

                // A room exists above, and is not next or previous
                filteredRooms = filteredRooms.Where(name => !name.Contains("Up")).ToList();

            }
            if(positionedRooms.Exists(room => room.xPosition == currentRoom.xPosition - 1 && room.yPosition == currentRoom.yPosition
            && (room.xPosition != previousRoom.xPosition && room.yPosition != previousRoom.yPosition)
            && (room.xPosition != nextRoom.xPosition && room.yPosition != nextRoom.yPosition))) {

                // A room exists left, and is not next or previous
                filteredRooms = filteredRooms.Where(name => !name.Contains("Left")).ToList();

            }
            if(positionedRooms.Exists(room => room.xPosition == currentRoom.xPosition && room.yPosition == currentRoom.yPosition - 1
            && (room.xPosition != previousRoom.xPosition && room.yPosition != previousRoom.yPosition)
            && (room.xPosition != nextRoom.xPosition && room.yPosition != nextRoom.yPosition))) {

                // A room exists below, and is not next or previous
                filteredRooms = filteredRooms.Where(name => !name.Contains("Down")).ToList();

            }
            if(positionedRooms.Exists(room => room.xPosition == currentRoom.xPosition + 1 && room.yPosition == currentRoom.yPosition
            && (room.xPosition != previousRoom.xPosition && room.yPosition != previousRoom.yPosition)
            && (room.xPosition != nextRoom.xPosition && room.yPosition != nextRoom.yPosition))) {

                // A room exists right, and is not next or previous
                filteredRooms = filteredRooms.Where(name => !name.Contains("Right")).ToList();

            }

            // Pick from the remaining list of possible rooms and place
            string roomSelection = filteredRooms[UnityEngine.Random.Range(0, filteredRooms.Count)];
            RoomDefinition newRoom = rooms[roomSelection];
            Instantiate(newRoom.room, new Vector3(currentRoom.xPosition * 25, currentRoom.yPosition * 27, 0), Quaternion.identity, roomsParent);
            placedRooms.Add(new PlacedRoom(currentRoom.xPosition, currentRoom.yPosition, newRoom.openings));

        }

        yield return null;

    }

    /*// Pick starting room
        int amntRooms = rooms.Count;
        int selectedRoom = UnityEngine.Random.Range(0, amntRooms);
        RoomDefinition startingRoom = rooms.ElementAt(selectedRoom).Value;
        Instantiate(startingRoom.room, new Vector3(0, 0, 0), Quaternion.identity, roomsParent);
        placedRooms.Add(new PlacedRoom(0, 0, startingRoom.openings));

        // Build path to boss
        for(int i = 1; i < roomsToBoss; i++) {

            PlacedRoom currentRoom = placedRooms[i - 1];

            // Count openings in current room
            int openings = 0;
            for(int j = 0; j < 4; j++) {
                if(currentRoom.openings[j]) {
                    openings++;
                }
            }

            // Select random opening to be path to boss and sanitize
            int selectedOpening = -1;
            int sanitizedSelection = -1;
            bool openingIsValid = false;
            int attemptsToIncreaseMagnitude = 0;
            do {
                selectedOpening = UnityEngine.Random.Range(0, openings);
                for(int j = 0; j < 4; j++) {
                    if(currentRoom.openings[j]) {
                        if(selectedOpening == 0) {
                            sanitizedSelection = j;
                        }

                        selectedOpening--;
                    }
                }

                openingIsValid = true;
                foreach(PlacedRoom checkRoom in placedRooms) {
                    if(sanitizedSelection == 0 && checkRoom.xPosition == currentRoom.xPosition && checkRoom.yPosition == currentRoom.yPosition + 1) {
                        // Selection is up, but a room already exists there
                        openingIsValid = false;
                        break;
                    }

                    if(sanitizedSelection == 1 && checkRoom.xPosition == currentRoom.xPosition - 1 && checkRoom.yPosition == currentRoom.yPosition) {
                        // Selection is left, but a room already exists there
                        openingIsValid = false;
                        break;
                    }

                    if(sanitizedSelection == 2 && checkRoom.xPosition == currentRoom.xPosition && checkRoom.yPosition == currentRoom.yPosition - 1) {
                        // Selection is down, but a room already exists there
                        openingIsValid = false;
                        break;
                    }

                    if(sanitizedSelection == 3 && checkRoom.xPosition == currentRoom.xPosition + 1 && checkRoom.yPosition == currentRoom.yPosition) {
                        // Selection is right, but a room already exists there
                        openingIsValid = false;
                        break;
                    }

                }

                // Try to bias the direction such that it wont loop back on itself
                if(attemptsToIncreaseMagnitude < 2) {
                    int checkX = currentRoom.xPosition;
                    int checkY = currentRoom.yPosition;
                    if(sanitizedSelection == 0 && openingIsValid) {
                        checkY++;
                    } else if(sanitizedSelection == 1 && openingIsValid) {
                        checkX--;
                    } else if(sanitizedSelection == 2 && openingIsValid) {
                        checkY--;
                    } else if(sanitizedSelection == 3 && openingIsValid) {
                        checkX++;
                    }

                    if(Mathf.Abs(checkX) < Mathf.Abs(currentRoom.xPosition) || Mathf.Abs(checkY) < Mathf.Abs(currentRoom.yPosition)) {
                        attemptsToIncreaseMagnitude++;
                        openingIsValid = false;
                    }
                }


                yield return null;
            } while(!openingIsValid);

            // Build list of candidate rooms based on direction of selected opening
            List<string> candidateRooms = new List<string>();
            int newX = 0;
            int newY = 0;
            if(sanitizedSelection == 0) {
                string[] candidates = new[] {
                    "Left-Down", "Down-Right", "Up-Left-Down", "Left-Down-Right", "Down-Right-Up", "Up-Down", "Junction",
                };
                candidateRooms.AddRange(candidates);

                newX = currentRoom.xPosition;
                newY = currentRoom.yPosition + 1;
            } else if(sanitizedSelection == 1) {
                string[] candidates = new[] {
                    "Down-Right", "Right-Up", "Left-Down-Right", "Down-Right-Up", "Right-Up-Left", "Left-Right", "Junction",
                };
                candidateRooms.AddRange(candidates);

                newX = currentRoom.xPosition - 1;
                newY = currentRoom.yPosition;
            } else if(sanitizedSelection == 2) {
                string[] candidates = new[] {
                    "Up-Left", "Right-Up", "Up-Left-Down", "Down-Right-Up", "Right-Up-Left", "Up-Down", "Junction",
                };
                candidateRooms.AddRange(candidates);

                newX = currentRoom.xPosition;
                newY = currentRoom.yPosition - 1;
            } else if(sanitizedSelection == 3) {
                string[] candidates = new[] {
                    "Up-Left", "Left-Down", "Up-Left-Down", "Left-Down-Right", "Right-Up-Left", "Left-Right", "Junction",
                };
                candidateRooms.AddRange(candidates);

                newX = currentRoom.xPosition + 1;
                newY = currentRoom.yPosition;
            }
            

            // Sanitize candidate rooms - throw out any that involve creating an opening straight into an adjacent wall
            List<string> disallowList = new List<string>();
            foreach(PlacedRoom checkRoom in placedRooms) {

                // Check if the currently checked room is current room, if we disallow
                // based on current room, no layouts will be allowed
                if(checkRoom.xPosition == currentRoom.xPosition && checkRoom.yPosition == currentRoom.yPosition) {
                    continue;
                }

                if(checkRoom.xPosition == newX && checkRoom.yPosition == newY + 1) {
                    // Disallow up openings
                    string[] disallow = new[] {
                        "Up-Left", "Right-Up", "Up-Left-Down", "Down-Right-Up", "Right-Up-Left", "Up-Down", "Junction",
                    };
                    disallowList.AddRange(disallow);
                }

                if(checkRoom.xPosition == newX - 1 && checkRoom.yPosition == newY) {
                    // Disallow left openings
                    string[] disallow = new[] {
                        "Up-Left", "Left-Down", "Up-Left-Down", "Left-Down-Right", "Right-Up-Left", "Left-Right", "Junction",
                    };
                    disallowList.AddRange(disallow);
                }

                if(checkRoom.xPosition == newX && checkRoom.yPosition - 1 == newY) {
                    // Disallow down openings
                    string[] disallow = new[] {
                        "Left-Down", "Down-Right", "Up-Left-Down", "Left-Down-Right", "Down-Right-Up", "Up-Down", "Junction",
                    };
                    disallowList.AddRange(disallow);
                }

                if(checkRoom.xPosition == newX + 1 && checkRoom.yPosition == newY) {
                    // Disallow right openings
                    string[] disallow = new[] {
                        "Down-Right", "Right-Up", "Left-Down-Right", "Down-Right-Up", "Right-Up-Left", "Left-Right", "Junction",
                    };
                    disallowList.AddRange(disallow);
                }
            }


            // Remove from candidates if on disallow list
            foreach(string checkCandidate in candidateRooms.ToArray()) {
                if(disallowList.Exists(x => String.Equals(x, checkCandidate))) {
                    candidateRooms.Remove(checkCandidate);
                }
            }


            // Select random candidate room
            string selectedCandidate = candidateRooms[UnityEngine.Random.Range(0, candidateRooms.Count)];

            // Place selected room
            RoomDefinition newRoom = rooms[selectedCandidate];
            Instantiate(newRoom.room, new Vector3(newX * 25, newY * 27, 0), Quaternion.identity, roomsParent);
            placedRooms.Add(new PlacedRoom(newX, newY, newRoom.openings));


            yield return null;
        }*/


}
