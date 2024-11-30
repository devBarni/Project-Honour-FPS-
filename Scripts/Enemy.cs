using Godot;

namespace Godotype2FPS.Scripts;

public partial class Enemy : CharacterBody3D
{
    // Reference to the 3D animated sprite for death amimation.
    private AnimatedSprite3D _animatedSprite3D;
    
    // Exported variables to allow configuration in the Godot
    [ExportCategory("Attributes")] [Export(PropertyHint.Range, "0,10, 0.5")]
    private float _moveSpeed = 2.0f;

    [Export(PropertyHint.Range, "0,10, 0.5")]
    private float _attackRange = 1.0f;
    
    // Reference to the player node
    private CharacterBody3D _player;
    
    private bool _dead = false;

    public override void _Ready()
    {
        // Find the player node in the "player" group
        _player = GetTree().GetFirstNodeInGroup("player") as CharacterBody3D;
        
        // Get reference to the death sound and animated sprite.
        _animatedSprite3D = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
    }

    public override void _PhysicsProcess(double delta)
    {
        
        // if enemy is dead or player is not found, stop processing
        if (_dead || _player == null) return;
        
        // Calculate direction to player, ignoring vertical difference
        var dir = _player.GlobalPosition - GlobalPosition;
        dir.Y = 0.0f;
        dir = dir.Normalized();
        
        // Move towards the player at defined speed
        Velocity = dir * _moveSpeed;
        MoveAndSlide();
        
        
        // Attempt to kill player if in range
        AttemptToKillPlayer();
    }

    private void AttemptToKillPlayer()
    {
        // Calculate distance to player.
        var distToPlayer = GlobalPosition.DistanceTo(_player.GlobalPosition);

        // If player is outside attack range, do nothing
        if (distToPlayer > _attackRange) return;
        
        // If player is outside attack range, do nothing;
        var eyeLine = Vector3.Up * 1.5f;
        
        // Create ray query parameters to check line of sight.
        var query = PhysicsRayQueryParameters3D.Create(
            GlobalPosition + eyeLine,
            _player.GlobalPosition + eyeLine,
            1);
        
        // Perform ray cast to check for obstacles
        var resultObstacles = GetWorld3D().DirectSpaceState.IntersectRay(query);

        if (resultObstacles.Count == 0)
        {
            _player.Call("Kill");
        }
    }
    
    // Method to handle enemy's death
    private void Kill()
    {
        _dead = true;
        
        GetNode<AudioStreamPlayer3D>("DeathSound").Play();
        
        // Play death animation.
        _animatedSprite3D.Play("death");
        
        // Disable collision

        GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
    }
}