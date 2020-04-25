using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisProfiling.Controllers
{
	[ApiController]
	[Route("redis")]
	public class RedisController : ControllerBase
	{
		private readonly IRedisConnectionPool _redisConnectionPool;
		private readonly IRedisProfiler _redisProfiler;

		public RedisController(IRedisConnectionPool redisConnectionPool, IRedisProfiler redisProfiler)
		{
			_redisConnectionPool = redisConnectionPool ?? throw new ArgumentNullException(nameof(redisConnectionPool));
			_redisProfiler = redisProfiler ?? throw new ArgumentNullException(nameof(redisProfiler));
		}

		[HttpGet]
		[Route("profiler")]
		public async Task<IActionResult> Get()
		{
			var connection = await _redisConnectionPool.GetConnection();

			var database = connection.GetDatabase();

			var key = new RedisKey(Guid.NewGuid().ToString());
			var value = new RedisValue(Guid.NewGuid().ToString());
			var ttl = TimeSpan.FromSeconds(60);

			await using (_redisProfiler.Profile("StringSetAsync"))
			{
				await database.StringSetAsync(key, value, ttl);
			}

			string result;
			await using (_redisProfiler.Profile("StringGetAsync"))
			{
				result = await database.StringGetAsync(key);
			}

			return Ok(result);
		}
	}
}