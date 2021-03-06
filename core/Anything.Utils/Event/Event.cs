﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anything.Utils.Event
{
    public class Event<TArgs>
    {
        private readonly List<Func<TArgs, Task>> _asyncHandlers = new();
        private readonly List<Action<TArgs>> _handlers = new();

        public void On(Func<TArgs, Task> handler)
        {
            _asyncHandlers.Add(handler);
        }

        public void On(Action<TArgs> handler)
        {
            _handlers.Add(handler);
        }

        internal void Emit(TArgs args)
        {
            foreach (var handler in _handlers)
            {
                handler.Invoke(args);
            }

            Task.WhenAll(_asyncHandlers.Select(handler => handler.Invoke(args))).Wait();
        }

        internal Task EmitAsync(TArgs args)
        {
            foreach (var handler in _handlers)
            {
                handler.Invoke(args);
            }

            return Task.WhenAll(_asyncHandlers.Select(handler => handler.Invoke(args)));
        }
    }
}
