using Godot;
using System;

public partial class PlayerStats : Node
{
	// Player variables.
	private float health = 100f;
	private float max_health = 100f;
	private float armor = 0f;
	private float max_armor = 100f;
	private bool is_alive = true;

	private float ammo_pistol = 50f;
	private float ammo_shotgun = 0f;
	private float ammo_rocket = 0f;

	private float ammo_max_pistol = 200;
	private float ammo_max_shotgun = 0f;
	private float ammo_max_rocket = 0f;

	private bool game_ready = false;

}
