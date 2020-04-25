using System.Threading.Tasks;

namespace RedisProfiling
{
	public static class ValueTasksExtensions
	{
		public static readonly ValueTask EmptyValueTask = new ValueTask();
	}
}