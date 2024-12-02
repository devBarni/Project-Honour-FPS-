using Godot;

namespace Godotype2FPS.Scripts;

public partial class MainMenu : Control
{
    // Referencje do nodeów
    private TextureButton _startButton;
    private AudioStreamPlayer _buttonSound;

    private CustomTextureButton _bntStart = null;
    private CustomTextureButton _bntLoad = null;
    private CustomTextureButton _bntOptions = null;
    private CustomTextureButton _bntQuit = null;

    public override void _Ready()
    {
        _bntStart   = GetNode<CustomTextureButton>("%btn_start");
        _bntLoad    = GetNode<CustomTextureButton>("%btn_load");
        _bntOptions = GetNode<CustomTextureButton>("%btn_options");
        _bntQuit    = GetNode<CustomTextureButton>("%btn_quit");

        // Połącz sygnały przycisków
        ConnectSignals();
    }

    private void ConnectSignals()
    {
        _bntStart.Connect(CustomTextureButton.SignalName.SignalClicked, new Callable(this, nameof(OnStartPressed)));
        _bntLoad.Connect(CustomTextureButton.SignalName.SignalClicked, new Callable(this, nameof(OnLoadPressed)));
        _bntOptions.Connect(CustomTextureButton.SignalName.SignalClicked, new Callable(this, nameof(OnOptionsPressed)));
        _bntQuit.Connect(CustomTextureButton.SignalName.SignalClicked, new Callable(this, nameof(OnQuitPressed)));
    }

    private void OnStartPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/World.tscn");
        GD.Print("OnStartPressed");
    }

    private void OnLoadPressed()
    {
        GD.Print("OnLoadPressed");
    }

    private void OnOptionsPressed()
    {
        GD.Print("OnOptionsPressed");
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
        GD.Print("OnQuitPressed");
    }
}