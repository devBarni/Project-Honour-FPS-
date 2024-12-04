/* using Godot;
using System;

public partial class AmmoPickup : Area3D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			AudioController.PlayAmmoSnd(); // Ammo pickup sound
			PlayerStats.ChangePistolAmmo(10); // Add that amount of ammo for player
			body.Call("show_pick_overlay");
			QueueFree();
		}
	}
} */
