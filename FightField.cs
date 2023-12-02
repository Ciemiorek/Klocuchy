using Godot;
using System;
using System.Collections.Generic;


public partial class FightField : Node3D
{


    double deltaPassed = 0;
    double delta;

    //Logic of game


    private bool doPlayerChoosToMove = true;
    private bool doPlayerChoosToAttac = true;
    private bool isPlayerTurn = true;
    private bool isTimeToSelectPawn = true;
    //-1 none of them active, 0-6 active one of them
    private int activePown = -1;


    private Pawn[] pawns = new Pawn[6];
    private BattleFieldBlock[][] battleFieldBlocks = new BattleFieldBlock[8][];

    Node3D pawnsParent;
    Control interfaceee;
    List<BattleFieldBlock> pathSelectetByPlayer = new List<BattleFieldBlock>();
    int temp = 0;




    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        interfaceee = GetNode<Control>("Interface");
        pawnsParent = GetNode<Node3D>("pawns");

        initializationOfFieldBlockMatrix();

        spawnPawn(0, 0, 0, new Color(1, 0, 0));
        spawnPawn(0, 2, 1, new Color(1, 0, 0));
        spawnPawn(0, 4, 2, new Color(1, 0, 0));
        spawnPawn(7, 5, 3, new Color(1, 0, 0));
        spawnPawn(7, 3, 4, new Color(1, 0, 0));
        spawnPawn(7, 1, 5, new Color(1, 0, 0));

        for (int i = 0; i < 6; i++)
        {
            pawns[i].attacks[0] = AttacksRepo.hit;
            pawns[i].attacks[1] = AttacksRepo.acurateHit;
            pawns[i].attacks[2] = AttacksRepo.GretestOfAllHit;
            pawns[i].attacks[3] = AttacksRepo.SuperHIt;
        }




        interfaceee.Call("setName", "00", 0);
        interfaceee.Call("setName", "01", 1);
        interfaceee.Call("setName", "02", 2);
        interfaceee.Call("setName", "03", 3);
        interfaceee.Call("setName", "04", 4);
        interfaceee.Call("setName", "05", 5);
        for (int i = 0; i < 6; i++)
        {
            interfaceee.Call("setMaxHP", 100, i);
            interfaceee.Call("setCurrentHp", 33, i);

        }
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        deltaPassed += delta;

