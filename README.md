# Vislab XR Tutorial

In this tutorial, we will briefly work through the basic XR development using Unity3D, XR Interaction Toolkit, and AR foundation.

## Task

We will implement a mobile AR treasure hunting game. The goal of the game is to find a hidden treasure in the reality.

## Prerequiste

1. computer with Unity version 2021.3.13 lts (macOS must be used to build app for iOS) and empty space > 2gb
    1. [Unity Hub](https://unity3d.com/get-unity/download)
    2. Make sure the build tool for Android or iOS is installed
2. A mobile device (tablet or phone) with > 1gb empty space (iOS devices are more prefered)
3. usb that can connect your mobile devices and computer
4. code editor (visual studio is preferred, could be installed together with Unity)
5. web browser to download the skeleton code

## Task -1: Create a new project using Unity Hub

The first step is to creat a new project using Unity Hub. Fortunately, we have already prepared this template code with the necessary package installed (i.e., XR Interaction Toolkit and AR foundation). You may just simply download or clone this repository to your local computer. Then open the downloaded or cloned folder using Unity Hub.

## Task 0: Setup a scene in Unity

Scenes are the containers of the virtual worlds. Objects are included inside a scene. Therefore, the thing to begin with is to setup the scene. We are going to setup walls based on the seating plan of the UC 101.

1. Create a new scene by clicking `File` -> `New Scene`
2. Drag the UC floor plan image into the scene from the `Assets` folder
3. Set the scale of the floor plan to `x: 1.5 y: 1.5 z: 1.5`
4. Set the rotation of X to `90`

Now, we have a floor plan in the scene. Next, we are going to setup the walls.

1. Locate the seats 5 and 6 in the floor plan. We should be able to find that the size of the seats are 1340 and 2785. We will use these numbers to setup the walls.
2. Create a new `Cube` object by clicking `GameObject` -> `3D Object` -> `Cube`
3. In XR development, the convenational unit is `meter`. Therefore, we need to convert the pixel size to meter size. We can use the following formula to convert the pixel size to meter size.
4. Resize the cube to `x: 0.03 y: 1.34 z: 1.34`.
5. Repeat the step 2 and 3 to create the remaining cubes, until the seats 5 and 6 are covered.
6. Move the cubes to the correct position. You may need to adjust the scale of the cubes to fit the seats.

Since the time is limited, we will not setup the remaining walls. However, you may try to setup the remaining walls by yourself. We have already provided the scene with the walls setup. You may now open the scene `Part1` from the `Assets\Scenes` folder.

Note that we have added some `Out Zone` objects in the scene. These objects are used to prevent the player from moving outside the room. You may need to adjust the scale and position of these objects to fit the room.

## Task 1: Create the treasure

The first thing to do is to create the treasure. We will create a treasure using a 3D model. We have already provided a 3D model of the treasure in the `Assets\3D Models` folder.

1. Drag the treasure model into the scene
2. Move the treasure to a suitable position (just pick your favorite seat)
3. Make sure the treasure is hidden inside a seat

Now you should be able to see the treasure in the scene. You may refer to the `Part2` scene in the `Assets\Scenes` folder.

## Task 2: Randomize spwaning the treasure to a seat

To increase the difficulty of the game, we will randomize the spawning of the treasure. We will use the `Random` class to generate a random number. The random number will be used to select a seat to spawn the treasure.

1. Create a new `C# Script` by clicking `Assets` -> `Create` -> `C# Script`
2. Rename the script to `TreasureSpawner`
3. Open the script using the code editor
4. Add the following code to the script

    ```csharp
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TreasureSpawner : MonoBehaviour
    {
        public GameObject treasure;
        public GameObject[] seats;

        // Start is called before the first frame update
        void Start()
        {
            int index = Random.Range(0, seats.Length);
            GameObject seat = seats[index];
            Vector3 position = seat.transform.position;
            position.y += 0.1f;
            Quaternion quaternion = Quaternion.Euler(new Vector3(-90, 0, 0));
            Instantiate(treasure, position, quaternion);
        }
    }
    ```

5. Create a new `Empty` object by clicking `GameObject` -> `Create Empty`
6. Rename the object to `TreasureSpawner`
7. Attach the script to the `TreasureSpawner` object in the scene
8. Drag the treasure object from `Assets\3D Models` into the `treasure` field in the inspector
9. Delete the `treasure_chest` object in the scene
10. Create a new `Empty` object by clicking `GameObject` -> `Create Empty`
11. Rename the object to `Seat`
12. Repeat steps 9 and 10 to create all seats
13. Drag all seats into the `seats` field in the inspector

Now, you should be able to see the treasure is randomly spawned to a seat when you press the play button in the Unity editor. You may refer to the `Part3` scene in the `Assets\Scenes` folder.

## Task 3: Create the AR player

The next thing to do is to create the AR player. We will use the XR Interaction Toolkit to create the AR player. The XR Interaction Toolkit is a package that provides a set of tools to create XR applications. The XR Interaction Toolkit provides a set of prefabs that can be used to create the AR player.

1. Create a new `AR Session` and `AR Session Origin` from `GameObject` -> `XR`
2. Position the `AR Session Origin` to any position in the scene. It should be the starting position of the player when they open the app

## Task 4: Interact with the treasure

Finally, we are going to implement the interaction with the treasure. We will use the XR Interaction Toolkit to implement the interaction. The XR Interaction Toolkit provides a set of prefabs that can be used to create the interaction.

1. Attach `AR Gesture Interactor` to the `AR Session Origin` object
2. Drag the 3D model of the treasure into the scene
3. Attach `AR Selection Interactable` to the treasure
4. Drag the treasure object from the scene to the Asset folder
5. Update the treasure field in the `TreasureSpawner` script to the treasure object in the Asset folder
6. Update the following code to the `TreasureSpawner` script

    ```csharp
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TreasureSpawner : MonoBehaviour
    {
        public GameObject treasure;
        public GameObject[] seats;

        private GameObject spawnedTreasure;
        private int spawnedIndex = -1;

        // Start is called before the first frame update
        void Start() {
            RespwanTreasure(default(SelectEnterEventArgs));
        }

        public void RespwanTreasure(SelectEnterEventArgs arg0) {
            if (spawnedTreasure != null) {
                Destroy(spawnedTreasure);
            }

            int index = Random.Range(0, seats.Length);
            while (spawnedIndex == index) {
                index = Random.Range(0, seats.Length);
            }
            GameObject seat = seats[index];
            spawnedIndex = index;
            Vector3 position = seat.transform.position;
            position.y += 0.1f;
            Quaternion quaternion = Quaternion.Euler(new Vector3(-90, 0, 0));
            spawnedTreasure = Instantiate(treasure, position, quaternion);
            spawnedTreasure.GetComponent<ARSelectionInteractable>().selectEntered.AddListener(RespwanTreasure);
        }
    }
    ```

7. Update the `Interactable Events` in the inspector of the treasure
8. Drag the `TreasureSpawner` object into the `On Select Entered` field
9. Select the `Respwan Treasure` method in the `TreasureSpawner` object

Now, you should be able to see the treasure is respawned to a new seat when you select the treasure. You may refer to the `Part4` scene in the `Assets\Scenes` folder.

## Task 5: Build the app

Lastly, we are going to build the app.

1. Click `File` -> `Build Settings`
    <details>
    <summary>Android</summary>

    1. Click `Switch Platform` and select `Android`
    2. Select `Player Setting`
    3. Uncheck `Auto Graphics API`
    4. Remove `Vulkan` from the `Graphics APIs` list (if it is there)
    5. Change the `Minimum API Level` to `Android 7.0 Nougat (API Level 24)`
    6. Change the `Scripting Backend` to `IL2CPP`
    7. Add `ARM64` to the `Target Architectures` list
    </details>

    <details><summary>iOS</summary>

    1. Click `Switch Platform` and select `iOS`
    2. Select `Player Setting`
    3. Add a message to the `Camera Usage Description` field (e.g. `This app requires camera access to scan the treasure`)

    </details>
2. Click `XR Plugin Management`
3. Click `Android` => `ARCore` or `iOS` => `ARKit`
4. Click `Build And Run`

## Task 6: Enhance the app

Now, you have completed the tutorial. You can try to enhance the app by adding more features. For example, you can add a timer to the game. You can also add a score system to the game. You can also add a leaderboard to the game.

For AR, you can try to make the wall transparent. You can also try to add some hints to the game. You can also try to add some sound effects to the game.

For AR interaction, you can try to add more interaction to the world. For example, you can add a button to the world to open the treasure. You can also add a button to the world to respawn the treasure.

## Conclusion

In this tutorial, you have learned how to create an AR treasure hunt game using Unity and AR Foundation. You have also learned how to use the XR Interaction Toolkit to create the AR player and the interaction with the treasure. You have also learned how to build the app for Android and iOS.
