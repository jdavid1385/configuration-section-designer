using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Modeling;

namespace ConfigurationSectionDesigner
{
    public partial class ConfigurationSectionModel
    {

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public ConfigurationSectionModel(Store store, params PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartition : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public ConfigurationSectionModel(Partition partition, params PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {

        }

        

        #endregion

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