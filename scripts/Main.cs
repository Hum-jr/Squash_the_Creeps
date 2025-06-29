using Godot;
using System;

public partial class Main : Node
{
	
	[Export]
	public PackedScene MobScene { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Control>("UI/Retry").Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_mob_timer_timeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		
		Vector3 playerPosition = GetNode<Player>("Player").Position;
		mob.InitializeMob(mobSpawnLocation.Position, playerPosition);
		
		AddChild(mob);

		mob.Squashed += GetNode<ScoreLabel>("UI/ScoreLabel")._on_mob_squashed;

	}

	private void _on_player_hit()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Control>("UI/Retry").Show();

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UI/Retry").Visible)
		{
			GetTree().ReloadCurrentScene();
		}
	}
	
}
