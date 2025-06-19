using System;
using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene;

	// Enemy Spawning
	public void OnSpawnTimerTimeout() { SpawnEnemy(); }
	public void SpawnEnemy(int _spawnPos = -1)
	{
		var enemy = (CharacterBody2D)EnemyScene.Instantiate();

		Random random = new Random();
		if (_spawnPos == -1) { _spawnPos = random.Next(1, 6); }

		enemy.GlobalPosition = GetNode<Marker2D>($"Hokusz/HokuszSpawn{_spawnPos}").GlobalPosition;
		GetNode<Node2D>("Hokusz").AddChild(enemy);
	}

	public override void _Ready()
	{
		for (int i = 1; i <= 5; i++) { SpawnEnemy(i); }
	}


	public override void _Process(double delta)
	{
	}
}
