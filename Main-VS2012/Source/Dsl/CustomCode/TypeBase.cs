using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigurationSectionDesigner.Properties;

namespace ConfigurationSectionDesigner
{
    public abstract partial class TypeBase
    {
        #region Convenience Properties

        /// <summary>
        /// Gets the full type name of this configuration type.
        /// </summary>
        public virtual string FullName
        {
            get
            {
                return string.Format( "{0}.{1}", this.Namespace, this.Name );
            }
        }

        #endregion

        #region NameChanged Event (for validation and for derived classes to set XML name)

        /// <summary>
        /// Occurs when the name of the ConfigurationElement changed.
        /// </summary>
        protected event EventHandler<EventArgs> NameChanged;

        /// <summary>
        /// Raises the <see cref="E:NameChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnNameChanged( EventArgs e )
        {
            if( this.NameChanged != null )
            {
                this.NameChanged( this, EventArgs.Empty );
            }
        }

        internal sealed partial class NamePropertyHandler
        {
            protected override void OnValueChanged( TypeBase element, string oldValue, string newValue )
            {
                if( !element.Store.InUndoRedoOrRollback )
                {
                    // Hard validation of the new name.
                    if( string.IsNullOrEmpty( newValue ) )
                    {
                        throw new ArgumentException( "The Name is required and cannot be an empty string.", newValue );
                    }
                    if (!NamingHelper.IsValidName(newValue))
                    {
                        throw new ArgumentException(Resources.Error_InvalidName, newValue);
                    }
                    // Raise the NameChanged event for derived classes to act upon.
                    element.OnNameChanged( EventArgs.Empty );
                }

                // Always call the base class method.
                base.OnValueChanged( element, oldValue, newValue );
            }
        }

        #endregion
    }
}
