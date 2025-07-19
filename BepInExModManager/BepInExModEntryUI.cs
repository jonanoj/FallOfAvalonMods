using Awaken.TG.Main.UI.Menu.ModManager;
using Awaken.Utility.Assets.Modding;

namespace BepInExModManager;

public class BepInExModEntryUI : ModEntryUI
{
    public BepInExModEntryUI(int index, ModMetadata metadata) : base(index, new ModHandle(index))
    {
        Metadata = metadata;
    }

    public override void OnInitialize()
    {
        // Prevent base OnInitialize from being called since it tries to interact with the ModManager
    }
}
