using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	[Export] public Vector2 armPosLeft = new Vector2(2.5f, 5.0f), armPosRight = new Vector2(-3, 5.0f);
	float tolerance = 0.1f;

	AnimatedSprite2D animatedSprite;
	Node2D visuals;
	public override void _Ready()
	{
		visuals = GetNode<Node2D>("Visuals");
		animatedSprite = visuals.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	}	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
	

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		float direction = Input.GetAxis("walk_left", "walk_right");
		if (direction != 0)
		{
			velocity.X = direction * Speed;
			if (direction < 0)
			{
				visuals.Scale = new Vector2(-1, Scale.Y);

			}
			else if (direction > 0)
			{
				visuals.Scale = new Vector2(1, Scale.Y);
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		animatedSprite.Play(velocity.X == 0 ? "idle" : "walk");

		Velocity = velocity;
		
		MoveAndSlide();
	}
}
