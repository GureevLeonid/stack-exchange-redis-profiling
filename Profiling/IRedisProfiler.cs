using StackExchange.Redis.Profiling;

namespace RedisProfiling
{
	public interface IRedisProfiler
	{
		ProfilingSession GetSession();

		IRedisProfilingSession Profile(string session);

		void ResetSession();

		ProfilingSession StartSession();
	}
}