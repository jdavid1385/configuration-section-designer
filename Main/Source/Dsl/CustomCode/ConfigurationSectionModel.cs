using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Modeling;

namespace ConfigurationSectionDesigner
{
    public partial class ConfigurationSectionModel
    {
        #region Validation

        /// <summary>
        /// Validates the Namespaces.
        /// </summary>
        /// <param name="context">The validation context.</param>
        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save)]
        private void ValidateNamespaces(ValidationContext context)
        {
            if (string.IsNullOrEmpty(this.Namespace))
            {
                context.LogError("The Namespace is required and cannot be an empty string.", "RequiredProperty", this);
            }
            if (string.IsNullOrEmpty(this.XmlSchemaNamespace))
            {
                context.LogError("The XML Schema Namespace is required and cannot be an empty string.", "RequiredProperty", this);
            }
        }

        #endregion
    }
}