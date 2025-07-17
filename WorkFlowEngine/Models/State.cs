namespace WorkflowEngine.Models;

public class State{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public bool IsInitial { get; init; } = false;
    public bool IsFinal { get; init; } = false;
    public bool Enabled { get; init; } = true;
}