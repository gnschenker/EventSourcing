using System;
using System.Threading.Tasks;

namespace ReadModelBuilder
{
    public interface IProjectionWriter<in TId, TView>
    {
        Task Add(TView item);
        Task Update(TId id, Action<TView> update);
    }
}
