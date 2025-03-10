﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using Unity.XR.OpenVR;
using UnityEngine.XR;
using JetBrains.Annotations;

// class that controls the forest fire cellular automaton
public class ForestFire3D : MonoBehaviour
{
    public int gridSizeX; // x size of the grid
    public int gridSizeY; // y size of the grid
    public int nlight; // the number of trees to set alight at the start of the game
    public int xC, yC; // used for picking random x, y points
    public int dogX, dogY; // x and y coordinates of the dog

    public int rockChance; // the percentage chance a cell is assigned as rock
    public int grassChance; // the percentage chance a cell is assigned as grass

    public GameObject cellPrefab; // gameobject prefab used to represent a cell on the grid

    

    public ForestFireCell[,] forestFireCells = new ForestFireCell[0, 0]; // array of ForestFireCell objects
    public ForestFireCell.State[,] forestFireCellsNextGenStates = new ForestFireCell.State[0, 0]; // array of cell states to be used in the next generation of the game 

    public GameObject[,] cellGameObjects = new GameObject[0, 0]; // an array to hold references to each gameobject that make up grid
    public bool gameRunning = false; // bool controlling whether the game is currently running

    /*
     * Declaration of my variables
     */
    public int closestXtoDog, closestYtoDog; // used to get the closest cell to the dog
    public int closestXtoPlayer, closestYtoPlayer; // used to get the closest cell to the player
    public Transform nozzle; // a tiny object that keeps track of the player's position
    public Dog dogPrefab; // reference to dog prefab to instantiate it on scene
    private Dog dogInstance; // Dog game object

    public GameObject pauseCanvas; // canvas that holds the pause menu
    public InputActionReference pauseGame; // control that pauses the game
    int levelValue = MainMenu.difficultyLevel; // reference from a static variable in MainMenu script
    /*
     
     */

    [Range(0.01f, 3f)]
    public float updateRate; // used to define how often will the game update (in seconds)
    private float _gameTimer; // a variable that will be used detect when the game should update 

    private Camera gameCamera; // the camera that is players viewport

    // Awake is a built-in Unity function that is only called once, before the Start function
    private void Awake()
    {
        // find the camera in the scene and store it for later
        gameCamera = FindObjectOfType<Camera>();
    }

    // Start is a built-in Unity function that is called before the first frame update
    private void Start()
    {
        CreateGrid(gridSizeX, gridSizeY);
        RandomiseGrid();
        PauseGame(true);
        UpdateGridVisuals();
    }

    // this function controls whether or not to pause the game
    private void PauseGame(bool setGamePause)
    {
        /*
         * if game setGamePause is true
         * - display the pause canvas
         * - pause the timer
         * - stop the dog's barking
         */

        if (setGamePause)
        {
            gameRunning = false;
            pauseCanvas.SetActive(true);
            GameTimer.isPaused = true;
            dogInstance.StopBarking();
        }
        /*
         * else if setGamePause is false
         * - set gameRunning to true
         * - hide the pause canvas
         * - hide the resume the game timer
         * - resume the dog's barking
         */
        else 
        {
            gameRunning = true;
            pauseCanvas.SetActive(false);
            GameTimer.isPaused = false;
            dogInstance.Bark();
        }
    }

    // Update is a built-in Unity function that is called once per frame 
    private void Update()
    {
        // check if the Y key on controller has been pressed. this key will toggle between whether the game is currently running or paused
        if (pauseGame.action.WasPressedThisFrame())
        {
            // if the gameRunning is true, pause the game
            if (gameRunning)
            {
                PauseGame(true);
            }
            else // if the gameRunning is false, unpause the game
            {
                PauseGame(false);
                dogInstance.Bark();
            }
        }

        // check if the R key has been pressed. this key will clear the grid and pause the game
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RandomiseGrid();
        }

        // update the visual state of each cell
        UpdateGridVisuals();

        // Check if shooting has been pressed


