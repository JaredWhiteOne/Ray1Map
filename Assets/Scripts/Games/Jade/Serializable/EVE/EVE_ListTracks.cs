﻿using BinarySerializer;

namespace Ray1Map.Jade
{
    public class EVE_ListTracks : Jade_File {
		public override string Export_Extension => "trl";
		public override bool HasHeaderBFFile => true;

        public Jade_Reference<EVE_ListTracks> ListTracks_TRS { get; set; }
        public ushort TracksCount { get; set; }
        public ushort TracksCount2 { get; set; }
        public ushort Flags { get; set; }
        public EVE_Track[] Tracks { get; set; }
        public ushort Montreal_Version { get; set; }

        protected override void SerializeFile(SerializerObject s)
        {
            if (s.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_TMNT)) {
				ListTracks_TRS = s.SerializeObject<Jade_Reference<EVE_ListTracks>>(ListTracks_TRS, name: nameof(ListTracks_TRS));
                if (Loader.IsBinaryData) {
                    ListTracks_TRS?.ResolveEmbedded(s, flags: LOA_Loader.ReferenceFlags.DontCache | LOA_Loader.ReferenceFlags.DontUseCachedFile | LOA_Loader.ReferenceFlags.Log, unknownFileSize: true);
                } else {
                    ListTracks_TRS?.Resolve();
                }
			}
            bool useCount2 = false;
            TracksCount = s.Serialize<ushort>(TracksCount, name: nameof(TracksCount));
            if (s.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_Montreal) && TracksCount >= 0x8000) {
                Montreal_Version = TracksCount;
                TracksCount2 = s.Serialize<ushort>(TracksCount2, name: nameof(TracksCount2));
                useCount2 = true;
            }
			Flags = s.Serialize<ushort>(Flags, name: nameof(Flags));
            Tracks = s.SerializeObjectArray<EVE_Track>(Tracks, useCount2 ? TracksCount2 : TracksCount, onPreSerialize: trk => trk.ListTracks = this, name: nameof(Tracks));
        }

		protected override void OnChangeContext(Context oldContext, Context newContext) {
			base.OnChangeContext(oldContext, newContext);
            if (newContext.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_Montpellier) && oldContext.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_Montreal)) {
                TracksCount = 0;
                TracksCount2 = 0;
                Tracks = new EVE_Track[0];
            }
		}
	}
}