using System;
using System.Collections.Generic;
using System.Text;
using ConfigurationSectionDesigner.Properties;
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
            if (!NamingHelper.IsValidName(this.Name))
            {
                context.LogError(Resources.Error_InvalidName, "RequiredProperty", this);
            }
        }

        #endregion

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
                return (string.IsNullOrEmpty(this.Documentation) ? string.Format("{0}.", this.Name) : this.Documentation);
            }
        }

        #endregion
    }
}