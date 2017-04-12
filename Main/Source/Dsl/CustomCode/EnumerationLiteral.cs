using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;

namespace ConfigurationSectionDesigner
{
    [ValidationState(ValidationState.Enabled)]
    public partial class EnumerationLiteral
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
        }

        #endregion
    }
}