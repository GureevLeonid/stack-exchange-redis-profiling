using System;
using System.Threading.Tasks;
using Prometheus;
using StackExchange.Redis.Profiling;

namespace RedisProfiling
{
	public class PrometheusRedisProfilingMetrics : IRedisProfilingMetrics
	{
		private const string SessionLabel = "session";
		private const string CommandLabel = "command";

		private static readonly string[] CommonLabels =
		{
			SessionLabel,
			CommandLabel
		};

		private static readonly Histogram RedisElapsedTime = Metrics
			.CreateHistogram(
				"redis_profiler_elapsed_time_total",
				"Histogram of redis command elapsed times.",
				CommonLabels);

		private static readonly Histogram RedisCreationToEnqueuedTime = Metrics
			.CreateHistogram(
				"redis_profiler_creation_to_enqueued_time_total",
				"Histogram of redis command creation to enqueued times.",
				CommonLabels);

		private static readonly Histogram RedisEnqueuedToSendingTime = Metrics
			.CreateHistogram(
				"redis_profiler_enqueued_to_sending_time_total",
				"Histogram of redis command enqueued to sending times.",
				CommonLabels);

		private static readonly Histogram RedisResponseToCompletionTime = Metrics
			.CreateHistogram(
				"redis_profiler_response_to_completion_time_total",
				"Histogram of redis command reponse to completion times.",
				CommonLabels);

		private static readonly Histogram RedisSentToReponseTime = Metrics
			.CreateHistogram(
				"redis_profiler_sent_to_response_time_total",
				"Histogram of redis command sent to response times.",
				CommonLabels);

		public ValueTask Collect(string session, IProfiledCommand command)
		{
			if (string.IsNullOrWhiteSpace(session))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(session));

			_ = command ?? throw new ArgumentNullException(nameof(command));

			var commonLabels = new[]
			{
				session,
				command.Command
			};

			RedisElapsedTime
				.WithLabels(commonLabels)
				.Observe(command.ElapsedTime.TotalMilliseconds);

			RedisCreationToEnqueuedTime
				.WithLabels(commonLabels)
				.Observe(command.CreationToEnqueued.TotalMilliseconds);

			RedisEnqueuedToSendingTime
				.WithLabels(commonLabels)
				.Observe(command.EnqueuedToSending.TotalMilliseconds);

			RedisResponseToCompletionTime
				.WithLabels(commonLabels)
				.Observe(command.ResponseToCompletion.TotalMilliseconds);

			RedisSentToReponseTime
				.WithLabels(commonLabels)
				.Observe(command.SentToResponse.TotalMilliseconds);

			return ValueTasksExtensions.EmptyValueTask;
		}
	}
}