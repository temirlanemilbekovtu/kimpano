namespace Godot;

[GlobalClass]
public partial class StateDown : State
{
    [Export] private StringName _animationName;
    
    [ExportGroup("Transitions")] 
    [Export] private StateIdle _idle;
    [Export] private StateDeath _death;

    public override void HandleDamage() {
        MyStateMachine.SwitchState(_death, new string[] { });
    }
    
    public override void StateEntry(string[] args) {
        MyAnimationPlayer.Play(_animationName);
        MyAnimationPlayer.AnimationFinished -= OnAnimationFinished;
        MyAnimationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void StateExit() {
        MyAnimationPlayer.AnimationFinished -= OnAnimationFinished;
        MyAnimationPlayer.Stop();
    }

    private void OnAnimationFinished(StringName anim) {
        MyStateMachine.SwitchState(_idle, new string[] { });
    }
}