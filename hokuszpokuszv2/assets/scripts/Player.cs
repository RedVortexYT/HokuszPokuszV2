using Godot;

public partial class Player : CharacterBody2D
{
	public AnimatedSprite2D ANIM;
	public Area2D SWORD;
	public bool canAttack;
	public const float SPEED = 300.0f;
	public const float JUMPVELOCITY = 750.0f;

	// Game Over
	public void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("enemy"))
		{
			GetNode<CollisionShape2D>("Hitbox").SetDeferred(
				CollisionShape2D.PropertyName.Disabled,
				true
			);
			GetNode<AnimatedSprite2D>("Anim").Hide();

			SWORD.SetDeferred(
				Area2D.PropertyName.Monitorable,
				false
			);
			SWORD.Hide();

			GetNode<AudioStreamPlayer>("DeathSound").Play();
			GetNode<GpuParticles2D>("DeathParticle").Emitting = true;

			GlobalData.GameData._gameState = GlobalData.GameState.GameOver;
		}
	}

	// Attack Cooldown
	public void OnAttackTimerTimeout()
	{
		canAttack = true;
		SWORD.SetDeferred(
			Area2D.PropertyName.Monitorable,
			false
		);
	}

	public override void _Ready()
	{
		canAttack = true;
		ANIM = GetNode<AnimatedSprite2D>("Anim");
		SWORD = GetNode<Area2D>("Sword");
		SWORD.SetDeferred(
			Area2D.PropertyName.Monitorable,
			false
		);
    }


	public override void _PhysicsProcess(double delta)
	{
		var gameState = GlobalData.GameData._gameState;
		if (gameState == GlobalData.GameState.Menu)
		{
			GlobalPosition = new Vector2(480, 480);
			GetNode<CollisionShape2D>("Hitbox").SetDeferred(
				CollisionShape2D.PropertyName.Disabled,
				false
			);
			GetNode<AnimatedSprite2D>("Anim").Show();

			SWORD.Show();
		}

		else if (gameState == GlobalData.GameState.Playing)
		{
			Vector2 velocity = Velocity;

			// Gravity
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}

			// Jumping
			if (Input.IsActionJustPressed("jump") && IsOnFloor())
			{
				velocity.Y = -JUMPVELOCITY;
			}

			// Movement
			float direction = Input.GetAxis("moveL", "moveR");
			if (direction != 0)
			{
				velocity.X = direction * SPEED;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, SPEED);
			}

			Velocity = velocity;
			MoveAndSlide();

			// Animation
			if (direction < 0)
			{
				ANIM.FlipH = true;
				ANIM.Play();
			}
			else if (direction > 0)
			{
				ANIM.FlipH = false;
				ANIM.Play();
			}
			else
			{
				ANIM.Stop();
			}

			// Sword Position
			if (direction < 0)
			{
				SWORD.Position = new Vector2(-32, 4);
				SWORD.GetNode<Sprite2D>("Sprite").FlipH = true;
			}
			else if (direction > 0)
			{
				SWORD.Position = new Vector2(32, 4);
				SWORD.GetNode<Sprite2D>("Sprite").FlipH = false;
			}

			// Attack
			if (Input.IsActionPressed("attack") && canAttack)
			{
				SWORD.SetDeferred(
					Area2D.PropertyName.Monitorable,
					true
				);

				canAttack = false;
				GetNode<Timer>("Sword/AttackTimer").Start();

				SWORD.Rotation += SWORD.Position.X / 2 * (float)delta;
			}

			// Sword Rotation
			if (SWORD.Rotation != 0)
			{
				SWORD.Rotation += SWORD.Position.X / 2 * (float)delta;
			}

			// Sword Rotation Reset
			if (SWORD.Rotation > Mathf.DegToRad(360) || SWORD.Rotation < Mathf.DegToRad(-360))
			{
				SWORD.Rotation = 0;
			}
		}

	}
}
