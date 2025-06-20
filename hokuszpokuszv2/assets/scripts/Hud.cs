using Godot;
using System;

public partial class Hud : CanvasLayer
{
	// Start
	public void OnStartButtonPress()
	{
		GlobalData.GameData._gameState = GlobalData.GameState.Playing;
		GetNode<Timer>("../Hokusz/SpawnTimer").Start();
	}

	// Retry
	public void OnRetryButtonPress()
	{ GlobalData.GameData._gameState = GlobalData.GameState.Menu; }

	// Update Score Labels
	public void UpdateScore()
	{
		var scoreLabel = GetNode<Label>("Playing/Score");
		scoreLabel.Text = $"Score: {GlobalData.GameData._score}";
		var hiScoreLabel = GetNode<Label>("Playing/HiScore");
		hiScoreLabel.Text = $"Hi-Score: {GlobalData.GameData._hiScore}";
	}

	// HUD Updates
	public override void _Process(double delta)
	{
		var gameState = GlobalData.GameData._gameState;

		var MenuLayer = GetNode<CanvasLayer>("Menu");
		var PlayingLayer = GetNode<CanvasLayer>("Playing");
		var GameOverLayer = GetNode<CanvasLayer>("GameOver");

		if (gameState == GlobalData.GameState.Menu)
		{
			MenuLayer.Show();
			PlayingLayer.Hide();
			GameOverLayer.Hide();
		}
		else if (gameState == GlobalData.GameState.Playing)
		{
			MenuLayer.Hide();
			PlayingLayer.Show();
			GameOverLayer.Hide();
			UpdateScore();
		}
		else if (gameState == GlobalData.GameState.GameOver)
		{
			MenuLayer.Hide();
			PlayingLayer.Hide();
			GameOverLayer.Show();
		}
	}
}
