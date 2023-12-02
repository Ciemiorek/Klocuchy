using Godot;
using System;

public partial class buttonsAttacks : MarginContainer
{
	Button[] buttons = new Button[4];
    Attack[] attacks = new Attack[4];



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int i = 0; i < 4; i++)
        {
			buttons[i] = GetNode<GridContainer>("GridContainer").GetNode<Button>("Button" + (i + 1));
        }

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void setAttackOnButton(Attack[] attacks)
    {
		this.attacks = attacks;
		for(int i = 0; i < attacks.Length; i++)
        {
			if (attacks[i] != null)
			{
				buttons[i].Text = this.attacks[i].attackName;
				buttons[i].Visible = true;
            }
            else
            {
				buttons[i].Visible = false;
            }
		}
        

	}



	[Signal]
	public delegate void buttonAttackPushedEventHandler(String buttonName);



	public void buttonAttacksPushedEmitSignal(String buttonName)
	{
		EmitSignal(SignalName.buttonAttackPushed, buttonName);
	}

	public void buttonAttack1()
    {
		buttonAttacksPushedEmitSignal(buttons[0].Text);

	}
	public void buttonAttack2()
	{
		buttonAttacksPushedEmitSignal(buttons[1].Text);
	}
	public void buttonAttack3()
	{
		buttonAttacksPushedEmitSignal(buttons[2].Text);
	}
	public void buttonAttack4()
	{
		buttonAttacksPushedEmitSignal(buttons[3].Text);

	}


	[Signal]
	public delegate void buttonAttackMouseEnterEventHandler(Attack attack);
	public void buttonAttackMouseEnterEmitSignal(Attack attack)
	{
		EmitSignal(SignalName.buttonAttackMouseEnter, attack);
	}
	public void buttonAttack1MouseEnter()
	{
		buttonAttackMouseEnterEmitSignal(attacks[0]);

	}
	public void buttonAttack2MouseEnter()
	{
		buttonAttackMouseEnterEmitSignal(attacks[1]);
	}
	public void buttonAttack3MouseEnter()
	{
		buttonAttackMouseEnterEmitSignal(attacks[2]);
	}
	public void buttonAttack4MouseEnter()
	{
		buttonAttackMouseEnterEmitSignal(attacks[3]);

	}



	[Signal]
	public delegate void buttonAttackMouseExitEventHandler();
	public void buttonAttackMouseExitEmitSignal()
	{
		EmitSignal(SignalName.buttonAttackMouseExit);
	}

	public void buttonAttack1MouseExit()
	{
		buttonAttackMouseExitEmitSignal();

	}
	public void buttonAttack2MouseExit()
	{
		buttonAttackMouseExitEmitSignal();
	}
	public void buttonAttack3MouseExit()
	{
		buttonAttackMouseExitEmitSignal();
	}
	public void buttonAttack4MouseExit()
	{
		buttonAttackMouseExitEmitSignal();

	}

}
