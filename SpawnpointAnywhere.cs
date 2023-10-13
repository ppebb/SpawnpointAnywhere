using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ModLoader;

namespace SpawnpointAnywhere {
    public class SpawnpointAnywhere : Mod {
        public override void Load() {
            Terraria.IL_Player.CheckSpawn_Internal += CheckSpawn_Internal;
        }

        private void CheckSpawn_Internal(ILContext il) {
            ILCursor c = new(il);

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchCall(typeof(Terraria.WorldGen).GetMethod("StartRoomCheck", BindingFlags.Public | BindingFlags.Static)),
                i => i.MatchBrtrue(out _)
            )) {
                Logger.Error("Unable to patch Terraria.Player.CheckSpawn_Internal");
            }

            c.Remove();
            c.Emit(OpCodes.Ldc_I4_1);
        }
    }
}
