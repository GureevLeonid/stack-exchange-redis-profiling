using System;
using System.Threading.Tasks;
using StackExchange.Redis.Profiling;

namespace RedisProfiling
{
	public class RedisProfilingSession : IRedisProfilingSession
	{
		private readonly string _session;
		private readonly IRedisProfilingMetrics _metrics;
		private readonly ProfilingSession _profilingSession;
		private readonly IRedisProfiler _redisProfiler;

		public RedisProfilingSession(string session, IRedisProfilingMetrics metrics, IRedisProfiler redisProfiler)
		{
			if (string.IsNullOrWhiteSpace(session))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(session));

			_session = session;
			_metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
			_redisProfiler = redisProfiler ?? throw new ArgumentNullException(nameof(redisProfiler));

			try
			{
				_profilingSession = redisProfiler.StartSession();
			}
			catch (Exception e)
			{
				// todo: to log exception
				_profilingSession = null;
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_profilingSession == null)
				return;

			try
			{
				var profiledCommands = _profilingSession.FinishProfiling();
				foreach (var profiledCommand in profiledCommands)
				{
					await _metrics.Collect(_session, profiledCommand);
				}
			}
			catch (Exception e)
			{
				// todo: to log exceptions here.
				// todo: we don't want to rethrow exceptions in profiler. 
			}
			finally
			{
				_redisProfiler.ResetSession();
			}
		}
	}
}