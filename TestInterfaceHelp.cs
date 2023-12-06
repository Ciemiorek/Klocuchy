using Godot;
using System;
using System.Collections.Generic;

public partial class TestInterfaceHelp : Control
{
	List<RichTextLabel> listRichTaxt = new List<RichTextLabel>();

	int witchOne = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int i = 1; i < 3; i++)
        {
			for(int j = 1; j < 8; j++)
            {
				listRichTaxt.Add(GetNode<HSplitContainer>("HSplitContainer").GetNode<VBoxContainer>("VBoxContainer" + i).GetNode<RichTextLabel>("RichTextLabel" + j)); 
				
			}

			
        }

		foreach(RichTextLabel label in listRichTaxt)
        {
			label.Text = "";
			label.MouseFilter = MouseFilterEnum.Pass;
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void addText(String whatToShowUP,int i)
    {

		listRichTaxt[i].Text = whatToShowUP;

		
	}
}
