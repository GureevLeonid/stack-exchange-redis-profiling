# stack-exchange-redis-profiling

Simple stack exchange redis profiling implementation.

## How to use

Register in DI

```
services.AddSingleton<IRedisConnectionPool, SingleRedisConnectionPool>();
services.AddSingleton<IRedisProfiler, AsyncLocalRedisProfiler>();
services.AddSingleton<IRedisProfilingMetrics, PrometheusRedisProfilingMetrics>();
```

Inject IRedisProfiler

```
public RedisController(IRedisConnectionPool redisConnectionPool, IRedisProfiler redisProfiler)
{
    _redisConnectionPool = redisConnectionPool ?? throw new ArgumentNullException(nameof(redisConnectionPool));
    _redisProfiler = redisProfiler ?? throw new ArgumentNullException(nameof(redisProfiler));
}
```

Wrap 

```
await using (_redisProfiler.Profile("StringSetAsync"))
{
    await database.StringSetAsync(key, value, ttl);
}
```

## Resulting prometheus metrics format

```
redis_profiler_creation_to_enqueued_time_total_sum{session="StringSetAsync",command="SETEX"} 310.5178
redis_profiler_creation_to_enqueued_time_total_count{session="StringSetAsync",command="SETEX"} 1
redis_profiler_elapsed_time_total_sum{session="StringSetAsync",command="SETEX"} 451.379
redis_profiler_elapsed_time_total_count{session="StringSetAsync",command="SETEX"} 1
redis_profiler_enqueued_to_sending_time_total_sum{session="StringSetAsync",command="SETEX"} 51.3488
redis_profiler_enqueued_to_sending_time_total_count{session="StringSetAsync",command="SETEX"} 1
redis_profiler_sent_to_response_time_total_sum{session="StringSetAsync",command="SETEX"} 76.3809
redis_profiler_sent_to_response_time_total_count{session="StringSetAsync",command="SETEX"} 1
redis_profiler_response_to_completion_time_total_sum{session="StringSetAsync",command="SETEX"} 13.1315
redis_profiler_response_to_completion_time_total_count{session="StringSetAsync",command="SETEX"} 1
```
