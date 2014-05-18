using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi;

namespace Wolfje.Plugins.EnglishPlox {
    [TerrariaApi.Server.ApiVersion(1, 16)]
    public class EnglishPloxPlugin : TerrariaApi.Server.TerrariaPlugin {
        public static readonly Regex invalidCharactersRegex = new Regex(@"[^\da-z!@#\$%\^\&\*\(\)\-\+~ ;{}|\[\]:\.,_`]", RegexOptions.IgnoreCase);

        public EnglishPloxPlugin(Terraria.Main game)
            : base(game) {
        }

        public override string Author {
            get {
                return "Wolfje";
            }
        }

        public override string Description {
            get {
                return "Forces everyone in the server to have a name consisting of only letters and numbers";
            }
        }

        public override string Name {
            get {
                return "EnglishPlox";
            }
        }

        public override Version Version {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public override void Initialize() {
            TerrariaApi.Server.ServerApi.Hooks.ServerJoin.Register(this, Server_Join);
        }

        private void Server_Join(TerrariaApi.Server.JoinEventArgs args) {
            Terraria.Player player = null;

            if ((player = Terraria.Main.player.ElementAtOrDefault(args.Who)) == null) {
                return;
            }

            if (invalidCharactersRegex.IsMatch(player.name) == true) {
                StringBuilder sb = new StringBuilder();
                foreach (Match m in invalidCharactersRegex.Matches(player.name)) {
                    sb.Append(m.Value);
                }
                Terraria.NetMessage.SendData((int)PacketTypes.Disconnect, player.whoAmi, text: "Your name cannot contain these characters: " + sb.ToString());
            }
        }

    }
}
