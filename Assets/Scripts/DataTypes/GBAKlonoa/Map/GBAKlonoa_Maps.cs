﻿using BinarySerializer;

namespace R1Engine
{
    public class GBAKlonoa_Maps : BinarySerializable
    {
        public bool Pre_SerializeData { get; set; }

        public Pointer[] MapPointers { get; set; }
        public MapTile[][] Maps { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            MapPointers = s.SerializePointerArray(MapPointers, 3, name: nameof(MapPointers));

            if (!Pre_SerializeData)
                return;

            Maps ??= new MapTile[MapPointers.Length][];

            for (int i = 0; i < Maps.Length; i++)
            {
                s.DoAt(MapPointers[i], () =>
                {
                    s.DoEncoded(new GBAKlonoa_Encoder(), () =>
                    {
                        var is8Bit = i == 2;

                        Maps[i] = s.SerializeObjectArray<MapTile>(Maps[i], is8Bit ? s.CurrentLength : s.CurrentLength / 2, x => x.Pre_GBAKlonoa_Is8Bit = is8Bit, name: $"{nameof(Maps)}[{i}]");
                    });
                });
            }
        }
    }
}