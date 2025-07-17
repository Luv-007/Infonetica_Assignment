using WorkflowEngine.Models;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WorkflowService>();
var app = builder.Build();

// Workflows
app.MapPost("/workflows", (WorkflowDefinition def, WorkflowService svc) =>
{
    svc.CreateDefinition(def);
    return Results.Created($"/workflows/{def.Id}", def);
});
app.MapGet("/workflows", (WorkflowService svc) => Results.Ok(svc.GetAllDefinitions()));
app.MapGet("/workflows/{id}", (string id, WorkflowService svc) =>
    svc.GetDefinition(id) is { } d ? Results.Ok(d) : Results.NotFound());
app.MapGet("/workflows/{id}/states", (string id, WorkflowService svc) =>
    svc.GetDefinition(id) is { } d ? Results.Ok(d.States) : Results.NotFound());
app.MapGet("/workflows/{id}/actions", (string id, WorkflowService svc) =>
    svc.GetDefinition(id) is { } d ? Results.Ok(d.Actions) : Results.NotFound());

// Instances
app.MapPost("/instances", (StartInstanceRequest req, WorkflowService svc) =>
{
    var inst = svc.StartInstance(req.WorkflowId);
    return Results.Created($"/instances/{inst.Id}", inst);
});
app.MapGet("/instances", (WorkflowService svc) => Results.Ok(svc.GetAllInstances()));
app.MapGet("/instances/{id}", (string id, WorkflowService svc) =>
    svc.GetInstance(id) is { } inst ? Results.Ok(inst) : Results.NotFound());
app.MapPost("/instances/{id}/actions/{actionId}", (string id, string actionId, WorkflowService svc) =>
    svc.ExecuteAction(id, actionId) is { } updated ? Results.Ok(updated) : Results.BadRequest());
    
app.MapGet("/", () => Results.Ok("✅ Workflow Engine is up and running. Try /workflows"));

app.Run();

public record StartInstanceRequest(string WorkflowId);