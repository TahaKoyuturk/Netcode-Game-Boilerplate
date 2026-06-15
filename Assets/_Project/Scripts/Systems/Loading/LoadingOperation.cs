using System;
using System.Threading.Tasks;

namespace Studio.Systems.Loading
{
    public sealed class LoadingOperation
    {
        public string Id { get; }
        public Func<IProgress<float>, Task> ExecuteAsync { get; }

        public LoadingOperation(string id, Func<IProgress<float>, Task> executeAsync)
        {
            Id = id;
            ExecuteAsync = executeAsync;
        }
    }
}
