namespace WorkflowEngine.Models;

public class Action{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public bool Enabled { get; init; } = true;
    public List<string> FromStates { get; init; } = new();
    public string ToState { get; init; } = null!;
}