using Godot;
using System;

public partial class Player : CharacterBody3D
{
	
	[Signal]
	public delegate void HitEventHandler();
	[Export] 
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleratioon { get; set; } = 75;
	
	[Export]
	public int JumpImpulse { get; set; } = 20;
	
	[Export]
	public int BounceImpulse { get; set; } = 16;
	
	
	private Vector3 _targetVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_targetVelocity = Vector3.Up * JumpImpulse;
		}

		for (var index = 0; index < GetSlideCollisionCount(); index++)
		{
			var collision = GetSlideCollision(index);

			if (collision.GetCollider() is Mob mob)
			{
				if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
				{
					mob.Squash();
					_targetVelocity.Y = BounceImpulse;

					break;
				}
			}
		}
		

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
			GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 4;

		}
		else
		{
			GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 1;
		}
		
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleratioon * (float)delta;
		}
		
		var pivot = GetNode<Node3D>("Pivot");
		pivot.Rotation = new Vector3(Mathf.Pi / 6.0f * Velocity.Y / JumpImpulse, pivot.Rotation.Y, pivot.Rotation.Z);
		
		
		Velocity = _targetVelocity;
		MoveAndSlide();
	}

	private void die()
	{
		EmitSignal(SignalName.Hit);
		QueueFree();
	}

	private void _on_mob_detector_body_entered(Node3D body)
	{
		die();
	}
}
