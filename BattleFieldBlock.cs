using Godot;
using System;

public partial class BattleFieldBlock : Node3D

{
	StandardMaterial3D blockMaterial = new StandardMaterial3D();
	StandardMaterial3D materialLight = new StandardMaterial3D();

	public Node3D light;
	public Node3D fire;
	public Node3D arrow;
	public Node3D strgLine;
	public Node3D corner;

	public Node3D fightingfield { get; set; }
	public int row { get; set; }
	public int block { get; set; }
	public bool isOcupate = false;
	public int pawnOnBlock = -1;
	private double sumdelta;


	//Calcuate path
	public int gCost;
	public int hCost;
	public int fCost;
	public BattleFieldBlock comeFrom;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fire = GetNode<Node3D>("Fire");
		light = GetNode<Node3D>("light");
		arrow = GetNode<Node3D>("arrow");
		strgLine = GetNode<Node3D>("StrLine");
		corner = GetNode<Node3D>("Corner");


		blockMaterial.Uv1Scale = new Vector3(3, 2, 1);

		GetNode<MeshInstance3D>("blockMeshInstance3D").SetSurfaceOverrideMaterial(0, blockMaterial);
		light.GetNode<MeshInstance3D>("lightMeshInstance3D").SetSurfaceOverrideMaterial(0, materialLight);
		changeTexturBlack();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


	}


	public void inputEvent(Camera3D camera, InputEvent eventt, Vector3 position, Vector3 normal, int shape_idx)
	{

		fightingfield.Call("blocksEvents", camera, eventt, position, normal, shape_idx, row,block);

	}



	public void changeLightColor(string color)
	{
		if (!light.Visible)
		{
			light.Visible = true;
		}

		if (color == "RED")
		{
			materialLight.AlbedoColor = new Color(1, 0, 0.3f, 0.6f);

		}
		else if (color == "GREEN")
		{
			materialLight.AlbedoColor = new Color(0, 1, 0.3f, 0.6f);
		}
	}
	public void ligtOff()
	{
		if (light.Visible)
		{
			light.Visible = false;
		}

	
	}

	public void changefire()
	{
		if (!fire.Visible)
		{
			fire.Visible = true;
        }
        else
        {
			fire.Visible = false;
        }
	}

	public void changeTexturRed()
    {
		blockMaterial.AlbedoTexture = ResourceLoader.Load<Texture2D>("matirials/groundRed.2png.png");
	}
	public void changeTexturBlack()
	{
		blockMaterial.AlbedoTexture = ResourceLoader.Load<Texture2D>("res://matirials/groundBlack.2png.png");
	}
	public void changeTexturGreen()
	{
		blockMaterial.AlbedoTexture = ResourceLoader.Load<Texture2D>("matirials/groundGreen.2png.png");
	}
	public void calculateFCost()
    {
		fCost = gCost + hCost;
    }
	 

	public void hidePath()
    {
		corner.Visible = false;
		arrow.Visible = false;
		strgLine.Visible = false;
		corner.Rotation = new Vector3(0, 0, 0);
		arrow.Rotation = new Vector3(0, 0, 0);
		strgLine.Rotation = new Vector3(0, 0, 0);
	}
	public void showCornerLeftUP()
    {
		corner.RotateY(4.71238898f);
		corner.Visible = true;
		GD.Print(corner.Rotation);

	}

	public void showCornerRightUP()
	{
		corner.RotateY(3.14159265f);
		corner.Visible = true;
		GD.Print(corner.Rotation);
	}
	public void showCornerRightDown()
	{
		corner.RotateY(1.57079633f);
		corner.Visible = true;
		GD.Print(corner.Rotation);
	}

	public void showCornerLeftDown()
	{
		
		corner.Visible = true;
		GD.Print(corner.Rotation);
	}
	public void showArrow()
	{
		arrow.Visible = true;

	}
	public void showstrgLine()
	{
		strgLine.Visible = true;

	}


	[Signal]
	public delegate void mouseEnteredEventHandler(int row, int block);

	public void mouseEnteredEmitSignal()
    {
		changeTexturGreen();
		EmitSignal(SignalName.mouseEntered,row,block);

	}

	[Signal]
	public delegate void mouseExitedEventHandler(int row, int block);


	public void mouseExitedEmitSignal()
	{
		
		changeTexturBlack();
		EmitSignal(SignalName.mouseExited, row, block);
	}



}
