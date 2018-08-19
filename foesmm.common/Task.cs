using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace foesmm.common
{
    public class Task
    {
        public Action<IProgress> ApplyAction { get; protected set; }
        public Action<IProgress> RollbackAction { get; protected set; }

        public Task[] SubTasks { get; protected set; }

        public Task(Action<IProgress> applyAction, Action<IProgress> rollbackAction = null, Task[] subTasks = null)
        {
            ApplyAction = applyAction;
            RollbackAction = rollbackAction;
            SubTasks = subTasks;
        }
    }
}
