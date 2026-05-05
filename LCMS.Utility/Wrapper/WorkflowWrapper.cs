using Galaxy.Workflow.Runtime.Application;
using Galaxy.Workflow.Runtime.Domain.Models;

namespace LCMS.Utility.Wrapper
{
    public class WorkflowWrapper : IWorkFlowWrapper
    {
        private readonly IWorkflowRuntimeInstanceService _workflowRuntimeInstanceService;
        private readonly IWorkflowRuntimeUow _workflowRuntimeUow;

        public WorkflowWrapper(IWorkflowRuntimeInstanceService workflowRuntimeInstanceService, IWorkflowRuntimeUow workflowRuntimeUow)
        {
            _workflowRuntimeInstanceService = workflowRuntimeInstanceService;
            _workflowRuntimeUow = workflowRuntimeUow;
        }

        public async Task<Workflows?> InitiateWorkflow(WorkflowParams workflowParams)
        {
            if (workflowParams is null) return new Workflows();
            var WorkFlowInstance = await _workflowRuntimeUow.WorkflowInstanceRepo.GetWorkflowInstanceByProcessType(workflowParams.ProcessTypeId, workflowParams.ProcessTypeValue, workflowParams.ProcessRefId);
            try
            {
                if (WorkFlowInstance == null)
                {
                    return await _workflowRuntimeInstanceService.InitiateWorkflow(workflowParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return default;
        }

        public async Task<(WorkflowElementStep, WorkflowElementStep)> CompleteWorkflowStep(WorkflowParams workflowParams)
        {
            if (workflowParams is null) return default;
            return await _workflowRuntimeInstanceService.CompleteWorkflowStep(workflowParams);
        }

        public async Task<WorkflowElementStep> SelfAssignWorkflowStep(WorkflowParams workflowParams)
        {
            if (workflowParams is null) return new WorkflowElementStep();
            return await _workflowRuntimeInstanceService.SelfAssignWorkflowStep(workflowParams);
        }

        public async Task<WorkflowElementStep> AssignWorkflowStepToUser(WorkflowParams workflowParams)
        {
            if (workflowParams is null) return new WorkflowElementStep();
            return await _workflowRuntimeInstanceService.AssignWorkflowStepToUser(workflowParams);
        }
        public async Task TerminateWorkflow(WorkflowParams workflowParams)
        {
            await _workflowRuntimeInstanceService.DeleteWorkFlow(workflowParams);
        }
    }
}
