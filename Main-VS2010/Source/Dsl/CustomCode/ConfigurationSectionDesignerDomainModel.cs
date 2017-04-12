using System;

namespace ConfigurationSectionDesigner
{
    public partial class ConfigurationSectionDesignerDomainModel
    {
        protected override Type[] GetCustomDomainModelTypes()
        {
            return new Type[] {
                typeof(ConfigurationElementCollectionHasItemTypeAddRule),
                typeof(ConfigurationElementCollectionHasItemTypeDeleteRule),
                typeof(ConfigurationElementCollectionHasItemTypeRolePlayerChangeRule)
            };
        }
    }
}