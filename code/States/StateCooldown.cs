namespace Godot;

[GlobalClass]
public partial class StateCooldown : State
{
	[ExportGroup("Transitions")]
	[Export] private StateIdle  _idle;
	[Export] private StateDeath _death;

	public override void StateEntry(string[] args) {
		MyAnimationPlayer.AnimationFinished -= OnAnimEvent;
		MyAnimationPlayer.AnimationFinished += OnAnimEvent;
	}

	public override void HandleDamage() {
		MyStateMachine.SwitchState(_death, new string[] { });
	}

	public override void StateExit() {
		MyAnimationPlayer.AnimationFinished -= OnAnimEvent;
	}

	private void OnAnimEvent(StringName animName) {
		MyStateMachine.SwitchState(_idle, new string[] { });
	}
}