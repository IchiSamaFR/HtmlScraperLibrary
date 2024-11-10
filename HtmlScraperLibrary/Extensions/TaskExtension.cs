using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlScraperLibrary.Extensions
{
    internal static class TaskExtension
    {
        private static readonly List<Task> lst = new List<Task>();
        private const int THREAD_SIMUL = 5;
        public static async Task WaitBy(IEnumerable<Task> tasks, int amount)
        {
            var tmp = new List<Task>();
            for (int i = 0; i < Math.Ceiling(tasks.Count() / (float)amount); i++)
            {
                tmp.Clear();
                tmp.AddRange(tasks.Skip(i * amount).Take(amount));
                tmp.ForEach(t => t.Start());
                await Task.WhenAll(tmp);
            }
        }
        public static async Task Wait(Task task)
        {
            while (lst.Count(t => !t.IsCompleted) > THREAD_SIMUL)
            {
                await Task.Delay(10);
            }
            task.Start();
            lst.Add(task);
        }
        public static async Task WaitUntilEnd()
        {
            while (lst.Any(t => !t.IsCompleted))
            {
                await Task.Delay(10);
            }
            lst.Clear();
        }
    }
}
