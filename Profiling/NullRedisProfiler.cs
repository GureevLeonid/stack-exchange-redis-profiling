using StackExchange.Redis.Profiling;

namespace RedisProfiling.Controllers
{
	public class NullRedisProfiler : IRedisProfiler  
	{
		public ProfilingSession GetSession()
		{
			return null;
		}

		public IRedisProfilingSession Profile(string session)
		{
			return NullRedisProfilingSession.Instance;
		}

		public void ResetSession()
		{
			// empty
		}

		public ProfilingSession StartSession()
		{
			return null;
		}
	}
}