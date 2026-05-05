using Galaxy.Workflow.Runtime.Domain.Models;

namespace LCMS.Utility
{
    public interface IWorkFlowWrapper
    {
        Task<Workflows?> InitiateWorkflow(WorkflowParams workflowParams);
        Task<(WorkflowElementStep, WorkflowElementStep)> CompleteWorkflowStep(WorkflowParams workflowParams);
        Task<WorkflowElementStep> SelfAssignWorkflowStep(WorkflowParams workflowParams);
        Task<WorkflowElementStep> AssignWorkflowStepToUser(WorkflowParams workflowParams);
        Task TerminateWorkflow(WorkflowParams workflowParams);
    }
}
