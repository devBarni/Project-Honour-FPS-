using Godot;

namespace Godotype2FPS.Scripts;

public partial class Player : CharacterBody3D
{
	// State
	private const float SPEED = 5.0f;
	private const float MOUSE_SENS = 0.5f;
	
	// References for nodes
	private AnimatedSprite2D _animatedSprite2D;
	private RayCast3D _rayCast3D;
	private AudioStreamPlayer _shootSound;
	private Control _deathScreen;
	private Button _restartButton;
	
	// Player's state
	private bool _canShoot = true;
	private bool _dead = false;

	public override void _Ready()
	{
		//Mouse's config.
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		// ... Nodes
		_animatedSprite2D = GetNode<AnimatedSprite2D>("%GunAnimated");
		_rayCast3D = GetNode<RayCast3D>("RayCast3D");
		_shootSound = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
		_deathScreen = GetNode<Control>("CanvasLayer/DeathScreen");
		_restartButton = GetNode<Button>("CanvasLayer/DeathScreen/Panel/Button");

		_animatedSprite2D.AnimationFinished += ShootAnimDone;
		_restartButton.ButtonUp += Restart;
	}

	// Mouse movement
	public override void _Input(InputEvent @event)
	{
		if (_dead) return;

		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotationDegrees = new Vector3(
				RotationDegrees.X,
				RotationDegrees.Y - mouseMotion.Relative.X * MOUSE_SENS,
				RotationDegrees.Z
			);
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("exit"))
		{
			GetTree().Quit();
		}

		if (Input.IsActionJustPressed("restart"))
		{
			Restart();
		}
		
		if (_dead) return;

		if (Input.IsActionJustPressed("shoot"))
		{
			Shoot();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		//Movement
		if (_dead) return;
		
		// Get the input direction and handle player movement.
		var inputDir = Input.GetVector("move_left", "move_right", "move_forwards", "move_backwards");
		var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		if (direction != Vector3.Zero)
		{
			Velocity = new Vector3(
				direction.X * SPEED,
				Velocity.Y,
				direction.Z * SPEED);
		}
		else
		{
			Velocity = new Vector3(
				Mathf.MoveToward(Velocity.X, 0, SPEED),
				Velocity.Y,
				Mathf.MoveToward(Velocity.Z, 0, SPEED));
		}
		
		MoveAndSlide();
	}

	private void Restart()
	{
		GetTree().ReloadCurrentScene();
	}

	private void Shoot()
	{
		// Shooting mechanics.
		if (!_canShoot) return;

		_canShoot = false;
		_animatedSprite2D.Stop();
		_animatedSprite2D.Play("shoot");
		_shootSound.Play();

		if (!_rayCast3D.IsColliding()) return;
		var  collider = _rayCast3D.GetCollider();
		if (collider is Node node && node.HasMethod("Kill"))
		{
			node.Call("Kill");
		}
	}

	private void ShootAnimDone()
	{
		_canShoot = true;
	}

	private void Kill()
	{
		// What happens when a player dies
		_dead = true;
		_deathScreen.Show();
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
}