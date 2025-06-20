using System;
using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene;

	public Timer SPAWNTIMER;
	
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
		SPAWNTIMER = GetNode<Timer>("Hokusz/SpawnTimer");
	}


	public override void _Process(double delta)
	{
		var gameState = GlobalData.GameData._gameState;
		var score = GlobalData.GameData._score;

		if (gameState == GlobalData.GameState.Menu)
		{
			GlobalData.GameData._score = 0;
			SPAWNTIMER.Stop();
		}

		else if (gameState == GlobalData.GameState.Playing)
		{
			// HiScore Update
			if (score > GlobalData.GameData._hiScore)
			{
				GlobalData.GameData._hiScore = score;
			}
		}

		else if (gameState == GlobalData.GameState.GameOver)
		{
			SPAWNTIMER.Stop();
		}
	}
}
