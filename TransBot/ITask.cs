using System;
using System.Threading.Tasks;

namespace TLBOT
{
    public interface ITask
    {
        string CurrentStatus { get; }
        uint Progress { get; }

        bool Finished { get; }

        string[] Lines { get; set; }

        Task Build(Action OnFinish = null);

        void UpdateLines(string[] lines);
    }
}