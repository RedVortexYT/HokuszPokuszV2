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
			Hide();

			GetNode<CollisionShape2D>("Sword/Hitbox").SetDeferred(
				CollisionShape2D.PropertyName.Disabled,
				true
			);
		}
	}

	// Attack Cooldown
	public void OnAttackTimerTimeout()
	{
		canAttack = true;
		GetNode<CollisionShape2D>("Sword/Hitbox").SetDeferred(
			CollisionShape2D.PropertyName.Disabled,
			true
		);
	}

	public override void _Ready()
	{
		canAttack = true;
		ANIM = GetNode<AnimatedSprite2D>("Anim");
		SWORD = GetNode<Area2D>("Sword");
		GetNode<CollisionShape2D>("Sword/Hitbox").SetDeferred(
			CollisionShape2D.PropertyName.Disabled,
			true
		);
    }


	public override void _PhysicsProcess(double delta)
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
			GetNode<CollisionShape2D>("Sword/Hitbox").SetDeferred(
				CollisionShape2D.PropertyName.Disabled,
				false
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
