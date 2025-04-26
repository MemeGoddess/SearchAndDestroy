﻿using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace SearchAndDestroy.Storage
{


    public class ExtendedDataStorage : WorldComponent, IExposable
    {
        private Dictionary<int, ExtendedPawnData> _store =
            new Dictionary<int, ExtendedPawnData>();

        private List<int> _idWorkingList;

        private List<ExtendedPawnData> _extendedPawnDataWorkingList;

        public ExtendedDataStorage(World world) : base(world)
        {

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(
                ref _store, "store",
                LookMode.Value, LookMode.Deep,
                ref _idWorkingList, ref _extendedPawnDataWorkingList);

            // Seems to null out on existing saves, value needs to be set
            if(_store == null)
                _store = new Dictionary<int, ExtendedPawnData>();

        }

        // Return the associate extended data for a given Pawn, creating a new association
        // if required.
        public ExtendedPawnData GetExtendedDataFor(Pawn pawn)
        {

            var id = pawn.thingIDNumber;
            ExtendedPawnData data = null;
            if (_store.TryGetValue(id, out data))
            {
                return data;
            }

            var newExtendedData = new ExtendedPawnData();

            _store[id] = newExtendedData;
            return newExtendedData;
        }

        public void DeleteExtendedDataFor(Pawn pawn)
        {
            _store.Remove(pawn.thingIDNumber);
        }
    }
}
