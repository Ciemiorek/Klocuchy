using Godot;
using System;
using System.Collections.Generic;

public partial class Pawn : Node3D


{
	bool isMoving = false;
	public List<BattleFieldBlock> moveList = null;
	private Vector3 destini;
	MeshInstance3D lightUp;
	StaticBody3D staticBody3D;
	[Export]
	Camera3D camera;
	public Node3D fightingfield { get; set; }
	public bool active = false;
	public int row { get; set; }
	public int block { get; set; }
	public int numberPawn { get; set; }
	public Attack[] attacks { get; set; }
	public int moveRange { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lightUp = GetNode<MeshInstance3D>("lightUp");
		staticBody3D = GetNode<MeshInstance3D>("MeshInstance3D").GetNode<StaticBody3D>("StaticBody3D");
		lightUp.Visible = false;
		attacks = new Attack[4];
		moveList = new List<BattleFieldBlock>();
		moveRange = numberPawn;



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		

        if (isMoving )
        {
			if (moveList.Count != 0)
			{
				Vector3 currentPos = this.GlobalPosition;
				float distance = destini.DistanceTo(currentPos);
				int speed = 10;
                if (distance < 0.8f)
                {
					destini.Y = 1.5f;
				}
                else
                {
					
                }

				this.GlobalPosition= currentPos.MoveToward(destini, (float)delta*speed);


                if (GlobalPosition == destini)
                {
					moveList.RemoveAt(0);
					if(moveList.Count == 0)
                    {
						isMoving = false;
                    }
                    else
                    {
						destini = moveList[0].GlobalPosition;
						destini.Y+=3f;
                    }
					
                }

				
			}
		}



	}

   

    public void inputEvent(Camera3D camera, InputEvent eventt, Vector3 position, Vector3 normal, int shape_idx)
	{

		fightingfield.Call("pawnsEvents", camera, eventt, position, normal, shape_idx,numberPawn);
		
	}


	public void createPawn(int numberPawn, Color color)
    {

		this.numberPawn = numberPawn;
		changeLabel(numberPawn.ToString());
		StandardMaterial3D mat = new StandardMaterial3D();
		mat.AlbedoColor = color;

		GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverlay = mat;

	}
	public void changeLabel(string a)
	{
		

		GetNode<Label>("Sprite3D/SubViewport/Label").Text = a;

	}

	public void mouseOn()
	{
		changeLabel("Mouse on");
		lightUp.Visible=true;
		
		
		
		
	}
	public void mouseOFF()
	{
		if (!active)
		{
			changeLabel("Mouse off");
			lightUp.Visible = false;
		}
	}

	public void setNumberPawn(int a)
    {
		numberPawn = a;
    }

	public void setActive()
    {
		active =true;
        
        
		lightUp.Visible = true;
		staticBody3D.InputRayPickable = false;
    }

	public void setNotActive()
	{
		active = false;


		lightUp.Visible = false;
		staticBody3D.InputRayPickable = true;
	}

	

	public void moveByBlockList()
    {
		isMoving = true;
		destini = moveList[0].GlobalPosition;
		destini.Y += 3f;

	}


	
}