        // if the game is not running, return here to prevent the rest of the code in this Update function from running    
        if (gameRunning == false)
            return;

        if (_gameTimer < updateRate)
        {
            _gameTimer += Time.deltaTime;
        }
        else
        {
            UpdateCells();
            _gameTimer = 0f;
        }
        // run the two functions to find the closest cell to the player and the dog, in order to update their location on the minimap
        FindClosestCellToDog();
        FindClosestCellToPlayer();
        
        


    }

    private void RandomiseGrid()
    {
        // set the difficulty level by altering the number of cells that are initially alight
        if (levelValue == 1)
        {
            nlight = 4; // how many trees to set on fire
        } else
        {
            nlight = 2;
        }
        
         // iterate through every cell in the cell in the grid and set its state to dead, decide what type of object is present and if flammable assign an amount of fuel

        for (int xCount = 0; xCount < gridSizeX; xCount++)
        {
            for (int yCount = 0; yCount < gridSizeY; yCount++)
            {
                xC = UnityEngine.Random.Range(0, 100); // generate a random number between 0 and 100

                if (xC < rockChance) // if the random value is less than rock chance, assign cell as rock
                {
                    forestFireCells[xCount, yCount].SetRock();
                }
                else if (xC < grassChance) // if the random value is less than grass chance, assign cell as grass and set cell fuel
                {
                    forestFireCells[xCount, yCount].SetGrass();
                    forestFireCells[xCount, yCount].cellFuel = UnityEngine.Random.Range(1, 5);
                }
                else // if the random value is higher than rock and grass chance, assign as tree and set cell fuel
                {
                    forestFireCells[xCount, yCount].SetTree();
                    forestFireCells[xCount, yCount].cellFuel = UnityEngine.Random.Range(15, 25);
                }
            }
        }

        do
        {
            xC = UnityEngine.Random.Range(0, gridSizeX);// now pick some trees at random and set them alight
            yC = UnityEngine.Random.Range(0, gridSizeY);

            if (forestFireCells[xC, yC].cellState == ForestFireCell.State.Tree)
            {
                forestFireCells[xC, yC].SetAlight();

                nlight = nlight - 1;     // decrease number of trees to light by 1
            }
        } while (nlight > 0);  // when you've lit them all exit this loop

        // set the middle cell as grass which is where the player is placed
        forestFireCells[20, 20].SetGrass();

        // get a random x and y value and instantiate the dog's position
        dogX = UnityEngine.Random.Range(0, gridSizeX);
        dogY = UnityEngine.Random.Range(0, gridSizeY);

        // make sure the dog's location always has grass
        forestFireCells[dogX, dogY].SetGrass();
        dogInstance = Instantiate(dogPrefab, null);

        dogInstance.transform.position = cellGameObjects[dogX, dogY].transform.position;


    }


    // update the status of each cell on grid according to the rules of the game
    private void UpdateCells()
    {
        // iterate through each cell in the rows and columns
        for (int xCount = 0; xCount < gridSizeX; xCount++)
        {
            for (int yCount = 0; yCount < gridSizeY; yCount++)
            {
                // find out the number of alight neighbours this cell has
                int alightNeighbourCells = CountAlightNeighbourCells(xCount, yCount);

                if (forestFireCells[xCount, yCount].cellState == ForestFireCell.State.Alight) // if the cell is currently alight let it burn but reduce it's fuel and see if it goes out
                {
                    forestFireCells[xCount, yCount].cellFuel--; // reduce fuel by 1 (-- operator reduces an integer by 1)


                    if (forestFireCells[xCount, yCount].cellFuel <= 0) // has it burned all its fuel?
                    {
                        // cell has no fuel so is burned out 
                        forestFireCellsNextGenStates[xCount, yCount] = ForestFireCell.State.Burnt;
                    }
                    else // cell still has fuel so carries on burning
                    {
                        forestFireCellsNextGenStates[xCount, yCount] = forestFireCells[xCount, yCount].cellState;
                    }
                }
                else if (forestFireCells[xCount, yCount].cellFuel > 0)  // the cell has fuel but is not alight yet
                {
                    // A dead cell with an alight neighbour which has fuel has a probability of becoming an alight cell
                    if (alightNeighbourCells > 0)
                    {
                        xC = UnityEngine.Random.Range(0, 100); // generate a random number between 0 and 100

                        if (xC < 10 * alightNeighbourCells) // the more alight neighbours the greater the probability of catching light
                                                            // e.g. 1 alight neighbour = 10 * 1 = 10% chance of catching fire, 2 alight neighbours = 10 * 2 = 20% chance of catching fire, etc.
                        {
                            forestFireCellsNextGenStates[xCount, yCount] = ForestFireCell.State.Alight;  // a 10% chance of catching fire
                        }
                    }
                    else // no neighbours on fire. use the current state of the cell for the next generation of the game.
                    {
                        forestFireCellsNextGenStates[xCount, yCount] = forestFireCells[xCount, yCount].cellState;
                    }
                }
                else // this cell has 0 fuel, so has either burnt out or is a cell which cant be burnt (rock), use the current state of the cell for the next generation of the game.
                {
                    forestFireCellsNextGenStates[xCount, yCount] = forestFireCells[xCount, yCount].cellState;
                }
            }
        }

        // now the state of each cell has been calculated, apply the results by setting the current game array values to that of the next generation
        for (int xCount = 0; xCount < gridSizeX; xCount++)
        {
            for (int yCount = 0; yCount < gridSizeY; yCount++)
            {
                forestFireCells[xCount, yCount].cellState = forestFireCellsNextGenStates[xCount, yCount];
            }
        }
    }

    // count the alight cells surrounding a specified cell on the grid 
    private int CountAlightNeighbourCells(int cellPositionX, int cellPositionY)
    {
        // create local variable to keep track of alight neighbour cells
        int alightNeighbourCells = 0;

        // the code below tries to iterate through the neighbour cells immediately surrounding the specified cell on the grid as well as the specified cell
        //
        // On the grid the it would like this
        //
        //  N N N
        //  N C N
        //  N N N
        //
        // N = neighbour C = cell that's neighbours are being counted

        for (int xPosition = cellPositionX - 1; xPosition < cellPositionX + 2; xPosition++)
        {
            for (int yPosition = cellPositionY - 1; yPosition < cellPositionY + 2; yPosition++)
            {
                // only continue if the neighbour is valid
                if (IsNeighbourValid(xPosition, yPosition))
                {
                    // if the cell is currently alight (1), increase the count of alight neighbours by one
                    if (forestFireCells[xPosition, yPosition].cellState == ForestFireCell.State.Alight)
                    {
                        alightNeighbourCells++;

                        // we don't want to check if the specified cell is alight, only its neighbours so it was added, subtract it
                        if (xPosition == cellPositionX && yPosition == cellPositionY)
                        {
                            alightNeighbourCells--;
                        }
                    }
                }
            }
        }

        // return the number of alight neighbour cells
        return alightNeighbourCells;
    }

    // make sure that the cell we are trying to count is not beyond the range of the game grid (edges of the game board)
    private bool IsNeighbourValid(int cellPositionX, int cellPositionY)
    {
        if (cellPositionX < 0 || cellPositionY < 0)
            return false;

        if (cellPositionX >= gridSizeX || cellPositionY >= gridSizeY)
            return false;

        return true;
    }

    // this function creates the grid of the game
    private void CreateGrid(int sizeX, int sizeY)
    {
        // initialise the game array and array containing their states for the next generation to the size of grid
        forestFireCells = new ForestFireCell[sizeX, sizeY];
        forestFireCellsNextGenStates = new ForestFireCell.State[sizeX, sizeY];

        // initialise the array of gameobjects that will hold the game objects on the grid
        cellGameObjects = new GameObject[sizeX, sizeY];

        int xSpacing = 0;
        int ySpacing = 0;

        for (int xCount = 0; xCount < sizeX; xCount++)
        {
            for (int yCount = 0; yCount < sizeY; yCount++)
            {
                // create cell object and name it according to its position (3 adds extra space between cells)
                GameObject newCell = Instantiate(cellPrefab, new Vector3(xCount, 0, yCount), Quaternion.identity);
                newCell.name = "cell " + xCount + " " + yCount;

                //position the cell on the grid, spacing them out using the x and y count as coordinates 
                newCell.transform.position = new Vector3(xCount + xSpacing, 0, yCount + ySpacing);

                // add the gameobject of the cell to the array that stores references of the cell gameObjects
                cellGameObjects[xCount, yCount] = newCell;

                // add to array
                ForestFireCell forestFireCell = newCell.GetComponent<ForestFireCell>();
                forestFireCells[xCount, yCount] = forestFireCell;

                // add reference to camera
                forestFireCell.playerCamera = gameCamera.gameObject;

                ySpacing += 3;
            }

            ySpacing = 0;
            xSpacing += 3;
        }
    }

    // udpate the grid cells according to their current state
    // this function will be called every frame of the game so the grid is always up to date 
    private void UpdateGridVisuals()
    {
        // iterate through each cell in the rows and columns
        for (int xCount = 0; xCount < gridSizeX; xCount++)
        {
            // check current state of cell an update visual
            for (int yCount = 0; yCount < gridSizeY; yCount++)
            {
                if (forestFireCells[xCount, yCount].cellState == ForestFireCell.State.Alight)
                {
                    forestFireCells[xCount, yCount].SetAlight();
                }
                else if (forestFireCells[xCount, yCount].cellState == ForestFireCell.State.Rock)
                {
                    forestFireCells[xCount, yCount].SetRock();
                }
                else if (forestFireCells[xCount, yCount].cellState != ForestFireCell.State.Rock && forestFireCells[xCount, yCount].cellFuel <= 0)// it's not a rock but it's fuel is zero, therefore it must be burnt out grass or tree
                {
                    forestFireCells[xCount, yCount].SetBurnt();
                }
                else if (forestFireCells[xCount, yCount].cellState == ForestFireCell.State.Grass)
                {
                    forestFireCells[xCount, yCount].SetGrass();
                }
                else if (forestFireCells[xCount, yCount].cellState == ForestFireCell.State.Tree)
                {
                    forestFireCells[xCount, yCount].SetTree();
                }
            }
        }
    }


