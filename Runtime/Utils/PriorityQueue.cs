using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodHub.Core.Runtime.Utils
{

    /// <summary>
    /// An ordered queue of objects where each has an associated priority.
    /// De-queueing an item will return the item with the highest priority that was first added.
    /// Higher value = higher priority
    /// </summary>
    public class PriorityQueue<T>
    {
        public class QueueItem<TValue>
        {
            public readonly int priority;
            public readonly TValue value;

            public QueueItem(int priority, TValue value)
            {
                this.priority = priority;
                this.value = value;
            }
        }

        private LinkedList<QueueItem<T>> _linkedList;

        public List<QueueItem<T>> Values => _linkedList.ToList();

        public int Count => _linkedList.Count;

        public PriorityQueue()
        {
            _linkedList = new LinkedList<QueueItem<T>>();
        }

        /// <summary>
        /// Adds an item to the priority queue, higher value equals higher priority meaning the popup will be shown sooner.
        /// </summary>
        public void Enqueue(int priority, T value)
        {
            QueueItem<T> newQueueItem = new QueueItem<T>(priority, value);

            // If the list is empty is empty
            if (_linkedList.Count == 0)
            {
                _linkedList.AddFirst(newQueueItem);
                return;
            }

            // Check if higher priority than the first item
            if (_linkedList.First.Value.priority < priority)
            {
                _linkedList.AddFirst(newQueueItem);
                return;
            }

            // Check if lower or equal priority than the last item
            if (_linkedList.Last != null && _linkedList.Last.Value.priority >= priority)
            {
                _linkedList.AddLast(newQueueItem);
                return;
            }

            // Iterate over the list and search for this values slot
            LinkedListNode<QueueItem<T>> current = _linkedList.First;

            while (current != null && current.Value.priority >= priority)
            {
                current = current.Next;
            }

            if (current == null)
                return;

            _linkedList.AddAfter(current, newQueueItem);
        }

        /// <summary>
        /// Removes and returns the item in the queue with the highest priority value.
        /// </summary>
        public T Dequeue()
        {
            if (_linkedList.Count == 0)
                return default;

            QueueItem<T> firstNodeValue = _linkedList.Last.Value;
            _linkedList.RemoveFirst();

            return firstNodeValue.value;
        }

        /// <summary>
        /// Returns the item in the queue with the highest priority value without removing it from the queue.
        /// </summary>
        public T Peek()
        {
            return _linkedList.Count == 0 ? default : _linkedList.First.Value.value;
        }

        /// <summary>
        /// Remove all items from the queue.
        /// </summary>
        public void Clear()
        {
            _linkedList.Clear();
        }
    }

}