using System;
using System.Threading;
using StackExchange.Redis.Profiling;

namespace RedisProfiling
{
	public class AsyncLocalRedisProfiler : IRedisProfiler
	{
		private readonly IRedisProfilingMetrics _metrics;
		private readonly AsyncLocal<ProfilingSession> perAsyncFlowSession = new AsyncLocal<ProfilingSession>();

		public AsyncLocalRedisProfiler(IRedisProfilingMetrics metrics)
		{
			_metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
		}

		public ProfilingSession GetSession()
		{
			return perAsyncFlowSession.Value;
		}

		public void ResetSession()
		{
			if (perAsyncFlowSession.Value != null)
				perAsyncFlowSession.Value = null;
		}

		public IRedisProfilingSession Profile(string session)
		{
			if (string.IsNullOrWhiteSpace(session))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(session));

			// todo: to check that we are not trying to profile while are profiling
			try
			{
				return new RedisProfilingSession(session, _metrics, this);
			}
			catch (Exception exception)
			{
				ResetSession();
				// todo: to log exception here

				return NullRedisProfilingSession.Instance;
			}
		}

		public ProfilingSession StartSession()
		{
			var profilingSession = perAsyncFlowSession.Value;
			if (profilingSession == null) perAsyncFlowSession.Value = profilingSession = new ProfilingSession();
			return profilingSession;
		}
	}
}