        this.delta = delta;

       


    }

   

    //!!!!!!!!!!!!!!!EVENTS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public void pawnsEvents(Camera3D camera, InputEvent eventt, Vector3 position, Vector3 normal, int shape_idx, int numberPawn)
    {
        if (eventt is InputEventMouseButton e)
        {

            if (e.ButtonIndex.Equals(MouseButton.Left) && eventt.IsPressed() && isTimeToSelectPawn)
            {
                if (activePown >= 0)
                {
                    pawns[activePown].Call("setNotActive");
                }
                fieldLightOFF();
                setPawnActive(numberPawn);
                GD.Print("Ustawiony aktywny");

                GD.Print(numberPawn);
            }
            else
            {
                GD.Print("inne przyciski");
            }


        }

    }
    public void blocksEvents(Camera3D camera, InputEvent eventt, Vector3 position, Vector3 normal, int shape_idx, int row, int block) 
    {
        if (eventt is InputEventMouseButton e)
        {

            if (e.ButtonIndex.Equals(MouseButton.Left) && eventt.IsPressed())
            {

                if (activePown >= 0)
                {


                    if (isPlayerTurn && doPlayerChoosToMove)
                    {
                        BattleFieldBlock startingBlock = battleFieldBlocks[pawns[activePown].row][pawns[activePown].block];
                        BattleFieldBlock destinBlock = pathSelectetByPlayer[pathSelectetByPlayer.Count - 1]; 
                        if(destinBlock.row == row && destinBlock.block==block && pathSelectetByPlayer.Count!=1){
                            
                            
                            startingBlock.isOcupate = false;
                            startingBlock.pawnOnBlock = -1;

                        


                            putPawnOnBlockWithAnimaton(activePown, row, block, pathSelectetByPlayer);


                            clearPathMarks(pathSelectetByPlayer);





                            fieldLightOFF();
                           
                            pawns[activePown].setNotActive();
                            activePown = -1;
                            pathSelectetByPlayer.Clear();
                        }
                        else
                        {
                            setNewPath(row, block);

                        }

                    }
                }

                GD.Print("lewy w block " + row + block);

            }
            else if (e.ButtonIndex.Equals(MouseButton.Right) && eventt.IsPressed())
            {
                List<BattleFieldBlock> path = pathFindAStar(pawns[activePown].row, pawns[activePown].block, row, block);
                if (path.Count <= pawns[activePown].moveRange + 1)
                {
                    foreach (BattleFieldBlock blockTofire in path)
                    {
                        blockTofire.changeLightColor("RED");
                    }
                }

            }
            else
            {
                GD.Print("inne przyciski w block");
            }


        }

    }

   

    public void buttonAttacksEvents(String buttonName)
    {

        GD.Print("Przycisk z atakiem");
        GD.Print(buttonName);
    }
    public void buttonEvents(String buttonName)
    {

        interfaceee.Call("setMaxHP", 100, 0);
        interfaceee.Call("setCurrentHp", 33, 0);
        GD.Print("Przycisk z celem");
        GD.Print(buttonName);
        if (buttonName == "Attack" && activePown >= 0)
        {
            SetAttactsToButtons(activePown);
            interfaceee.Call("changeVisibilityAttacksButtons", true);
            interfaceee.Call("changeVisibilityButtons", false);
        }

    }

    private void signalBlockMouseEntered(int row, int block)
    {
        pathPlayerMaker(row, block);

    }

    private void signalBlockMouseExited(int row, int block)
    {
        
    }

    //!!!!!!!!!!!!!!!EVENTS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    private void spawnPawn(int row, int block, int numberPawn, Color color)
    {
        var s = GD.Load<PackedScene>("res://Pawn.tscn");
        Node3D instance = s.Instantiate() as Node3D;

        instance.Call("createPawn", numberPawn, color);
        pawnsParent.AddChild(instance);
        pawns[numberPawn] = instance as Pawn;
        pawns[numberPawn].fightingfield = this;
        putPawnOnBlock(numberPawn, row, block);
    }



    private void SetAttactsToButtons(int activePown)
    {
        if (activePown >= 0)
        {
            for (int i = 0; i < 4; i++)
            {
                interfaceee.Call("setAttack", pawns[activePown].attacks);
            }


        }

    }

    private bool canMoveThere(int pawn, int rowGo, int blockGo)
    {
        
        Pawn p = pawns[pawn];
        int move = p.moveRange;
        int abs = Math.Abs(p.row - rowGo) + Math.Abs(p.block - blockGo);

        if (abs > move || abs == 0 || battleFieldBlocks[rowGo][blockGo].isOcupate)
        {
            return false;
        }
        else
        {
            return true;
        }

    }



    private void setPawnActive(int pawn)
    {
        clearPathMarks(pathSelectetByPlayer);
        pathSelectetByPlayer.Clear();
        activePown = pawn;
        pathSelectetByPlayer.Add(battleFieldBlocks[pawns[pawn].row][pawns[pawn].block]);
        showWhereCanGO(pawn);
        pawns[pawn].Call("setActive");
       
    }


    //pokaz zasieg
    private void showWhereCanGO(int pawn)
    {
       

        int steps = pawns[pawn].moveRange+1; //Pwans steps in future
        int rowPawn = pawns[pawn].row;
        int blockPawn = pawns[pawn].block;
        int[][] visited = new int[battleFieldBlocks.Length][];
        for (int i = 0; i < battleFieldBlocks.Length; i++)
        {
            visited[i] = new int[battleFieldBlocks[i].Length];
            for (int j = 0; j < battleFieldBlocks[i].Length; j++)
            {
                visited[i][j] = 0;
            }

        }

        showWhereCangoRecus(steps, rowPawn, blockPawn, visited);

    }
    private bool showWhereCangoRecus(int steps, int rowGo, int blockGo, int[][] maxStepsMap)
    {

        if (steps == 0) { return false; }
        else
        {
            maxStepsMap[rowGo][blockGo] = steps;




            battleFieldBlocks[rowGo][blockGo].Call("changeLightColor", "GREEN");


            if ((rowGo - 1) >= 0 && !battleFieldBlocks[rowGo - 1][blockGo].isOcupate && maxStepsMap[rowGo - 1][blockGo] < steps - 1)
            {
                showWhereCangoRecus(steps - 1, rowGo - 1, blockGo, maxStepsMap);
            }



            if ((rowGo + 1) <= 7 && !battleFieldBlocks[rowGo + 1][blockGo].isOcupate && maxStepsMap[rowGo + 1][blockGo] < steps - 1)
            {
                showWhereCangoRecus(steps - 1, rowGo + 1, blockGo, maxStepsMap);
            }



            if ((blockGo - 1) >= 0 && !battleFieldBlocks[rowGo][blockGo - 1].isOcupate && maxStepsMap[rowGo][blockGo - 1] < steps - 1)
            {
                showWhereCangoRecus(steps - 1, rowGo, blockGo - 1, maxStepsMap);
            }



            if ((blockGo + 1) <= 5 && !battleFieldBlocks[rowGo][blockGo + 1].isOcupate && maxStepsMap[rowGo][blockGo + 1] < steps - 1)
            {
                showWhereCangoRecus(steps - 1, rowGo, blockGo + 1, maxStepsMap);
            }
            return true;




        }

    }



    private List<BattleFieldBlock> pathFindAStar(int curentpositionRow, int curentpositionBlock, int rowGo, int blockGo)
    {
        List<BattleFieldBlock> cloeseList = new List<BattleFieldBlock>();
        List<BattleFieldBlock> openList = new List<BattleFieldBlock>();
        BattleFieldBlock startingBlock = battleFieldBlocks[curentpositionRow][curentpositionBlock];
        openList.Add(startingBlock);

        for (int i = 0; i < battleFieldBlocks.Length; i++)
        {
            for (int j = 0; j < battleFieldBlocks[i].Length; j++)
            {
                BattleFieldBlock block = battleFieldBlocks[i][j];
                block.gCost = int.MaxValue;
                block.calculateFCost();
                block.comeFrom = null;
            }
        }
        startingBlock.gCost = 0;
        startingBlock.hCost = Math.Abs(curentpositionRow - rowGo) + Math.Abs(curentpositionBlock - blockGo);
        startingBlock.calculateFCost();
        while (openList.Count > 0)
        {
            BattleFieldBlock currentBlockck = getLowestCostBlock(openList);
            if (currentBlockck.row == rowGo && currentBlockck.block == blockGo)
            {
                return calculateEndPath(rowGo, blockGo);
            }
            openList.Remove(currentBlockck);
            cloeseList.Add(currentBlockck);

            foreach (BattleFieldBlock block in getNeighboursList(currentBlockck))
            {
                if (!cloeseList.Contains(block))
                {
                    int temtativeGCost = currentBlockck.gCost + 1;
                    if (temtativeGCost < block.gCost)
                    {
                        block.comeFrom = currentBlockck;
                        block.gCost = temtativeGCost;
                        block.hCost = Math.Abs(block.row - rowGo) + Math.Abs(block.block - blockGo);
                        block.calculateFCost();
                        if (!openList.Contains(block))
                        {
                            openList.Add(block);
                        }

                    }
                }
            }

        }

        return openList;

    }

    private List<BattleFieldBlock> calculateEndPath(int rowGo, int blackGo)
    {
        List<BattleFieldBlock> path = new List<BattleFieldBlock>();
        path.Add(battleFieldBlocks[rowGo][blackGo]);
        BattleFieldBlock currentBlock = path[0];
        while (currentBlock.comeFrom != null)
        {
            path.Add(currentBlock.comeFrom);
            currentBlock = currentBlock.comeFrom;
        }
        path.Reverse();
        return path;
    }

    private List<BattleFieldBlock> getNeighboursList(BattleFieldBlock currentBlock)
    {
        List<BattleFieldBlock> list = new List<BattleFieldBlock>();
        if ((currentBlock.row - 1) >= 0 && !battleFieldBlocks[currentBlock.row - 1][currentBlock.block].isOcupate)
        {
            list.Add(battleFieldBlocks[currentBlock.row - 1][currentBlock.block]);
        }



        if ((currentBlock.row + 1) < battleFieldBlocks.Length && !battleFieldBlocks[currentBlock.row + 1][currentBlock.block].isOcupate)
        {
            list.Add(battleFieldBlocks[currentBlock.row + 1][currentBlock.block]);
        }



        if ((currentBlock.block - 1) >= 0 && !battleFieldBlocks[currentBlock.row][currentBlock.block - 1].isOcupate)
        {
            list.Add(battleFieldBlocks[currentBlock.row][currentBlock.block - 1]);
        }



        if ((currentBlock.block + 1) < battleFieldBlocks[0].Length && !battleFieldBlocks[currentBlock.row][currentBlock.block + 1].isOcupate)
        {
            list.Add(battleFieldBlocks[currentBlock.row][currentBlock.block + 1]);
        }

        return list;
    }

    private BattleFieldBlock getLowestCostBlock(List<BattleFieldBlock> openList)
    {
        BattleFieldBlock lowestFCost = openList[0];
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].fCost < lowestFCost.fCost)
            {
                lowestFCost = openList[i];
            }
        }
        return lowestFCost;
    }

    private void pathPlayerMaker(int row, int block)
    {

        if (activePown == -1)
        {

        }
        else if (pathSelectetByPlayer.Contains(battleFieldBlocks[row][block]) && pathSelectetByPlayer.Count > 1)
        {
            int pathLong = pathSelectetByPlayer.Count - 1;
            if (pathSelectetByPlayer[pathLong - 1].row == row && pathSelectetByPlayer[pathLong - 1].block == block)
            {
                pathSelectetByPlayer[pathLong].hidePath();
                pathSelectetByPlayer.RemoveAt(pathLong);

                pathSelectetByPlayer[pathLong-1].hidePath();
                showPathOnOneBlock(pathLong, pathLong -1);
            }
        }
        else if (pawns[activePown].moveRange >= pathSelectetByPlayer.Count && !battleFieldBlocks[row][block].isOcupate)
        {
            BattleFieldBlock blocklastPath = pathSelectetByPlayer[pathSelectetByPlayer.Count - 1];
            int rowLast = blocklastPath.row;
            int lastblock = blocklastPath.block;

            if (((rowLast + 1 == row || rowLast - 1 == row) && (lastblock == block)) || ((lastblock - 1 == block || lastblock + 1 == block) && (rowLast == row)))
            {



                pathSelectetByPlayer.Add(battleFieldBlocks[row][block]);
                int pathLong = pathSelectetByPlayer.Count ;
                pathSelectetByPlayer[pathLong-2].hidePath();
                showPathOnOneBlock(pathLong, pathLong-2);
                showPathOnOneBlock(pathLong, pathLong-1);
            }
        }

    }
    private void setNewPath(int row, int block)
    {
        if (canMoveThere(activePown, row, block))
        {
            while (pathSelectetByPlayer.Count > 1)
            {
                pathSelectetByPlayer[1].hidePath();
                pathSelectetByPlayer.RemoveAt(1);
            }

            pathSelectetByPlayer = pathFindAStar(pawns[activePown].row, pawns[activePown].block, row, block);

            int endOfPath = pathSelectetByPlayer.Count;

            for (int i = 1; i < endOfPath; i++)
            {
                showPathOnOneBlock(endOfPath, i);

            }


        }
    }

    private void showPathOnOneBlock(int endOfPath, int i)
    {
        GD.Print("arrow");
        if (i > 0)
        {
            int rowDifBack = pathSelectetByPlayer[i].row - pathSelectetByPlayer[i - 1].row;

            int blockDifBack = pathSelectetByPlayer[i].block - pathSelectetByPlayer[i - 1].block;



            if (i == endOfPath - 1)
            {


                pathSelectetByPlayer[i].arrow.LookAt(pathSelectetByPlayer[i - 1].arrow.GlobalPosition);
                pathSelectetByPlayer[i].showArrow();

            }

            else
            {
                int rowDifFront = pathSelectetByPlayer[i].row - pathSelectetByPlayer[i + 1].row;
                int blockDifFront = pathSelectetByPlayer[i].block - pathSelectetByPlayer[i + 1].block;
                if (rowDifFront != rowDifBack && blockDifBack != blockDifFront)
                {
                    GD.Print(" rowDifBack =" + rowDifBack + " blockDifBack =" + blockDifBack + " rowDifFront=" + rowDifFront + " blockDifFront=" + blockDifFront);

                    if (rowDifBack == 1)
                    {

                        if (blockDifFront == 1)
                        {

                            GD.Print("LeftUP");
                            pathSelectetByPlayer[i].showCornerLeftUP();
                        }
                        else
                        {

                            GD.Print("LeftDown");
                            pathSelectetByPlayer[i].showCornerLeftDown();
                        }
                    }
                    else if(rowDifBack == -1)
                    {
                        if (blockDifFront == 1)
                        {

                            GD.Print("RightUP");
                            pathSelectetByPlayer[i].showCornerRightUP();
                        }
                        else
                        {

                            GD.Print("RightDown");
                            pathSelectetByPlayer[i].showCornerRightDown();
                        }


                    }else if(rowDifFront == 1)
                    {
                        if (blockDifBack == 1)
                        {

                            GD.Print("LeftUP");
                            pathSelectetByPlayer[i].showCornerLeftUP();
                        }
                        else
                        {

                            GD.Print("LeftDown");
                            pathSelectetByPlayer[i].showCornerLeftDown();
                        }

                    }
                    else if (rowDifFront == -1)
                    {
                        if (blockDifBack == 1)
                        {

                            GD.Print("RightUP");
                            pathSelectetByPlayer[i].showCornerRightUP();
                        }
                        else
                        {

                            GD.Print("RightDown");
                            pathSelectetByPlayer[i].showCornerRightDown();
                        }
                    }


                }
                else
                {

                    pathSelectetByPlayer[i].strgLine.LookAt(pathSelectetByPlayer[i - 1].arrow.GlobalPosition);
                    pathSelectetByPlayer[i].showstrgLine();
                }
            }
        }
    }

    private void clearPathMarks(List<BattleFieldBlock> pathSelectetByPlayer)
    {
        foreach (BattleFieldBlock battleFieldBlock in pathSelectetByPlayer)
        {
            battleFieldBlock.hidePath();
        }
    }


    private void endTurnEffect()
    {

    }




    private void initializationOfFieldBlockMatrix()
    {
        for (int i = 0; i < battleFieldBlocks.Length; i++)
        {
            battleFieldBlocks[i] = new BattleFieldBlock[6];

            for (int j = 0; j < battleFieldBlocks[i].Length; j++)
            {
                battleFieldBlocks[i][j] = GetNode<BattleFieldBlock>("battleField/Row" + i + "/" + j + "battleFieldBlock");
                battleFieldBlocks[i][j].fightingfield = this;
                battleFieldBlocks[i][j].row = i;
                battleFieldBlocks[i][j].block = j;
                battleFieldBlocks[i][j].mouseEntered += signalBlockMouseEntered;
                battleFieldBlocks[i][j].mouseExited += signalBlockMouseExited;
            }

        }
    }
    private void putPawnOnBlock(int pawn, int row, int block)
    {
        battleFieldBlocks[row][block].isOcupate = true;
        battleFieldBlocks[row][block].pawnOnBlock = pawn;
        Pawn p = pawns[pawn];


        Vector3 v = new Vector3();
        v = battleFieldBlocks[row][block].GlobalPosition;
        v.Y += 1.5f;
        p.row = row;
        p.block = block;
        p.GlobalPosition = v;

    }
    private void putPawnOnBlockWithAnimaton(int pawn, int row, int block, List<BattleFieldBlock> path)
    {
        battleFieldBlocks[row][block].isOcupate = true;
        battleFieldBlocks[row][block].pawnOnBlock = pawn;
        path.RemoveAt(0);
       
        
        pawns[pawn].moveList = new List<BattleFieldBlock>( path);

        pawns[pawn].Call("moveByBlockList");



        pawns[pawn].row = row;
        pawns[pawn].block = block;


    }

    private void fieldLightOFF()
    {
        for (int i = 0; i < battleFieldBlocks.Length; i++)
        {
            for (int j = 0; j < battleFieldBlocks[i].Length; j++)
            {


                battleFieldBlocks[i][j].Call("ligtOff");

            }

        }
    }


  


    //Time
    public void endTime()
    {
        GD.Print("Main dostaje info o koncu czasu");
    }
    public void startTime(int t)
    {
        interfaceee.Call("startTimer", t);
    }
    public void stopTime()
    {
        interfaceee.Call("stopTimer");
    }





    //ComputerMethods
    private void computerTurn()
    {
        if (computerAlgor())
        {
            computerMove();
        }
        else
        {
            computerAttac();
        }


    }

    private bool computerAlgor()
    {
        return true;
    }

    private void computerAttac()
    {

    }

    private void computerMove()
    {

    }



}