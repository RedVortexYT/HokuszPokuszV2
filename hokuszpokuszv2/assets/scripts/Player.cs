using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public AnimatedSprite2D ANIM;
	public Area2D SWORD;
	public const float SPEED = 300.0f;
	public const float JUMPVELOCITY = 750.0f;

	public override void _Ready()
	{
		ANIM = GetNode<AnimatedSprite2D>("Anim");
		SWORD = GetNode<Area2D>("Sword");
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

		// Sword
		if (direction < 0)
		{
			SWORD.Position = new Vector2(-32, -8);
			SWORD.GetNode<Sprite2D>("Sprite").FlipH = true;
		}
		else if (direction > 0)
		{
			SWORD.Position = new Vector2(32, -8);
			SWORD.GetNode<Sprite2D>("Sprite").FlipH = false;
		}

	}
}
