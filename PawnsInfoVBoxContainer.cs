using Godot;
using System;

public partial class PawnsInfoVBoxContainer : VBoxContainer
{

	GridContainer[] pawnInfo = new GridContainer[6];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < pawnInfo.Length; i++)
		{
			pawnInfo[i] = GetNode<GridContainer>("Pawn0" + (i + 1));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void changeHeltBar(int hp, int numberFromTop )
    {
		pawnInfo[numberFromTop].Call("changeValueOfHealthBar", hp);
		
	}

	public void changeName(string name,int numberFromTop)
    {
		pawnInfo[numberFromTop].Call("setName", name);

	}
	
	public void setMaxHP(int maxHP, int numberFromTop)
	{
		pawnInfo[numberFromTop].Call("setMaxValueOfHealthBar", maxHP);

	}
	public void setcurrentHP(int HP, int numberFromTop)
	{
		pawnInfo[numberFromTop].Call("setValueOfHealthBar", HP);

	}

}
