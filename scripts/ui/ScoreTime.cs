using Godot;
using System;

public partial class ScoreTime : Panel
{
	RichTextLabel rl_score, rl_topScore;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rl_score = GetNode<RichTextLabel>("Score");
		rl_topScore = GetNode<RichTextLabel>("Best");
		rl_score.Text = $"Last Score: {GameManager.lastScore}";
		rl_topScore.Text = $"Best Score: {GameManager.bestScore}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
