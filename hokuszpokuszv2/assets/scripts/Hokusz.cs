using System.Runtime.CompilerServices;
using Godot;

public partial class Hokusz : CharacterBody2D
{
	public int direction = 1;
	public bool jump;
	public const float Speed = 100.0f;
	public const float JumpVelocity = 750.0f;

	// Removal
	public void OnQueueFreeTimerTimeout() { QueueFree(); }

	// Collision
	public void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("sword"))
		{
			GetNode<CollisionShape2D>("Hitbox").SetDeferred(
				CollisionShape2D.PropertyName.Disabled,
				true
			);
			GetNode<Area2D>("CollCheck").SetDeferred(
				Area2D.PropertyName.Monitorable,
				false
			);
			GetNode<AnimatedSprite2D>("Anim").Hide();
			GetNode<Timer>("QueueFreeTimer").Start();

			GlobalData.GameData._score += 100;
			GetNode<AudioStreamPlayer>("DeathSound").Play();
			GetNode<GpuParticles2D>("DeathParticle").Emitting = true;
		}
	}

	// Move to player
	public void LocatePlayer()
	{
		if (GetNode<CharacterBody2D>("../../Player") == null) { direction = 0; return; }
		var playerPos = GetNode<CharacterBody2D>("../../Player").GlobalPosition;
		var enemyPos = GlobalPosition;

		// Left/Right
		if (enemyPos.X > playerPos.X)
		{
			direction = -1;
		}
		else if (enemyPos.X < playerPos.X)
		{
			direction = 1;
		}

		// Jumping
		if (enemyPos.Y - 64 > playerPos.Y)
		{
			jump = true;
		}
	}

	public override void _Ready()
	{
		LocatePlayer();
		GetNode<AnimatedSprite2D>("Anim").Play();
    }


	public override void _PhysicsProcess(double delta)
	{
		var gameState = GlobalData.GameData._gameState;
		if (gameState == GlobalData.GameState.Menu || gameState == GlobalData.GameState.GameOver)
		{
			QueueFree();
		}

		else if (gameState == GlobalData.GameState.Playing)
		{
			Vector2 velocity = Velocity;

			// Gravity
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}

			// Jump
			if (jump && IsOnFloor())
			{
				jump = false;
				velocity.Y = -JumpVelocity;
			}

			// Movement
			if (direction != 0)
			{
				velocity.X = direction * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

			// Sprite flip
			GetNode<AnimatedSprite2D>("Anim").FlipH = direction < 0;

			Velocity = velocity;
			MoveAndSlide();
		}

	}
}
