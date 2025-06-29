using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export] 
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleratioon { get; set; } = 75;
	private Vector3 _targetVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}

		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}

		if (Input.IsActionPressed("move_foward"))
		{
			direction.Z -= 1.0f;
		}

		if (direction != Vector3.Zero)
		{
			direction.Normalized();
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}
		
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleratioon * (float)delta;
		}
		
		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}
