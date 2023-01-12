using Volo.Abp.Settings;

namespace FSI.Settings
{
    public class SettingDefinitionProvider : Volo.Abp.Settings.SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(NOMSettings.MySetting1));
            
            context.Add(new SettingDefinition(Settings.RootUpdate));
        }
    }
}
