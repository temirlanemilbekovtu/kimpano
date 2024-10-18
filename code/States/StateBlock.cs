namespace Godot;

[GlobalClass]
public partial class StateBlock : State
{
    [ExportGroup("Transitions")]
    [Export] private StateIdle _idleState;
    [Export] private StateDown _downState;

    public override void HandleInput(MoveInputInfo moveInputInfo) {
        if (!moveInputInfo.IsPressed && moveInputInfo.MoveKey == MoveKey.Block) {
            MyStateMachine.SwitchState(_idleState, new string[] { });
        }
    }

    public override void HandleDamage() {
        MyStateMachine.SwitchState(_downState, new string[] { });
    }
}