using System;
using System.Collections.Generic;
using System.Text;
using ConfigurationSectionDesigner.Properties;
using Microsoft.VisualStudio.Modeling.Validation;
using System.Collections.Specialized;
using Microsoft.VisualStudio.Modeling;

namespace ConfigurationSectionDesigner
{
    [ValidationState(ValidationState.Enabled)]
    public partial class ConfigurationProperty
    {
        #region Convenience Properties

        /// <summary>
        /// Gets a full documentation text based on the user-given <see cref="Documentation"/>.
        /// </summary>
        /// <remarks>
        /// If no user-given documentation is present, a "good enough" default description is returned.
        /// </remarks>
        public string DocumentationText
        {
            get
            {
                return (string.IsNullOrEmpty(this.Documentation) ? string.Format("The {0}.", this.Name) : this.Documentation);
            }
        }

        /// <summary>
        /// Gets a full property documentation text based on the user-given <see cref="Documentation"/>.
        /// </summary>
        /// <remarks>
        /// This is used to document a .NET property in code (i.e. adds a "Gets" or "Gets or sets" prefix
        /// depending if the property is read-only).
        /// </remarks>
        public string DocumentationPropertyText
        {
            get
            {
                string propertyDocName = this.DocumentationText;
                string propertyDoc = (this.IsReadOnly ? "Gets " : "Gets or sets ");
                propertyDoc += NamingHelper.ToCamelCase(propertyDocName);
                return propertyDoc;
            }
        }

        /// <summary>
        /// Gets the type name of this property.
        /// </summary>
        public abstract string TypeName
        {
            get;
        }

        #endregion

		#region Calculated value for Custom Attributes

        private string GetCustomAttributesValue()
        {
            StringBuilder sb = new StringBuilder();
            if( Attributes.Count > 0 )
            {
                // List each attribute separated with a comma and a space
                foreach( Attribute attribute in Attributes )
                    sb.AppendFormat( "{0}, ", attribute.ToString() );

                // Remove the final comma and space
                sb.Remove( sb.Length - 2, 2 );

            }
            return sb.ToString();
        }

		#endregion

        #region Name Validation and Automatically Set XML Name

        internal sealed partial class NamePropertyHandler
        {
            protected override void OnValueChanged(ConfigurationProperty element, string oldValue, string newValue)
            {
                if (!element.Store.InUndoRedoOrRollback)
                {
                    // Hard validation of the new name.
                    if (string.IsNullOrEmpty(newValue))
                    {
                        throw new ArgumentException("The Name is required and cannot be an empty string.", newValue);
                    }

                    if (!NamingHelper.IsValidXmlName(newValue))
                    {
                        throw new ArgumentException(Resources.Error_InvalidName, newValue);
                    }

                    // When the name changes, set the XML name to the same name but camelCased.
                    element.XmlName = NamingHelper.ToCamelCase(element.Name);
                }

                // Always call the base class method.
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

        #endregion

        #region Automatically Set Required If Key

        internal sealed partial class IsKeyPropertyHandler
        {
            protected override void OnValueChanged(ConfigurationProperty element, bool oldValue, bool newValue)
            {
                if (!element.Store.InUndoRedoOrRollback)
                {
                    // If the Key property was set to true, make it Required as well.
                    if (newValue)
                    {
                        element.IsRequired = true;
                    }
                }

                // Always call the base class method.
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the Name property.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save)]
        private void ValidateName(ValidationContext context)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                context.LogError("The Name is required and cannot be an empty string.", "RequiredProperty", this);
            }
            if (!NamingHelper.IsValidName(this.Name))
            {
                context.LogError(Resources.Error_InvalidName, "RequiredProperty", this);
            }
        }

        /// <summary>
        /// Validates the XML Name.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save)]
        private void ValidateXmlName(ValidationContext context)
        {
            if (string.IsNullOrEmpty(this.XmlName) && !this.IsDefaultCollection)
            {
                context.LogError("The Xml Name is required and cannot be an empty string.", "RequiredProperty", this);
            }

            if (!NamingHelper.IsValidOrEmptyXmlName(this.XmlName))
            {
                context.LogError(Resources.Error_InvalidName, "RequiredProperty", this);
            }
        }

        /// <summary>
        /// Validates the fact that a Key property should also be Required.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save)]
        private void ValidateRequiredIfKey(ValidationContext context)
        {
            if (this.IsKey && !this.IsRequired)
            {
                context.LogError("A Key property must have Required set to True.", "KeyPropertyMustBeRequired", this);
            }
        }

        #endregion
    }
}