// function that finds the closest cell to the dog
    private void FindClosestCellToDog()
    {
        // start with a very high distance value
        float distance = 1000f;


        for (int xCount = 0; xCount < gridSizeX; xCount++)
        {
            for (int yCount = 0; yCount < gridSizeY; yCount++)
            {
                // reduce the distance value as the loop goes on, the least distance is the closest cell to the dog
                if (Vector3.Distance(dogInstance.transform.position, cellGameObjects[xCount, yCount].transform.position) < distance)
                {
                    closestXtoDog = xCount;
                    closestYtoDog = yCount;
                    distance = Vector3.Distance(dogInstance.transform.position, cellGameObjects[xCount, yCount].transform.position);
                }
            }

        }
    }

    // // function that finds the closest cell to the player
    private void FindClosestCellToPlayer()
    {
        float distance = 1000f;


        for (int xCount = 0; xCount < gridSizeX; xCount++)
        {
            for (int yCount = 0; yCount < gridSizeY; yCount++)
            {

                if (Vector3.Distance(nozzle.transform.position, cellGameObjects[xCount, yCount].transform.position) < distance)
                {
                    closestXtoPlayer = xCount;
                    closestYtoPlayer = yCount;
                    distance = Vector3.Distance(nozzle.transform.position, cellGameObjects[xCount, yCount].transform.position);
                }
            }

        }
    }


}