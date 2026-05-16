public readonly struct BattlePowerEntry
{
    public BattlePowerEntry(string label, int value)
    {
        Label = label;
        Value = value;
    }

    public string Label { get; }
    public int Value { get; }
}
