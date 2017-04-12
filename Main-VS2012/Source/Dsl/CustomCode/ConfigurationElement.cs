using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ConfigurationSectionDesigner.Properties;
using Microsoft.VisualStudio.Modeling.Validation;

namespace ConfigurationSectionDesigner
{
    [ValidationState( ValidationState.Enabled )]
    public partial class ConfigurationElement
    {
        #region Convenience Properties

        /// <summary>
        /// Gets all configuration properties as one list (both Elements and Attributes).
        /// </summary>
        public IList<ConfigurationProperty> Properties
        {
            get
            {
                List<ConfigurationProperty> properties = new List<ConfigurationProperty>();
                foreach( ConfigurationProperty attributeProperty in this.AttributeProperties )
                {
                    properties.Add( attributeProperty );
                }
                foreach( ConfigurationProperty elementProperty in this.ElementProperties )
                {
                    properties.Add( elementProperty );
                }
                return properties;
            }
        }

        /// <summary>
        /// Gets all configuration properties as one list (both Elements and Attributes),
        /// including properties from ancestor classes.
        /// </summary>
        public IList<ConfigurationProperty> AllProperties
        {
            get
            {
                IEnumerable<BaseConfigurationType> ancestors;
                if( GetAncestors( out ancestors ) )
                    return null;

                List<ConfigurationProperty> properties = new List<ConfigurationProperty>();
                foreach( ConfigurationElement element in ancestors )
                {
                    properties.AddRange( element.Properties );
                }
                return properties;
            }
        }

        /// <summary>
        /// Gets a list of ElementProperties, including ElementProperties
        /// from ancestor classes.
        /// </summary>
        public IList<ElementProperty> AllElementProperties
        {
            get
            {
                IEnumerable<BaseConfigurationType> ancestors;
                if( GetAncestors( out ancestors ) )
                    return null;

                List<ElementProperty> elementProperties = new List<ElementProperty>();
                foreach( ConfigurationElement element in ancestors )
                {
                    elementProperties.AddRange( element.ElementProperties );
                }
                return elementProperties;
            }
        }

        /// <summary>
        /// Gets a list of AttributeProperties, including AttributeProperties
        /// from ancestor classes.
        /// </summary>
        public IList<AttributeProperty> AllAttributeProperties
        {
            get
            {
                IEnumerable<BaseConfigurationType> ancestors;
                if( GetAncestors( out ancestors ) )
                    return null;

                List<AttributeProperty> attributeProperties = new List<AttributeProperty>();
                foreach( ConfigurationElement element in ancestors )
                {
                    attributeProperties.AddRange( element.AttributeProperties );
                }
                return attributeProperties;
            }
        }

        /// <summary>
        /// Gets a full documentation text based on the user-given <see cref="Documentation"/>.
        /// </summary>
        /// <remarks>
        /// If no user-given documentation is present, a "good enough" default description is returned.
        /// </remarks>
        public virtual string DocumentationText
        {
            get
            {
                return (string.IsNullOrEmpty( this.Documentation ) ? string.Format( "The {0} Configuration Element.", this.Name ) : this.Documentation);
            }
        }

        /// <summary>
        /// Gets all key properties, including the ones from superclasses
        /// </summary>
        public IList<ConfigurationProperty> KeyProperties
        {
            get
            {
                IEnumerable<BaseConfigurationType> ancestors;
                if( GetAncestors( out ancestors ) )
                    return null;

                List<ConfigurationProperty> keys = new List<ConfigurationProperty>();
                foreach( ConfigurationElement element in ancestors )
                {
                    foreach( ConfigurationProperty property in element.Properties )
                    {
                        if( property.IsKey )
                            keys.Add( property );
                    }
                }
                return keys;
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the Name property.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod( ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save )]
        private void ValidateName( ValidationContext context )
        {
            if( string.IsNullOrEmpty( this.Name ) )
            {
                context.LogError( "The Name is required and cannot be an empty string.", "RequiredProperty", this );
            }
            if (!NamingHelper.IsValidName(this.Name))
            {
                context.LogError(Resources.Error_InvalidName, "RequiredProperty", this);
            }
            
        }

        /// <summary>
        /// Makes sure there's no property with the same name as the type.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod( ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save )]
        private void ValidatePropertyNames( ValidationContext context )
        {
            foreach( ConfigurationProperty property in Properties )
            {
                if( property.Name == this.Name )
                {
                    context.LogError( "An element cannot have a property with the same name as its own name.", "InvalidName", property );
                    break;
                }
                if (!NamingHelper.IsValidOrEmptyName(this.Name))
                {
                    context.LogError(Resources.Error_InvalidName, "RequiredProperty", this);
                    break;
                }
            }

            IEnumerable<BaseConfigurationType> ancestors;
            if( !GetAncestors( out ancestors ) )
            {
                foreach( ConfigurationProperty property in Properties )
                {
                    foreach( BaseConfigurationType ancestor in ancestors )
                    {
                        if( ancestor == this )
                            continue;

                        ConfigurationElement element = ancestor as ConfigurationElement;
                        if( element == null )
                            continue;

                        foreach( ConfigurationProperty ancestralProperty in element.Properties )
                        {
                            if( property.Name == ancestralProperty.Name )
                            {
                                context.LogWarning( string.Format( "The {1} property on {0} will shadow the {1} property on {2}", this.Name, property.Name, element.Name ), "ShadowWarning", this, ancestor );
                                goto nextProperty;
                            }
                        }
                    }
                nextProperty: ;
                }
            }
        }

        #endregion
    }
}