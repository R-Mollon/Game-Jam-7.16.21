using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class Generate_World : MonoBehaviour {
    
    public int roomsToBoss;

    private Dictionary<string, RoomDefinition> rooms;
    private List<PositionedRoom> positionedRooms;
    private List<PlacedRoom> placedRooms;
    private List<string> listOfRooms;

    private GameObject[] roomDecorations;

    private Transform roomsParent;

    void Start() {

        rooms = new Dictionary<string, RoomDefinition>();
        positionedRooms = new List<PositionedRoom>();
        placedRooms = new List<PlacedRoom>();
        roomsParent = GameObject.Find("World").transform;

        roomDecorations = Resources.LoadAll<GameObject>("Prefabs/Room-Decoration");
        
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

        rooms.Add("Boss-Up", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Boss-Up"), false, false, false, false));
        rooms.Add("Boss-Left", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Boss-Left"), false, false, false, false));
        rooms.Add("Boss-Down", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Boss-Down"), false, false, false, false));
        rooms.Add("Boss-Right", new RoomDefinition(Resources.Load<GameObject>("Prefabs/Rooms/Boss-Right"), false, false, false, false));

        listOfRooms = new List<string>();
        string[] roomList = new[] {
            "Up", "Left", "Down", "Right", "Up-Left", "Left-Down", "Down-Right", "Right-Up",
            /*"Up-Left-Down", "Left-Down-Right", "Down-Right-Up", "Right-Up-Left",*/ "Up-Down",
            "Left-Right", //"Up-Left-Down-Right",
        };
        listOfRooms.AddRange(roomList);


        StartCoroutine("BuildWorld");

    }


    void Generate() {
        positionedRooms = new List<PositionedRoom>();
        placedRooms = new List<PlacedRoom>();

        foreach(Transform child in roomsParent) {
            Destroy(child.gameObject);
        }

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
            int maxPickAttempts = 4;
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
                    if(pickAttempts < maxPickAttempts) {
                        if(Mathf.Abs(newX) < Mathf.Abs(currentRoom.xPosition) || Mathf.Abs(newY) < Mathf.Abs(currentRoom.yPosition)) {
                            // Try to pick again
                            pickingDirection = true;
                            pickAttempts++;
                        }
                    }
                }
            } while(pickingDirection);

            positionedRooms.Add(new PositionedRoom(newX, newY));

            yield return null;
        }


        // Assign starting room direction
        string startingRoomDirection = "Up";
        if(positionedRooms[1].yPosition == 1) {
            startingRoomDirection = "Up";
        } else if(positionedRooms[1].xPosition == -1) {
            startingRoomDirection = "Left";
        } else if(positionedRooms[1].yPosition == -1) {
            startingRoomDirection = "Down";
        } else if(positionedRooms[1].xPosition == 1) {
            startingRoomDirection = "Right";
        }
        RoomDefinition newRoom = rooms[startingRoomDirection];
        Instantiate(newRoom.room, new Vector3(0, 0, 0), Quaternion.identity, roomsParent);
        placedRooms.Add(new PlacedRoom(0, 0, new[] {false, false, false, false}));



        // Build a path to boss;
        //      Pass two: Give path directional rooms and create rooms on map
        for(int i = 1; i < roomsToBoss - 1; i++) {

            PositionedRoom previousRoom = positionedRooms[i - 1];
            PositionedRoom currentRoom = positionedRooms[i];
            PositionedRoom nextRoom = positionedRooms[i + 1];

            int nextRoomDir = 0;
            int prevRoomDir = 0;

            // Remove dead ends from possible rooms
            List<string> filteredRooms = listOfRooms.Where(name => name.Contains("-")).ToList();


            // Get direction of next room based on coordinates and filter to that direction
            if(nextRoom.xPosition == currentRoom.xPosition && nextRoom.yPosition == currentRoom.yPosition + 1) {
                // Next direction is up, filter to only rooms containing up
                filteredRooms = filteredRooms.Where(name => name.Contains("Up")).ToList();
                nextRoomDir = 0;
            } else if(nextRoom.xPosition == currentRoom.xPosition - 1 && nextRoom.yPosition == currentRoom.yPosition) {
                // Next direction is left, filter to only rooms containing left
                filteredRooms = filteredRooms.Where(name => name.Contains("Left")).ToList();
                nextRoomDir = 1;
            } else if(nextRoom.xPosition == currentRoom.xPosition && nextRoom.yPosition == currentRoom.yPosition - 1) {
                // Next direction is down, filter to only rooms containing down
                filteredRooms = filteredRooms.Where(name => name.Contains("Down")).ToList();
                nextRoomDir = 2;
            } else if(nextRoom.xPosition == currentRoom.xPosition + 1 && nextRoom.yPosition == currentRoom.yPosition) {
                // Next direction is right, filter to only rooms containing right
                filteredRooms = filteredRooms.Where(name => name.Contains("Right")).ToList();
                nextRoomDir = 3;
            }


            // Get direction of previous room based on coordinates and filter to that direction
            if(previousRoom.xPosition == currentRoom.xPosition && previousRoom.yPosition == currentRoom.yPosition + 1) {
                // Previous direction was down, filter to only rooms containing up
                filteredRooms = filteredRooms.Where(name => name.Contains("Up")).ToList();
                prevRoomDir = 0;
            } else if(previousRoom.xPosition == currentRoom.xPosition - 1 && previousRoom.yPosition == currentRoom.yPosition) {
                // Previous direction was right, filter to only rooms containing left
                filteredRooms = filteredRooms.Where(name => name.Contains("Left")).ToList();
                prevRoomDir = 1;
            } else if(previousRoom.xPosition == currentRoom.xPosition && previousRoom.yPosition == currentRoom.yPosition - 1) {
                // Previous direction was up, filter to only rooms containing down
                filteredRooms = filteredRooms.Where(name => name.Contains("Down")).ToList();
                prevRoomDir = 2;
            } else if(previousRoom.xPosition == currentRoom.xPosition + 1 && previousRoom.yPosition == currentRoom.yPosition) {
                // Previous direction was left, filter to only rooms containing right
                filteredRooms = filteredRooms.Where(name => name.Contains("Right")).ToList();
                prevRoomDir = 3;
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
            newRoom = rooms[roomSelection];
            Instantiate(newRoom.room, new Vector3(currentRoom.xPosition * 25, currentRoom.yPosition * 27, 0), Quaternion.identity, roomsParent);
            PlacedRoom placedRoom = new PlacedRoom(currentRoom.xPosition, currentRoom.yPosition, newRoom.openings);

            placedRoom.openings[nextRoomDir] = false;
            placedRoom.openings[prevRoomDir] = false;
            
            placedRooms.Add(placedRoom);

            yield return null;
        }

        // Place boss room
        string bossRoomDirection = "Up";
        int amntPositionedRooms = positionedRooms.Count - 1;

        int bossRoomX = positionedRooms[amntPositionedRooms].xPosition;
        int bossRoomY = positionedRooms[amntPositionedRooms].yPosition;

        int bossRoomOffsetX = 0;
        int bossRoomOffsetY = 0;

        if(bossRoomY == positionedRooms[amntPositionedRooms - 1].yPosition - 1) {
            bossRoomDirection = "Up";
            bossRoomOffsetY = -15;
        } else if(bossRoomX == positionedRooms[amntPositionedRooms - 1].xPosition + 1) {
            bossRoomDirection = "Left";
            bossRoomOffsetX = 15;
        } else if(bossRoomY == positionedRooms[amntPositionedRooms - 1].yPosition + 1) {
            bossRoomDirection = "Down";
            bossRoomOffsetY = 15;
        } else if(bossRoomX == positionedRooms[amntPositionedRooms - 1].xPosition - 1) {
            bossRoomDirection = "Right";
            bossRoomOffsetX = -15;
        }
        newRoom = rooms["Boss-" + bossRoomDirection];
        Instantiate(newRoom.room, new Vector3((bossRoomX * 25) + bossRoomOffsetX, (bossRoomY * 27) + bossRoomOffsetY, 0), Quaternion.identity, roomsParent);

        // Check if boss room is going to be clipping into other rooms
        bool bossRoomObstructed = false;
        if(bossRoomDirection == "Up") {
            if(placedRooms.Exists(match => 
                (match.xPosition >= bossRoomX - 1 && match.xPosition <= bossRoomX + 1) &&
                (match.yPosition >= bossRoomY && match.yPosition <= bossRoomY + 2)
            )) {
                bossRoomObstructed = true;
                Generate();
            }
        } else if(bossRoomDirection == "Left") {
            if(placedRooms.Exists(match => 
                (match.xPosition >= bossRoomX && match.xPosition <= bossRoomX - 2) &&
                (match.yPosition >= bossRoomY - 1 && match.yPosition <= bossRoomY + 1)
            )) {
                bossRoomObstructed = true;
                Generate();
            }
        } else if(bossRoomDirection == "Down") {
            if(placedRooms.Exists(match => 
                (match.xPosition >= bossRoomX - 1 && match.xPosition <= bossRoomX + 1) &&
                (match.yPosition >= bossRoomY && match.yPosition <= bossRoomY - 2)
            )) {
                bossRoomObstructed = true;
                Generate();
            }
        } else if(bossRoomDirection == "Right") {
            if(placedRooms.Exists(match => 
                (match.xPosition >= bossRoomX && match.xPosition <= bossRoomX + 2) &&
                (match.yPosition >= bossRoomY - 1 && match.yPosition <= bossRoomY + 1)
            )) {
                bossRoomObstructed = true;
                Generate();
            }
        }

        if(!bossRoomObstructed) {
            decorateRooms();
        }

        yield return null;
    }


    void decorateRooms() {
        for(int i = 0; i < placedRooms.Count; i++) {

            // Get room we are trying to decorate
            if(GameObject.Find("World").transform.childCount < i - 1) {
                break;
            }

            GameObject room = GameObject.Find("World").transform.GetChild(i).gameObject;

            // Select a random decoration pattern
            int patternIndex = UnityEngine.Random.Range(0, roomDecorations.Length);
            GameObject decoration = roomDecorations[patternIndex];

            // Get tilemaps from room
            Tilemap roomFloorTiles = room.transform.Find("Floor/Floor").GetComponent<Tilemap>();
            Tilemap roomCeilingTiles = room.transform.Find("Ceiling/Ceiling").GetComponent<Tilemap>();
            Tilemap roomColliderTiles = room.transform.Find("Collider/Collider").GetComponent<Tilemap>();

            // Get tilemaps from decoration
            Tilemap decorFloorTiles = decoration.transform.Find("Floor/Floor").GetComponent<Tilemap>();
            Tilemap decorCeilingTiles = decoration.transform.Find("Ceiling/Ceiling").GetComponent<Tilemap>();
            Tilemap decorColliderTiles = decoration.transform.Find("Collider/Collider").GetComponent<Tilemap>();


            roomFloorTiles.CompressBounds();
            decorFloorTiles.CompressBounds();

            roomCeilingTiles.CompressBounds();
            decorCeilingTiles.CompressBounds();

            roomColliderTiles.CompressBounds();
            decorColliderTiles.CompressBounds();


            // Set floor tiles
            BoundsInt roomBounds = roomFloorTiles.cellBounds;
            TileBase[] roomTiles = roomFloorTiles.GetTilesBlock(roomBounds);

            BoundsInt decorBounds = decorFloorTiles.cellBounds;
            TileBase[] decorTiles = decorFloorTiles.GetTilesBlock(decorBounds);

            for(int x = 0; x < decorBounds.size.x; x++) {
                for(int y = 0; y < decorBounds.size.y; y++) {
                    TileBase newTile = decorTiles[x + (y * decorBounds.size.x)];

                    if(newTile != null) {
                        roomFloorTiles.SetTile(new Vector3Int(x - 12, y - 14, 0), newTile);
                    }
                }
            }

            // Set ceiling tiles
            roomBounds = roomCeilingTiles.cellBounds;
            roomTiles = roomCeilingTiles.GetTilesBlock(roomBounds);

            decorBounds = decorCeilingTiles.cellBounds;
            decorTiles = decorCeilingTiles.GetTilesBlock(decorBounds);

            for(int x = 0; x < decorBounds.size.x; x++) {
                for(int y = 0; y < decorBounds.size.y; y++) {
                    TileBase newTile = decorTiles[x + (y * decorBounds.size.x)];

                    if(newTile != null) {
                        roomCeilingTiles.SetTile(new Vector3Int(x - 12, y - 14, 0), newTile);
                    }
                }
            }

            // Special rule for 'pit' designs
            if(decoration.name.Contains("Pit")) {
                Instantiate(decoration.transform.Find("Collider").gameObject, new Vector3(placedRooms[i].xPosition * 25, placedRooms[i].yPosition * 27, 0), Quaternion.identity, roomsParent);
                continue;
            }

            // Set collider tiles
            roomBounds = roomColliderTiles.cellBounds;
            roomTiles = roomColliderTiles.GetTilesBlock(roomBounds);

            decorBounds = decorColliderTiles.cellBounds;
            decorTiles = decorColliderTiles.GetTilesBlock(decorBounds);

            for(int x = 0; x < decorBounds.size.x; x++) {
                for(int y = 0; y < decorBounds.size.y; y++) {
                    TileBase newTile = decorTiles[x + (y * decorBounds.size.x)];

                    if(newTile != null) {
                        roomColliderTiles.SetTile(new Vector3Int(x - 12, y - 14, 0), newTile);
                    }
                }
            }

        }
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

        /*
        // Place first room
            positionedRooms.Add(new PositionedRoom(newX, newY));

            int initialCount = positionedRooms.Count;
            bool continuePlacing = true;
            int i;
            for(i = 0; i < numRooms && continuePlacing; i++) {

                PositionedRoom currentRoom = positionedRooms[initialCount + i - 1];
                
                // Choose next direction
                bool choosingDirection = true;
                int nextDirection = 0;
                do {
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

                    Debug.Log("Choosing at " + newX + ", " + newY);

                    choosingDirection = false;

                    // Make sure the picked direction is not an already existing room
                    if(positionedRooms.Exists(match => match.xPosition == newX && match.yPosition == newY)) {
                        choosingDirection = true;

                        // Check if a room can be positioned
                        if(positionedRooms.Exists(match => match.xPosition == currentRoom.xPosition && match.yPosition == currentRoom.yPosition + 1) &&
                        positionedRooms.Exists(match => match.xPosition == currentRoom.xPosition - 1 && match.yPosition == currentRoom.yPosition) &&
                        positionedRooms.Exists(match => match.xPosition == currentRoom.xPosition && match.yPosition == currentRoom.yPosition - 1) &&
                        positionedRooms.Exists(match => match.xPosition == currentRoom.xPosition + 1 && match.yPosition == currentRoom.yPosition)) {
                            choosingDirection = false;
                            continuePlacing = false;
                        }
                    }

                    yield return null;
                } while(choosingDirection);

                if(continuePlacing) {
                    positionedRooms.Add(new PositionedRoom(newX, newY));
                }

                yield return null;
            }

            for(int j = 0; j < i; j++) {

                PositionedRoom currentRoom = positionedRooms[initialCount + j - 1];

                newRoom = rooms["Up-Left-Down-Right"];
                Instantiate(newRoom.room, new Vector3(currentRoom.xPosition * 25, currentRoom.yPosition * 27, 0), Quaternion.identity, roomsParent);
                PlacedRoom placedRoom = new PlacedRoom(currentRoom.xPosition, currentRoom.yPosition, new[] {false, false, false, false});

                placedRooms.Add(placedRoom);
            }

            roomsWithOpenings = roomsWithOpenings.Where(room => room.hasOpenings()).ToList();
            */


}
