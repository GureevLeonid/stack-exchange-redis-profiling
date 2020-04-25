using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisProfiling
{
	public interface IRedisConnectionPool
	{
		Task<ConnectionMultiplexer> GetConnection();
	}
}