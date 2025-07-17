namespace WorkflowEngine.Models;

public class WorkflowDefinition{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<State> States { get; set; } = new();
    public List<Action> Actions { get; set; } = new();

    public void Validate()
    {
        if (States.Count(s => s.IsInitial) != 1)
            throw new Exception("Exactly one initial state required.");
        var stateIds = States.Select(s => s.Id).ToList();
        if (stateIds.Distinct().Count() != stateIds.Count)
            throw new Exception("Duplicate state IDs.");
        var actionIds = Actions.Select(a => a.Id).ToList();
        if (actionIds.Distinct().Count() != actionIds.Count)
            throw new Exception("Duplicate action IDs.");
        foreach (var a in Actions)
        {
            if (!a.FromStates.All(stateIds.Contains))
                throw new Exception($"Action '{a.Id}' has invalid fromStates.");
            if (!stateIds.Contains(a.ToState))
                throw new Exception($"Action '{a.Id}' targets unknown state.");
        }
    }
}