using System;
using System.Threading.Tasks;
using Nito.AsyncEx;
using StackExchange.Redis;

namespace RedisProfiling
{
	public class SingleRedisConnectionPool : IRedisConnectionPool
	{
		private const string ConnectionString = "127.0.0.1:6379,defaultDatabase=0";

		private static readonly AsyncLazy<ConnectionMultiplexer> connection = new AsyncLazy<ConnectionMultiplexer>(
			() => ConnectionMultiplexer.ConnectAsync(ConnectionString));

		private readonly IRedisProfiler _profiler;

		public SingleRedisConnectionPool(IRedisProfiler profiler)
		{
			_profiler = profiler ?? throw new ArgumentNullException(nameof(profiler));
		}

		public async Task<ConnectionMultiplexer> GetConnection()
		{
			var result = await connection;
			result.RegisterProfiler(() => _profiler.GetSession());
			return result;
		}
	}
}