using System;

namespace ReadModelBuilder
{
    public class MongoDbAtomicWriterFactory : IProjectionWriterFactory
    {
        private readonly string _databaseName;
        private readonly string _connectionString;

        public MongoDbAtomicWriterFactory(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public IProjectionWriter<TId, TView> GetProjectionWriter<TId, TView>() where TView: class
        {
            return new MongoDbProjectionWriter<TId, TView>(_connectionString, _databaseName);
        }
    }
}