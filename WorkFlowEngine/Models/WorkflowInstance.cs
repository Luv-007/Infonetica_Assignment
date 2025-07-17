namespace WorkflowEngine.Models;

public class WorkflowInstance{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkflowId { get; set; } = null!;
    public string CurrentState { get; set; } = null!;

    // Use ActionRecord instead of tuple
    public List<ActionRecord> History { get; set; } = new();

    public WorkflowInstance(string workflowId, string startState)
    {
        WorkflowId = workflowId;
        CurrentState = startState;
    }
}