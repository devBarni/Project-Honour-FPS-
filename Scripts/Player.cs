using Godot;

public partial class Player : CharacterBody3D
{
    // Configuration constants
    private const float SPEED = 5.0f;
    private const float SHIFT_SPEED = 10.0f;
    private const float MOUSE_SENS = 0.5f;
    
    // Node references
    private AnimatedSprite2D _animatedSprite2D; 
    private RayCast3D _rayCast3D;
    private AudioStreamPlayer _shootSound;
    private Control _deathScreen;
    private Button _restartButton;
    
    // Player state
    private bool _canShoot = true;
    private bool _dead = false;

    // Initialization method
    public override void _Ready()
    {
        // Mouse configuration
        Input.MouseMode = Input.MouseModeEnum.Captured;
       
        // Get scene nodes
        _animatedSprite2D = GetNode<AnimatedSprite2D>("%GunAnimated");
        _rayCast3D = GetNode<RayCast3D>("RayCast3D");
        _shootSound = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _deathScreen = GetNode<Control>("CanvasLayer/DeathScreen");
        _restartButton = GetNode<Button>("CanvasLayer/DeathScreen/Panel/Button");

        // Connect events
        _animatedSprite2D.AnimationFinished += ShootAnimDone;
        _restartButton.ButtonUp += Restart;
    }

    // Mouse input handling
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

    // Main event processing method
    public override void _Process(double delta)
    {
        // Global input handling
        HandleGlobalInputs(delta);
    }

    // Extracted method for global input handling
    private void HandleGlobalInputs(double delta)
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

    // Physics-based movement processing
    public override void _PhysicsProcess(double delta)
    {
        if (_dead) return;

        ProcessPlayerMovement(delta);
    }

    // Extracted method for player movement logic
    private void ProcessPlayerMovement(double delta)
    {
        // Get movement direction
        var inputDir = Input.GetVector("move_left", "move_right", "move_forwards", "move_backwards");
        var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        // Determine current speed
        float currentSpeed = Input.IsActionPressed("sprint") ? SHIFT_SPEED : SPEED;

        // Calculate velocity
        if (direction != Vector3.Zero)
        {
            Velocity = new Vector3(
                direction.X * currentSpeed,
                Velocity.Y,
                direction.Z * currentSpeed);
        }
        else
        {
            Velocity = new Vector3(
                Mathf.MoveToward(Velocity.X, 0, currentSpeed),
                Velocity.Y,
                Mathf.MoveToward(Velocity.Z, 0, currentSpeed));
        }

        MoveAndSlide();
    }

    // Restart the current scene
    private void Restart()
    {
        GetTree().ReloadCurrentScene();
    }

    // Shooting mechanism
    private void Shoot()
    {
        // Check if shooting is possible
        if (!_canShoot) return;

        _canShoot = false;
        _animatedSprite2D.Stop();
        _animatedSprite2D.Play("shoot");
        _shootSound.Play();

        // Check for hit detection
        if (!_rayCast3D.IsColliding()) return;
        var collider = _rayCast3D.GetCollider();
        if (collider is Node node && node.HasMethod("Kill"))
        {
            node.Call("Kill");
        }
    }

    // Callback for shoot animation completion
    private void ShootAnimDone()
    {
        _canShoot = true;
    }

    // Player death method
    private void Kill()
    {
        _dead = true;
        _deathScreen.Show();
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}