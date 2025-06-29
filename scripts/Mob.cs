using Godot;
using System;

public partial class Mob : CharacterBody3D
{
	[Signal]
	public delegate void SquashedEventHandler();
	[Export] 
	public int MinSpeed { get; set; } = 10;
	[Export] 
	public int MaxSpeed { get; set; } = 18;

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void InitializeMob(Vector3 startPosition, Vector3 playerPosition)
	{
		LookAtFromPosition(startPosition, playerPosition);
		
		RotateY((float)GD.RandRange(-Mathf.Pi/4, Mathf.Pi/4));
		
		int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
		
		Velocity = new Vector3(0,0,-1) * randomSpeed;
		
		Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
	}

	private void _on_visible_on_screen_notifier_3d_screen_exited()
	{
		QueueFree();
	}

	public void Squash()
	{
		EmitSignal(SignalName.Squashed);
		QueueFree();
	}
}
