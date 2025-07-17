using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public class WorkflowService
{
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    // Definitions
    public void CreateDefinition(WorkflowDefinition def)
    {
        def.Validate();
        _definitions[def.Id] = def;
    }
    public IEnumerable<WorkflowDefinition> GetAllDefinitions() => _definitions.Values;
    public WorkflowDefinition? GetDefinition(string id) =>
        _definitions.TryGetValue(id, out var d) ? d : null;

    // Instances
    public WorkflowInstance StartInstance(string workflowId)
    {
        var def = GetDefinition(workflowId) ?? throw new Exception("Definition not found.");
        var initial = def.States.Find(s => s.IsInitial) ?? throw new Exception("No initial state.");
        var inst = new WorkflowInstance(workflowId, initial.Id);
        _instances[inst.Id] = inst;
        return inst;
    }
    public IEnumerable<WorkflowInstance> GetAllInstances() => _instances.Values;
    public WorkflowInstance? GetInstance(string id) =>
        _instances.TryGetValue(id, out var inst) ? inst : null;

    // Action execution
    public WorkflowInstance? ExecuteAction(string instanceId, string actionId)
    {
        if (!_instances.TryGetValue(instanceId, out var inst)) return null;
        var def = GetDefinition(inst.WorkflowId)!;
        def.Validate();

        var act = def.Actions.Find(a => a.Id == actionId)
                  ?? throw new Exception("Action not found.");
        if (!act.Enabled) throw new Exception("Action disabled.");
        if (!act.FromStates.Contains(inst.CurrentState))
            throw new Exception("Action not allowed from current state.");

        inst.CurrentState = act.ToState;
        
        inst.History.Add(new ActionRecord {
            ActionId = actionId,
            Timestamp = DateTime.UtcNow
        });

        return inst;
    }
}