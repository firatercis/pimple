using System.Collections.Generic;
using System.Linq;

namespace SoftwareKingdom.Common.InterfaceBus
{
    public class InterfaceBusEntry<TInterface> where TInterface : class
    {

        public int priority;
        public TInterface reference;

        public InterfaceBusEntry(TInterface reference, int priority)
        {
            this.priority = priority;
            this.reference = reference;
        }

    }

    public static class InterfaceBus<TInterface> where TInterface : class

    {
        const int DEFAULT_PRIORITY = 0;

        private static List<InterfaceBusEntry<TInterface>> entries = new List<InterfaceBusEntry<TInterface>>();
        public static void Register(TInterface reference, int priority = DEFAULT_PRIORITY)
        {
            InterfaceBusEntry<TInterface> entry = new InterfaceBusEntry<TInterface>(reference, priority);
            entries.Add(entry);
            entries = entries.OrderByDescending(x => x.priority).ToList();
        }

        public static void Remove(TInterface reference)
        {
            InterfaceBusEntry<TInterface> entryToBeDeleted = entries.Find(x => x.reference == reference);

            entries.Remove(entryToBeDeleted);
        }

        public static TInterface Get()
        {
            TInterface result = default(TInterface);

            if (entries.Count > 0)
                result = entries[0].reference;

            return result;
        }

        public static TInterface[] GetAll()
        {
            return entries.Select(x => x.reference).ToArray();
        }

        public static int GetEntryCount()
        {
            return entries.Count;
        }
    }
}