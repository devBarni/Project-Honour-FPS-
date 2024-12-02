using Godot;
using System;

public partial class PauseMenu : Node
{
	
	// Signals
	public event EventHandler PauseSignal;
	public event EventHandler UnpauseSignal;

	// References for nodes.
	private AudioStreamPlayer _buttonSound;

	public override void _Ready()
	{
		
		_buttonSound = GetNode<AudioStreamPlayer>("Button_snd");
		
	}
	
}
