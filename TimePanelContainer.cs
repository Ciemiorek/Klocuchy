using Godot;
using System;

public partial class TimePanelContainer : PanelContainer
{


	Timer timer;
	ProgressBar progressBar;
	bool isCounting  = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		progressBar = GetNode<ProgressBar>("ProgressBar");
		timer.OneShot = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (!timer.IsStopped())
        {
			setValueProgresBar(timer.TimeLeft);
			GD.Print(timer.TimeLeft);
        }

	}

	public void setMaxProgresBar(int t)
    {
		progressBar.MaxValue = t;
    }
	public void setValueProgresBar(double t)
	{
		progressBar.Value = t;
	}

	public void timerStart(int t)
    {
		setMaxProgresBar(t);
		timer.Start(t);
		
    }

	public void timeHasStoped()
    {
		setValueProgresBar(0);
		



	}

	public void timerStopIt()
    {
		timer.Stop();
    }

}
