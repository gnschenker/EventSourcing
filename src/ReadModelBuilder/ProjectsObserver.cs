using System;
using System.Threading.Tasks;
using EventSourcing.Contracts.Events;

namespace ReadModelBuilder
{
    public class ProjectsObserver
    {
        readonly IProjectionWriter<Guid, ProjectsView> _writer;

        public ProjectsObserver(IProjectionWriter<Guid, ProjectsView> writer)
        {
            _writer = writer;
        }

        public async Task When(ProjectAdded e)
        {
            await _writer.Add(new ProjectsView {Id = e.Id, Name = e.Name});
        }

        public async Task When(PmToProjectAssigned e)
        {
            await _writer.Update(e.Id, x => x.PmId = e.PmId);
        }
    }
}