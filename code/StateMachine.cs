using System.Collections.Generic;

namespace Godot;

[GlobalClass]
public partial class StateMachine : Node
{
    [Export] private InputHandler               _inputHandler;
    [Export] private ComboManager               _comboManager;
    [Export] private AnimationPlayer            _animPlayer;
    [Export] private Health                     _health;

    private List<State>     _states = new();
    private State           _initialState;
    private State           _currentState;
    
    public AnimationPlayer AnimPlayer {
        get => _animPlayer;
    }

    public override void _Ready() {
        if (_inputHandler is null) {
            GD.Print("InputHandler is missing. Free queued.");
            QueueFree();
            return;
        }

        if (_comboManager is null) {
            GD.Print("ComboManager is missing. Free queued.");
            QueueFree();
            return;
        }

        if (_health is null) {
            GD.Print("Health is missing. Free Queued.");
            QueueFree();
            return;
        }

        if (_states.Count == 0) {
            GD.Print("The state list is empty on ready. Free queued.");
            QueueFree();
            return;
        }
        
        if (_initialState is null) {
            GD.Print("The initial state is missing on ready. Free queued.");
            QueueFree();
            return;
        }

        _inputHandler.MoveInputUpdate += OnMoveInputUpdate;
        _comboManager.ComboAvailable += OnComboAvailable;
        _health.Damaged += OnDamaged;

        _currentState = _initialState;
        _currentState.StateEntry(new string[] { });
    }

    public void AddState(State state) {
        if (IsInstanceValid(state)) {
            _states.Add(state);
            _initialState = state.IsInitial ? state : _initialState;
        }
    }

    public void SwitchState(State state, string[] args) {
        _currentState.StateExit();

        if (IsInstanceValid(state) && _states.Contains(state)) {
            _currentState = state;
            _currentState.StateEntry(args);
        } else {
            _currentState = _initialState;
            _currentState!.StateEntry(args);
        }
    }

    public override void _Process(double delta) {
        _currentState?.StateProcess(delta);
    }

    public override void _PhysicsProcess(double delta) {
        _currentState?.StatePhysicsProcess(delta);
    }

    private void OnMoveInputUpdate(MoveInputInfo moveInputInfo) {
        _currentState?.HandleInput(moveInputInfo);
    }

    private void OnComboAvailable(Combo combo) {
        _currentState?.HandleCombo(combo);
    }

    private void OnDamaged() {
        _currentState?.HandleDamage();
    }
}