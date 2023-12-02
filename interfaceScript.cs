using Godot;
using System;

public partial class interfaceScript : Control
{

	VBoxContainer PawnsInfoVBoxContainer;
	PanelContainer TimePanelContainer;
	RichTextLabel TextPanelContainer;
	MarginContainer buttonsMarginContainer;
	MarginContainer buttonsAttacksMarginContainer;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PawnsInfoVBoxContainer = GetNode<VBoxContainer>("PawnsInfoVBoxContainer");
		TimePanelContainer = GetNode<PanelContainer>("TimePanelContainer");
		TextPanelContainer = GetNode<PanelContainer>("TextPanelContainer").GetNode<RichTextLabel>("Text");
		buttonsMarginContainer = GetNode<MarginContainer>("buttonsAIM");
		buttonsAttacksMarginContainer =  GetNode<MarginContainer>("buttonsAttacks");


	}




	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//infoPawns
	public void setName(string name ,int numberFromTop)
    {
		PawnsInfoVBoxContainer.Call("changeName",name, numberFromTop);

	}
	public void changeHP(int hp, int numberFromTop)
	{
		PawnsInfoVBoxContainer.Call("changeHeltBar", hp, numberFromTop);

	}
	public void setMaxHP(int maxHp, int numberFromTop)
    {
		PawnsInfoVBoxContainer.Call("setMaxHP", maxHp, numberFromTop);
	}
	public void setCurrentHp(int hp,int numberFromTop)
    {
		PawnsInfoVBoxContainer.Call("setcurrentHP", hp, numberFromTop);

	}




	//Text
	public void showText(string a)
    {
		TextPanelContainer.Text = a;	

	}
	public void clearText()
    {
		TextPanelContainer.Clear();

	}
	public void showAttackText(Attack attack)
    {
		GD.Print("Najechane");
		TextPanelContainer.Text = attack.attackName + " POWER :" + attack.baseDMG;

	}



	//Time
	public void startTimer(int s) {
		TimePanelContainer.Call("timerStart",s);
	}
	public void stopTimer() {
		TimePanelContainer.Call("timerStopIt");
	}

	[Signal]
	public delegate void timeHasStopedEventHandler();

	public void timeHasStopedEmitSignal()
    {
		EmitSignal(SignalName.timeHasStoped);
    }


	//buttons
	public void setAttack(Attack[] attacks) {

			
			buttonsAttacksMarginContainer.Call("setAttackOnButton", attacks);


		
        

	}
	public void setButton(String nameButton, int witchOne)
    {
		buttonsMarginContainer.Call("setNameAttackOnButton", witchOne, nameButton);

	}
	public void changeVisibilityAttacksButtons(bool visibility)		
	{
		buttonsAttacksMarginContainer.Visible = visibility;

	}
	public void changeVisibilityButtons(bool visibility)
	{
		buttonsMarginContainer.Visible = visibility;

	}



	[Signal]
	public delegate void buttonAttacksHasBeenPushedEventHandler(String buttonName);
	public void buttonAttacksHasBeenPushedEmitSignal(String buttonName)
	{
		EmitSignal(SignalName.buttonAttacksHasBeenPushed, buttonName);
	}

	[Signal]
	public delegate void buttonHasBeenPushedEventHandler(String buttonName);
	public void buttonHasBeenPushedEmitSignal(String buttonName)
	{
		EmitSignal(SignalName.buttonHasBeenPushed, buttonName);
	}




}
