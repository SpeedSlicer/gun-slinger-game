using Godot;
using System;
using System.Diagnostics;

public partial class Geese : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	double damageCooldown = 0.5f;
	[Export] public double runDistance = 270.0f, walkDistance = 100f;
	[Export] public float runSpeed = 9, walkSpeed = 5;
	Node2D player;
	AnimatedSprite2D animatedSprite;
	Timer hurtTimer;
	CollisionShape2D collisionShape2D;
	bool unrun = false, wastouchingwall = false, destroying = false;
	Vector2 rundirection;
	Logger logger;
	[Export] double health = 1d;
	public override void _Ready()
	{
		base._Ready();
		hurtTimer = new Timer();
		player = GetNode<Node2D>("../Player");
		AddChild(hurtTimer);
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		hurtTimer.OneShot=true;
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		GD.Print("geese");

	}

	public override void _PhysicsProcess(double delta)
	{
		if (destroying) { return; }
		Vector2 velocity = Velocity;
		if (wastouchingwall)
		{
			rundirection.X = -rundirection.X;
			wastouchingwall=false;
			GD.Print("Flipping");
		}
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			
		}
		
		if (!hurtTimer.IsStopped()) {
			animatedSprite.Play("flap");
		}
		else
		{
			if (player.GlobalPosition.DistanceTo(GlobalPosition) < walkDistance)
			{
				if (unrun || (!unrun && rundirection == Vector2.Zero))
				{
					rundirection = (GlobalPosition - player.GlobalPosition).X > 0 ? Vector2.Right : Vector2.Left;
				}

				if (player.GlobalPosition.DistanceTo(GlobalPosition) < runDistance)
				{
					unrun = false;
					animatedSprite.Play("run");
					velocity.X = rundirection.X * runSpeed;
				}
				else
				{
					unrun = false;
					velocity.X = rundirection.X * walkSpeed;
					animatedSprite.Play("walk");					
				}
				
			}
			
			else
			{
				velocity.X = 0;
				unrun = true;
				animatedSprite.Play("idle");
			}
		}
		
		Velocity = velocity;
		MoveAndSlide();
		if (GetSlideCollisionCount() > 0 && !wastouchingwall)
		{
			for (int i = 0; i < GetSlideCollisionCount(); i++)
			{
				if (GetSlideCollision(i).GetNormal() == new Vector2(-1, 0) || 
				GetSlideCollision(i).GetNormal() == new Vector2(1, 0))
				{
					wastouchingwall=true;
				}
	
 		}}
		animatedSprite.FlipH = velocity.X < 0;
	}
	public void Die()
	{
		destroying = true;
		GpuParticles2D particles2D = GetNode<GpuParticles2D>("Particles");
		if (particles2D != null)
		{
			particles2D.Emitting = true;
		}
		GameManager gm = GetNode<GameManager>("../GameManager");
		gm.SpawnNew();
		animatedSprite.QueueFree();
		Destroy();
	}
	public async void Destroy()
	{
		await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
		QueueFree();
	}
	public void Damage(double amount)
	{
		if (hurtTimer.IsStopped())
		{
			health-=amount;
			hurtTimer.Start(damageCooldown);
		}
		if (health <= 0 && !destroying)
		{
			Die();
		}
	}

	public double roundToValues(double top, double bottom, double value)
	{
		return Math.Abs(top - value) <= Math.Abs(bottom - value) ? top : bottom;
	}
 }
