using AnarchyEngine.DataTypes;
using AnarchyEngine.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = System.Action;

namespace AnarchyEngine.Core {
    public static class Scheduler {

        private static Queue<IScheduledItem>
            Items = new Queue<IScheduledItem>(),
            Backlog = new Queue<IScheduledItem>();

        public static void Push(ScheduleFlag flags, Object item) {
            Backlog.Enqueue(new ScheduledItem(flags, item));
        }

        public static void Push(ScheduleFlag flags, Object item, Action afterCycle) {
            var scheduled = new ScheduledItem(flags, item);
            Backlog.Enqueue(new ScheduledItemAction(scheduled, afterCycle));
        }

        internal static void Update() {
            Items.Empty(s => s.Cycle());
            Utilities.Swap(ref Items, ref Backlog);
        }


        struct ScheduledItem : IScheduledItem {
            public readonly ScheduleFlag Flags;
            public readonly Object Item;

            public ScheduledItem(ScheduleFlag flags, Object item) {
                Flags = flags;
                Item = item;
            }
            
            public void Cycle() {
                if (Flags.HasFlag(ScheduleFlag.Init)) {
                    Item.Init();
                } if (Flags.HasFlag(ScheduleFlag.Start)) {
                    Item.Start();
                } if (Flags.HasFlag(ScheduleFlag.Dispose)) {
                    Item.Dispose();
                }
            }
        }

        struct ScheduledItemAction : IScheduledItem {
            public readonly ScheduledItem Item;
            public readonly Action Finish;

            public ScheduledItemAction(ScheduledItem item, Action finish) {
                Item = item;
                Finish = finish;
            }

            public void Cycle() {
                Item.Cycle();
                Finish();
            }
        }

        interface IScheduledItem {
            void Cycle();
        }
    }

    [System.Flags]
    public enum ScheduleFlag {
        None = 0,
        Init = 1,
        Start = 2,
        Dispose = 4
    }
}
