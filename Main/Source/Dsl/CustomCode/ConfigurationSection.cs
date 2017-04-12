using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;

namespace ConfigurationSectionDesigner
{
    [ValidationState( ValidationState.Enabled )]
    public partial class ConfigurationSection
    {
        #region Convenience Properties

        /// <summary>
        /// Gets a full documentation text based on the user-given <see cref="Documentation"/>.
        /// </summary>
        /// <remarks>
        /// If no user-given documentation is present, a "good enough" default description is returned.
        /// </remarks>
        public override string DocumentationText
        {
            get
            {
                return (string.IsNullOrEmpty( this.Documentation ) ? string.Format( "The {0} Configuration Section.", this.Name ) : this.Documentation);
            }
        }

        #endregion

        #region Automatically Set XML Name

        protected override void OnNameChanged( EventArgs e )
        {
            // When the name changes, set the XML name to the same name but camelCased.
            this.XmlSectionName = NamingHelper.ToCamelCase( this.Name );

            // Always call the base class method.
            base.OnNameChanged( e );
        }

        #endregion

        #region Unset Singleton Code Generation Option when inheritance modifier is set to Abstract

        protected override void OnInheritanceModifierChanged( EventArgs e )
        {
            // If the inheritance modifier is set to abstract, remove the singleton code generation option
            if( this.InheritanceModifier == InheritanceModifiers.Abstract )
                this.CodeGenOptions &= ~ConfigurationSectionCodeGenOptions.Singleton;

            // If it's set to anything else, restore it.
            else
                this.CodeGenOptions |= ConfigurationSectionCodeGenOptions.Singleton;


            // Always call the base class method.
            base.OnInheritanceModifierChanged( e );
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the XML Name.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod( ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save )]
        private void ValidateXmlName( ValidationContext context )
        {
            if( string.IsNullOrEmpty( this.XmlSectionName ) )
            {
                context.LogError( "The Xml Section Name is required and cannot be an empty string.", "RequiredProperty", this );
            }
        }

        /// <summary>
        /// Validates the custom section provider.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod( ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save )]
        private void ValidateCustomSectionProvider( ValidationContext context )
        {
            if( (this.CodeGenOptions & ConfigurationSectionCodeGenOptions.Protection) == ConfigurationSectionCodeGenOptions.Protection )
            {
                if( this.ProtectionProvider == ProtectionProviders.Custom )
                {
                    if( string.IsNullOrEmpty( this.CustomProtectionProvider ) )
                    {
                        context.LogError( "The Custom Protection Provider cannot be an empty string when the Protection Provider is set to 'Custom'", "CustomProviderNotSet", this );
                    }
                }
            }
        }

        /// <summary>
        /// Validates the inheritance.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod( ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save )]
        private void ValidateSectionInheritance( ValidationContext context )
        {
            if( this.InheritanceModifier == InheritanceModifiers.Abstract && (this.CodeGenOptions & ConfigurationSectionCodeGenOptions.Singleton) == ConfigurationSectionCodeGenOptions.Singleton )
            {
                context.LogError( "An abstract Configuration Section cannot be instantiated. The Code Generation Options cannot contain the Singleton setting.", "InvalidProperty", this );
            }

            if( this.BaseClass != null && this.BaseClass is ConfigurationSection )
                if( this.HasCustomChildElements && ((ConfigurationSection)this.BaseClass).HasCustomChildElements )
                    context.LogViolation(
                        (this.BaseClass.InheritanceModifier == InheritanceModifiers.Abstract) ? ViolationType.Warning : ViolationType.Message,
                        string.Format(
                            "{0} and its base class {1} both have the HasCustomChildElements flag set. The custom handler in {1} will never be called" + ((this.BaseClass.InheritanceModifier == InheritanceModifiers.Abstract) ? "" : "when using {0}") + ".",
                            this.Name,
                            this.BaseClass.Name ),
                        "ShadowWarning",
                        this, this.BaseClass );

            if( (this.CodeGenOptions & ConfigurationSectionCodeGenOptions.XmlnsProperty) == ConfigurationSectionCodeGenOptions.XmlnsProperty )
            {
                IEnumerable<BaseConfigurationType> ancestors;
                if( !GetAncestors( out ancestors ) )
                {
                    foreach( BaseConfigurationType ancestor in ancestors )
                    {
                        if( this == ancestor )
                            continue;

                        ConfigurationSection section = ancestor as ConfigurationSection;
                        if( section == null )
                            continue;

                        if( (section.CodeGenOptions & ConfigurationSectionCodeGenOptions.XmlnsProperty) == ConfigurationSectionCodeGenOptions.XmlnsProperty )
                        {
                            context.LogWarning( string.Format( "The XmlnsProperty code generation option is set on both {0} and its base class {1}, which will cause shadowing.", this.Name, section.Name ), "ShadowWarning", this, section );
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}