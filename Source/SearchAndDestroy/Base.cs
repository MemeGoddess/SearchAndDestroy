using HugsLib;
using SearchAndDestroy.Storage;
using Verse;

namespace SearchAndDestroy
{
    class Base : ModBase
    {
        public override string ModIdentifier => "SearchAndDestroy";
        public static Base Instance { get; private set; }
        private ExtendedDataStorage _extendedDataStorage;

        public Base()
        {
            Instance = this;
        }
        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return _extendedDataStorage;
        }
        public override void WorldLoaded()
        {
            _extendedDataStorage = Find.World.GetComponent<ExtendedDataStorage>();
            base.WorldLoaded();
        }
    }


}
