using Godot;

public partial class Bullet : Area2D
{
	public float speed;
	public Vector2 direction;
	public float lifetime;
	double damage = 1.0f;
	Timer timer;
	public override void _Ready()
	{
		Monitoring = true;
		timer = new Timer();
		AddChild(timer);

		timer.OneShot=true;
		timer.Start(lifetime);
	}

	public override void _Process(double delta)
	{
		Position += direction * (float)(speed * delta);
		
		if (timer.IsStopped())
		{
			QueueFree();
		}
		if (HasOverlappingBodies())
		{
			foreach ( var x in GetOverlappingBodies())
			{
				if (x is Geese geese)
				{
					geese.Damage(damage);
					QueueFree();
				}
				if (x is TileMapLayer)
				{
					QueueFree();
				}
			} 
		}
	}
}
