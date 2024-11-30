using Godot;

namespace Godotype2FPS.Scripts;

[Tool]
public partial class CustomTextureButton : Control
{
    [Signal]
    public delegate void SignalClickedEventHandler(); // Definicja sygnału w C# musi kończyś się na "EventHandler"

    // Exported variables with default values
    [ExportCategory("Settings Menu")]
    [Export] private string Text = "Text Button";

    // Nodes references
    private Label _label;
    private AnimatedSprite2D _arrowLeft;
    private AnimatedSprite2D _arrowRight;
    private AudioStreamPlayer _audio;

    public override void _Ready()
    {
        // Get node references
        _label = GetNode<Label>("%button_text");
        _arrowLeft = GetNode<AnimatedSprite2D>("%arrow_left");
        _arrowRight = GetNode<AnimatedSprite2D>("%arrow_right");
        _audio = GetNode<AudioStreamPlayer>("%audio_stream_player");

        SetupText();
        HideArrow();
    }

    public override void _Process(double delta)
    {
        // Update in editor mode
        if (Engine.IsEditorHint())
        {
            SetupText();
            ShowArrow();
        }
    }

    private void SetupText()
    {
        // Update button text
        _label.Text = Text;
    }

    private void ShowArrow()
    {
        AnimatedSprite2D[] arrows = { _arrowLeft, _arrowRight };

        foreach (var arrow in arrows)
        {
            if (arrow == null) {
                continue;
            }

            arrow.Visible = true;
        }
    }

    private void HideArrow()
    {
        AnimatedSprite2D[] arrows = { _arrowLeft, _arrowRight };

        foreach (var arrow in arrows)
        {
            if (arrow == null) {
                continue;
            }

            arrow.Visible = false;
        }
    }

    // Signal methods
    private void _on_button_pressed()
    {
        _audio.Play();
        EmitSignal(SignalName.SignalClicked); // Wciskając guzik, emitujemy własny sygnał który zostanie obsłużony w MainMenu.cs
    }

    private void _on_button_mouse_entered()
    {
        ShowArrow();
    }

    private void _on_button_mouse_exited()
    {
        HideArrow();
    }
}