namespace Godot;

[GlobalClass]
public partial class StateCooldown : State
{
	[ExportGroup("Transitions")]
	[Export] private StateIdle  _idle;
	[Export] private StateDeath _death;

	public override void StateEntry(string[] args) {
		Combo combo = args.Length == 0 ? null : MyComboManager.FindComboOrNull(args[0]);
		if (combo is null) {
			MyStateMachine.SwitchState(_idle, new string[] { });
			return;
		}
		
		MyAnimationPlayer.Play(combo.CooldownAnimName);
		MyAnimationPlayer.AnimationFinished += OnAnimationFinished;
	}

	public override void HandleDamage() {
		MyStateMachine.SwitchState(_death, new string[] { });
	}

	public override void StateExit() {
		MyAnimationPlayer.Stop();
		MyAnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}

	private void OnAnimationFinished(StringName animName) {
		MyStateMachine.SwitchState(_idle, new string[] { });
	}
}