using System;
using System.Collections.Generic;

namespace ReadModelBuilder
{
    public class ObserverRegistry
    {
        public IEnumerable<object> GetObservers(IProjectionWriterFactory factory)
        {
            yield return new ProjectsObserver(factory.GetProjectionWriter<Guid, ProjectsView>());
        } 
    }
}