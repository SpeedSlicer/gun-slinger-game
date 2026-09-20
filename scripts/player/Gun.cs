using Godot;

public partial class Gun : Node2D
{
	Node2D firepoint, camera, visuals;
	public readonly PackedScene Bullet = ResourceLoader.Load<PackedScene>("res://prefabs/bullet_prefab.tscn");
	// Called when the node enters the scene tree for the first time.

	[Export] public float Cooldown = 0.5f, BulletSpeed = 500.0f, Lifetime = 2f;
	float lastShot = 0;

	Timer timer;
	public override void _Ready()
	{
		timer = new Timer();
		timer.OneShot = true;
		firepoint = GetNode<Node2D>("Arm1/GunHandle/TriggerPoint");
		camera = GetNode<Node2D>("../../Camera2D");
		visuals = GetNode<Node2D>("../../Visuals");
		AddChild(timer);
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();
		
		Vector2 direction = (camera.GetGlobalPosition() - mousePos).Normalized();

		Rotation = visuals.Scale.X * (direction.Angle() + Mathf.Pi) + (visuals.Scale.X < 0 ? Mathf.Pi : 0);

		if (Input.IsActionPressed("shoot") && !CantShoot())
		{
			timer.Start(Cooldown);
			var bullet = Bullet.Instantiate<Bullet>();
			bullet.Position = firepoint.GetGlobalPosition();
			bullet.Rotation = Rotation * visuals.Scale.X;
			bullet.direction = -direction;
			bullet.speed =BulletSpeed;
			bullet.lifetime = Lifetime;
			GetTree().CurrentScene.AddChild(bullet);
		}
	}

	public bool CantShoot()
	{
		return !timer.IsStopped();
	}
}
