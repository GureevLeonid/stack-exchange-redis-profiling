using System.Threading.Tasks;
using StackExchange.Redis.Profiling;

namespace RedisProfiling
{
	public interface IRedisProfilingMetrics
	{
		ValueTask Collect(string session, IProfiledCommand command);
	}
}