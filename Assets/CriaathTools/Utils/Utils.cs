using System;
using System.Threading.Tasks;

namespace Criaath
{
    public static class Utils
    {
        public static async void ActionDelay(float delay, Action action)
        {
            await Task.Delay((int)(delay * 1000));
            action?.Invoke();
        }

    }
}
