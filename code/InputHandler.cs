using Godot.Collections;
using System;
using Generic = System.Collections.Generic;

namespace Godot;

[GlobalClass]
public partial class InputHandler : Node
{
    public delegate void MoveInputHandler(MoveInputInfo moveInputInfo);

    public event MoveInputHandler MoveInputUpdate;

    [Export] private Array<Key> _keys = new();
    [Export] private Array<MoveKey> _moveKeys = new();

    private Generic.Dictionary<Key, MoveKey> _inputMap = new();
    private Generic.List<MoveInputInfo> _moveLog = new();

    public override void _Input(InputEvent @event) {
        if (@event.IsEcho() || @event is not InputEventKey eventKey) {
            return;
        }

        Key key = eventKey.Keycode;
        if (_inputMap.ContainsKey(key)) {
            return;
        }

        MoveInputInfo newMiInfo = new(_inputMap[key], eventKey.IsPressed(), Time.GetTicksMsec());
        _moveLog.Add(newMiInfo);
        MoveInputUpdate?.Invoke(newMiInfo);
    }

    public override void _Ready() {
        for (var i = 0; i < _keys.Count && i < _moveKeys.Count; ++i) {
            _inputMap.Add(_keys[i], _moveKeys[i]);
        }
    }

    /*
    public bool TryGetLog(int count, out MoveInputInfo[] targetLog) {
        targetLog = null;
        int logSize = _moveLog.Count;
        if (count == 0 || logSize < Math.Abs(count)) {
            return false;
        }

        targetLog = count < 0 ? _moveLog.GetRange(logSize - count, count).ToArray() : _moveLog.GetRange(0, count).ToArray();
        return true;
    }
    */
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