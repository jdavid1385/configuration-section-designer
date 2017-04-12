using System;
using System.Collections.Generic;
using System.Text;
using ConfigurationSectionDesigner.Properties;
using Microsoft.VisualStudio.Modeling.Validation;

namespace ConfigurationSectionDesigner
{
    [ValidationState(ValidationState.Enabled)]
    public partial class TypeDefinition
    {
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
        /// Validates the Namespace property.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save)]
        private void ValidateNamespace(ValidationContext context)
        {
            if (string.IsNullOrEmpty(this.Namespace))
            {
                context.LogError("The Namespace is required and cannot be an empty string.", "RequiredProperty", this);
            }
            if (!NamingHelper.IsValidName(this.Name))
            {
                context.LogError(Resources.Error_InvalidNamespace, "RequiredProperty", this);
            }
        }

        #endregion
    }
}