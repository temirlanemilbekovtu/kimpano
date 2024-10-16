using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

namespace Godot;

[GlobalClass]
public partial class ComboManager : Node
{
    public delegate void ComboHandler(Combo combo);
    public event ComboHandler ComboAvailable;

    [Export] private InputHandler   _inputHandler;
    [Export] private float          _moveInputInterval = 0.25f;
    [Export] private Array<Combo>   _combos;

    private List<MoveInputInfo>     _moveInputLog = new();
    private Combo                   _currentCombo;

    public override void _Ready() {
        if (_inputHandler is null) {
            GD.Print("InputHandler isn't set. Free queued.");
            QueueFree();
            return;
        }

        _inputHandler.MoveInputUpdate += OnMoveInputUpdate;
    }

    private Combo FindCombo(MoveKey[] moveList) {
        return _combos.FirstOrDefault(combo => combo.MoveList.Equals(moveList));
    }

    private Combo GetAvailableCombo() {
        var moveInputLogSize = _moveInputLog.Count;
        for (var i = 0; i < moveInputLogSize; ++i) {
            var lastMoveKeys = _moveInputLog.GetRange(i, moveInputLogSize - i).Select(x => x.MoveKey).ToArray();
            Combo result = FindCombo(lastMoveKeys);
            if (result is not null) {
                return result;
            }
        }

        return null;
    }

    private void OnMoveInputUpdate(MoveInputInfo moveInputInfo) {
        if (!moveInputInfo.IsPressed) {
            return;
        }

        if (_moveInputLog.Count != 0 && moveInputInfo.TimeMsec - _moveInputLog[^1].TimeMsec > _moveInputInterval) {
            _moveInputLog.Clear();
        }

        _moveInputLog.Add(moveInputInfo);

        Combo availableCombo = GetAvailableCombo();
        if (availableCombo is not null) {
            ComboAvailable?.Invoke(availableCombo);
        }
    }

    public Combo FindComboOrNull(StringName name) {
        return _combos.Single(x => x.Name == name);
    }
}