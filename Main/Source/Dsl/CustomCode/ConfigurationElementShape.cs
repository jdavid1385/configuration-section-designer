using System;
using System.Drawing;
using ConfigurationSectionDesigner.Properties;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ConfigurationSectionDesigner
{
    public partial class ConfigurationElementShape
    {
        #region Provide Icons For Compartment Entries

        protected override CompartmentMapping[] GetCompartmentMappings( Type melType )
        {
            // The ConfigurationElementShape is defined as Double Derived, so we
            // can override this method and modify the return value to include an icon image getter.

            // First we retrieve the basic compartment mappings as configured in the DSL definition.
            CompartmentMapping[] mappings = base.GetCompartmentMappings( melType );

            // Then, for each compartment, we provide an image getter that determines the icon to display.
            foreach( ElementListCompartmentMapping mapping in mappings )
            {
                mapping.ImageGetter = CompartmentImageProvider;
            }
            return mappings;
        }

        /// <summary>
        /// Determines the icon to show in a compartment entry, based on its properties.
        /// </summary>
        /// <param name="element">The configuration property being shown in the compartment.</param>
        /// <returns>The icon to use to represent the configuration property.</returns>
        private Image CompartmentImageProvider( ModelElement element )
        {
            // Show icons.
            ConfigurationProperty prop = (ConfigurationProperty)element;
            if( prop is AttributeProperty )
            {
                if( prop.IsKey )
                {
                    return Resources.KeyProperty;
                }
                else if( prop.IsRequired )
                {
                    return Resources.RequiredProperty;
                }
                else
                {
                    return Resources.Property;
                }
            }
            else if( prop is ElementProperty )
            {
                if( prop.IsKey )
                {
                    return Resources.KeyElement;
                }
                else if( prop.IsRequired )
                {
                    return Resources.RequiredElement;
                }
                else
                {
                    return Resources.Element;
                }
            }
            return null;
        }

        #endregion
    }
}