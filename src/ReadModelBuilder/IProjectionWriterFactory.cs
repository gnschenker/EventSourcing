namespace ReadModelBuilder
{
    public interface IProjectionWriterFactory
    {
        IProjectionWriter<TId, TView> GetProjectionWriter<TId, TView>() where TView: class;
    }
}