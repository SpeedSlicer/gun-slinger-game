using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node2D
{
	public int score = 0;
	static int length = 4;
	const double game_time = 20;
	const int startAmountGoose = 3;
	public static int lastScore = 0, bestScore = 0;
	Node2D[] nodes = new Node2D[length];
	public readonly PackedScene goose = ResourceLoader.Load<PackedScene>("res://prefabs/geese.tscn");
	Node2D SCENE;
	RichTextLabel rl_time, rl_score;
	Timer timeLeft;
	PackedScene menuScene = ResourceLoader.Load<PackedScene>("res://scenes/MainMenu.tscn");

	public override void _Ready()
	{
		for (int i = 0; i < length; i++)
		{
			nodes[i] = GetNode<Node2D>("Point" + (i + 1));
		}
		
		rl_time = GetNode<RichTextLabel>("../UI/ScoreTime/Time");
		rl_score = GetNode<RichTextLabel>("../UI/ScoreTime/Score");
		timeLeft = new Timer();
		AddChild(timeLeft);
		timeLeft.OneShot = true;
		timeLeft.Start(game_time);
		for (int i = 0; i < startAmountGoose; i ++)
		{
			SummonGoose();
		}
		rl_score.Text = $"Score: 0";

		
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		rl_time.Text = $"Time Left: {Math.Round(timeLeft.TimeLeft * 100) / 100}";

		if (timeLeft.IsStopped())
		{
			lastScore = score;
			if (bestScore < lastScore)
			{
				bestScore = lastScore;
			}
			GetTree().ChangeSceneToPacked(menuScene);
		}
	}
	public void SpawnNew()
	{
		score++;
		SummonGoose();
		rl_score.Text = $"Score: {score}";
	}
	private async void SummonGoose()
	{
		await Task.Delay(50);
		GD.Print("out");
		int random = new Random().Next(0,nodes.Length);
		Geese geese = (Geese) goose.Instantiate();
		geese.Position = nodes[random].GlobalPosition;
		GetTree().CurrentScene.CallDeferred("add_child", geese);
	}
}
