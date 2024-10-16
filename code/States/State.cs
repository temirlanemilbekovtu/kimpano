namespace Godot;

[GlobalClass]
public abstract partial class State : Node
{
    [Export] private bool _isInitial;

    protected StateMachine                  MyStateMachine;
    protected ComboManager                  MyComboManager;
    protected ExtendedAnimationPlayer       MyAnimationPlayer;

    public bool IsInitial { 
                get => _isInitial; 
        private set => _isInitial = value;
    }

    public override void _EnterTree() {
        MyStateMachine = GetParentOrNull<StateMachine>();
        if (MyStateMachine is null) {
            GD.Print("Parent isn't StateMachine type, free queued.");
            QueueFree();
            return;
        }

        MyAnimationPlayer = MyStateMachine.ExtAnimPlayer;
        if (MyAnimationPlayer is null) {
            GD.Print("Couldn't get AnimationPlayer, free queued.");
            QueueFree();
            return;
        }

        MyStateMachine.AddState(this);
    }

    public virtual void HandleInput(MoveInputInfo moveInputInfo) {
    }

    public virtual void HandleCombo(Combo combo) {
    }

    public virtual void HandleDamage() {
    }

    public virtual void StateProcess(double delta) {
    }

    public virtual void StatePhysicsProcess(double delta) {
    }

    public virtual void StateEntry(string[] args) {
    }

    public virtual void StateExit() {
    }
}