using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

namespace Godot;

[GlobalClass]
public partial class ComboManager : Node
{
    public delegate void ComboHandler(Combo combo);

    public event ComboHandler ComboAvailable;

    [Export] private InputHandler _inputHandler;
    [Export] private float _moveInputInterval = 0.25f;
    [Export] private Array<Combo> _combos;

    private List<MoveInputInfo> _moveInputLog = new();
    private Combo _currentCombo;

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
        var logSize = _moveInputLog.Count;
        for (var i = 0; i < logSize; ++i) {
            var moveKeys = _moveInputLog.GetRange(i, logSize - i).Select(x => x.MoveKey).ToArray();
            Combo retCombo = FindCombo(moveKeys);
            if (retCombo is not null) {
                return retCombo;
            }
        }

        return null;
    }

    private void OnMoveInputUpdate(MoveInputInfo miInfo) {
        if (!miInfo.IsPressed) {
            return;
        }

        if (_moveInputLog.Count != 0 && miInfo.TimeMsec - _moveInputLog[^1].TimeMsec > _moveInputInterval) {
            _moveInputLog.Clear();
        }

        _moveInputLog.Add(miInfo);

        Combo availableCombo = GetAvailableCombo();
        if (availableCombo is not null) {
            ComboAvailable?.Invoke(availableCombo);
        }
    }

    public Combo GetComboOrNull(StringName name) {
        return _combos.Single(x => x.Name == name);
    }
}