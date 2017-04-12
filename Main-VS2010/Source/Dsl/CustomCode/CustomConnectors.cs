using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ConfigurationSectionDesigner
{
    // The methods in this file are used to provide the "custom source" for various Connectors.
    // Certain properties refer to other ModelElements in the diagram.
    // These properties don't have their own shape (they are shown in a Compartment Decorator) so they cannot
    // have a standard connector from one shape to the other. These properties are therefore defined as having
    // a "Custom Source", for which the following methods provide the implementation code.

    public partial class ConfigurationSectionDesignerDiagram
    {
        private NodeShape GetSourceShapeForElementPropertyHasTypeConnector( ElementPropertyHasTypeConnector connector )
        {
            // Jump from the Element Property to its parent shape, i.e. the Configuration Element that contains it.
            // This will make the connection start at shape of the "owning" Configuration Element.
            return (NodeShape)connector.ParentShape;
        }

        private ModelElement GetSourceRolePlayerForLinkMappedByElementPropertyHasTypeConnector( ElementPropertyHasTypeConnector connector )
        {
            // Likewise, the source role player is the "owning" Configuration Element.
            return connector.ParentShape.ModelElement;
        }

        private NodeShape GetSourceShapeForConfigurationSectionPropertyHasConfigurationSectionConnector( ConfigurationSectionPropertyHasConfigurationSectionConnector connector )
        {
            // Jump from the ConfigurationSection Property to its parent shape, i.e. the Configuration Section Group that contains it.
            // This will make the connection start at shape of the "owning" Configuration Section Group.
            return (NodeShape)connector.ParentShape;
        }

        private ModelElement GetSourceRolePlayerForLinkMappedByConfigurationSectionPropertyHasConfigurationSectionConnector( ConfigurationSectionPropertyHasConfigurationSectionConnector connector )
        {
            // Likewise, the source role player is the "owning" Configuration Section Group.
            return connector.ParentShape.ModelElement;
        }

        private NodeShape GetSourceShapeForConfigurationSectionGroupHasConfigurationSectionGroupPropertiesConnector( ConfigurationSectionGroupHasConfigurationSectionGroupPropertiesConnector connector )
        {
            // Jump from the ConfigurationSectionGroup Property to its parent shape, i.e. the Configuration Section Group that contains it.
            // This will make the connection start at shape of the "owning" Configuration Section Group.
            return (NodeShape)connector.ParentShape;
        }

        private ModelElement GetSourceRolePlayerForLinkMappedByConfigurationSectionGroupHasConfigurationSectionGroupPropertiesConnector( ConfigurationSectionGroupHasConfigurationSectionGroupPropertiesConnector connector )
        {
            // Likewise, the source role player is the "owning" Configuration Section Group.
            return connector.ParentShape.ModelElement;
        }
    }

    internal partial class FixUpDiagram
    {
        private ModelElement GetParentForElementPropertyHasType( ElementPropertyHasType childLink )
        {
            // Jump from the link to the Element property and up to its "owning" Configuration Element.
            return childLink.ElementProperty.ConfigurationElement;
        }

        private ModelElement GetParentForConfigurationSectionPropertyHasConfigurationSection( ConfigurationSectionPropertyHasConfigurationSection childLink )
        {
            // Jump from the link to the Configuration Section property and up to its "owning" Configuration Section Group.
            return childLink.ConfigurationSectionProperty.ConfigurationSectionGroup;
        }

        private ModelElement GetParentForConfigurationSectionGroupPropertyHasConfigurationSectionGroup( ConfigurationSectionGroupPropertyHasConfigurationSectionGroup childLink )
        {
            // Jump from the link to the Configuration Section Group property and up to its "owning" Configuration Section Group.
            return childLink.ConfigurationSectionGroupProperty.ConfigurationSectionGroup;
        }
    }
}