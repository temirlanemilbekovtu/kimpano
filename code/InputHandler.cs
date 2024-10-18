using Godot.Collections;
using System;
using Generic = System.Collections.Generic;

namespace Godot;

[GlobalClass]
public partial class InputHandler : Node
{
    public delegate void MoveInputHandler(MoveInputInfo moveInputInfo);

    public event MoveInputHandler MoveInputUpdate;

    [Export] private Array<Key>         _keys = new();
    [Export] private Array<MoveKey>     _moveKeys = new();

    private Generic.Dictionary<Key, MoveKey>    _inputMap = new();

    public override void _Input(InputEvent @event) {
        if (@event.IsEcho() || @event is not InputEventKey eventKey) {
            return;
        }

        Key key = eventKey.Keycode;
        if (_inputMap.ContainsKey(key)) {
            return;
        }

        MoveInputInfo newMoveInputInfo = new(_inputMap[key], eventKey.IsPressed(), Time.GetTicksMsec());
        MoveInputUpdate?.Invoke(newMoveInputInfo);
    }

    public override void _Ready() {
        for (var i = 0; i < _keys.Count && i < _moveKeys.Count; ++i) {
            _inputMap.Add(_keys[i], _moveKeys[i]);
        }
    }

    public static Direction MoveKeyToDirection(MoveKey moveKey) {
        switch (moveKey) {
            case MoveKey.Back:
                return Direction.Backward;
            case MoveKey.Front:
                return Direction.Forward;
            default:
                return Direction.None;
        }
    }

    public static Direction GetOppositeDirection(Direction dir) {
        switch(dir) {
            case Direction.Forward:
                return Direction.Backward;
            case Direction.Backward:
                return Direction.Forward;
            default:
                return Direction.None;
        }
    }
}

public enum MoveKey
{
    None,
    Back,
    Up,
    Front,
    Down,
    LowKick,
    MidKick,
    HighKick,
    Block
}

public enum Direction
{
    None = 0,
    Forward = -1,
    Backward = 1
}