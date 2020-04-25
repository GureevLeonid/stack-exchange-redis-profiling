using System.Threading.Tasks;

namespace RedisProfiling
{
	public class NullRedisProfilingSession : IRedisProfilingSession
	{
		private NullRedisProfilingSession()
		{
		}

		public static NullRedisProfilingSession Instance { get; } = new NullRedisProfilingSession();

		public ValueTask DisposeAsync()
		{
			return ValueTasksExtensions.EmptyValueTask;
		}
	}
}