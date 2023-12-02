using Godot;
using System;

public partial class PawnInfoInterface : GridContainer
{
	// Called when the node enters the scene tree for the first time.
	
	ProgressBar progressBar;
	RichTextLabel name;
	int curentHP;
	int maxHP;
	RichTextLabel hpOnMaxhp;

	public override void _Ready()
	{
		progressBar = GetNode<MarginContainer>("MarginContainer").GetNode<ProgressBar>("HelthProgressBar");
		name = GetNode<RichTextLabel>("RichTextLabel");
		hpOnMaxhp = GetNode<MarginContainer>("MarginContainer").GetNode<RichTextLabel>("hpOnMaxHp");
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void changeValueOfHealthBar(int a)
    {
		curentHP += a;
		progressBar.Value += a;
		hpOnMaxhp.Text = "[center]"+curentHP+"/"+maxHP;

	}

	public void setMaxValueOfHealthBar(int a)
    {
		maxHP = a;
		progressBar.MaxValue = a;
		hpOnMaxhp.Text = "[center]" + curentHP + "/" + maxHP;
	}
	public void setValueOfHealthBar(int a)
	{
		curentHP = a;
		progressBar.Value = a;
		hpOnMaxhp.Text = "[center]" + curentHP + "/" + maxHP;

	}

	public void setName(string name)
    {
		this.name.Text = name;
    }


}
