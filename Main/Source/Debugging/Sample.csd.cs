//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4952
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sample
{
    
    
    /// <summary>
    /// The SampleConfigurationSection Configuration Section.
    /// </summary>
    public partial class SampleConfigurationSection : global::System.Configuration.ConfigurationSection
    {
        
        #region Singleton Instance
        /// <summary>
        /// The XML name of the SampleConfigurationSection Configuration Section.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string SampleConfigurationSectionSectionName = "sampleConfigurationSection";
        
        /// <summary>
        /// Gets the SampleConfigurationSection instance.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public static global::Sample.SampleConfigurationSection Instance
        {
            get
            {
                return ((global::Sample.SampleConfigurationSection)(global::System.Configuration.ConfigurationManager.GetSection(global::Sample.SampleConfigurationSection.SampleConfigurationSectionSectionName)));
            }
        }
        #endregion
        
        #region Xmlns Property
        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string XmlnsPropertyName = "xmlns";
        
        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.SampleConfigurationSection.XmlnsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[global::Sample.SampleConfigurationSection.XmlnsPropertyName]));
            }
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region Samples Property
        /// <summary>
        /// The XML name of the <see cref="Samples"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string SamplesPropertyName = "samples";
        
        /// <summary>
        /// Gets or sets the Samples.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Samples.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.SampleConfigurationSection.SamplesPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public int Samples
        {
            get
            {
                return ((int)(base[global::Sample.SampleConfigurationSection.SamplesPropertyName]));
            }
            set
            {
                base[global::Sample.SampleConfigurationSection.SamplesPropertyName] = value;
            }
        }
        #endregion
        
        #region Foo Property
        /// <summary>
        /// The XML name of the <see cref="Foo"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string FooPropertyName = "foo";
        
        /// <summary>
        /// Gets or sets the Foo.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Foo.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.SampleConfigurationSection.FooPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public global::Sample.Foo Foo
        {
            get
            {
                return ((global::Sample.Foo)(base[global::Sample.SampleConfigurationSection.FooPropertyName]));
            }
            set
            {
                base[global::Sample.SampleConfigurationSection.FooPropertyName] = value;
            }
        }
        #endregion
        
        #region Bars Property
        /// <summary>
        /// The XML name of the <see cref="Bars"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string BarsPropertyName = "bars";
        
        /// <summary>
        /// Gets or sets the Bars.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Bars.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.SampleConfigurationSection.BarsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public global::Sample.Bars Bars
        {
            get
            {
                return ((global::Sample.Bars)(base[global::Sample.SampleConfigurationSection.BarsPropertyName]));
            }
            set
            {
                base[global::Sample.SampleConfigurationSection.BarsPropertyName] = value;
            }
        }
        #endregion
    }
}
namespace Sample
{
    
    
    /// <summary>
    /// The Foo Configuration Element.
    /// </summary>
    public partial class Foo : global::System.Configuration.ConfigurationElement
    {
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region Baz Property
        /// <summary>
        /// The XML name of the <see cref="Baz"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string BazPropertyName = "baz";
        
        /// <summary>
        /// Gets or sets the Baz.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Baz.")]
        [global::System.ComponentModel.TypeConverter(typeof(global::Sample.CustomTypeTypeConverter))]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.Foo.BazPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public global::Debugging.CustomType Baz
        {
            get
            {
                return ((global::Debugging.CustomType)(base[global::Sample.Foo.BazPropertyName]));
            }
            set
            {
                base[global::Sample.Foo.BazPropertyName] = value;
            }
        }
        #endregion
    }
}
namespace Sample
{
    
    
    /// <summary>
    /// A collection of Bar instances.
    /// </summary>
    [global::System.Configuration.ConfigurationCollectionAttribute(typeof(global::Sample.Bar), CollectionType=global::System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMapAlternate, AddItemName="addBar", RemoveItemName="removeBar", ClearItemsName="clearBars")]
    public partial class Bars : global::System.Configuration.ConfigurationElementCollection
    {
        
        #region Constants
        /// <summary>
        /// The XML name of the individual <see cref="global::Sample.Bar"/> instances in this collection.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string BarPropertyName = "bar";
        #endregion
        
        #region Overrides
        /// <summary>
        /// Gets the type of the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>The <see cref="global::System.Configuration.ConfigurationElementCollectionType"/> of this collection.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override global::System.Configuration.ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return global::System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMapAlternate;
            }
        }
        
        /// <summary>
        /// Gets the name used to identify this collection of elements
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        protected override string ElementName
        {
            get
            {
                return global::Sample.Bars.BarPropertyName;
            }
        }
        
        /// <summary>
        /// Indicates whether the specified <see cref="global::System.Configuration.ConfigurationElement"/> exists in the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="elementName">The name of the element to verify.</param>
        /// <returns>
        /// <see langword="true"/> if the element exists in the collection; otherwise, <see langword="false"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        protected override bool IsElementName(string elementName)
        {
            return (elementName == global::Sample.Bars.BarPropertyName);
        }
        
        /// <summary>
        /// Gets the element key for the specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="global::System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="global::System.Configuration.ConfigurationElement"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        protected override object GetElementKey(global::System.Configuration.ConfigurationElement element)
        {
            return ((global::Sample.Bar)(element)).Crackle;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Sample.Bar"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Sample.Bar"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        protected override global::System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new global::Sample.Bar();
        }
        #endregion
        
        #region Indexer
        /// <summary>
        /// Gets the <see cref="global::Sample.Bar"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Sample.Bar"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public global::Sample.Bar this[int index]
        {
            get
            {
                return ((global::Sample.Bar)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Sample.Bar"/> with the specified key.
        /// </summary>
        /// <param name="crackle">The key of the <see cref="global::Sample.Bar"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public global::Sample.Bar this[object crackle]
        {
            get
            {
                return ((global::Sample.Bar)(base.BaseGet(crackle)));
            }
        }
        #endregion
        
        #region Add
        /// <summary>
        /// Adds the specified <see cref="global::Sample.Bar"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="bar">The <see cref="global::Sample.Bar"/> to add.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public void Add(global::Sample.Bar bar)
        {
            base.BaseAdd(bar);
        }
        #endregion
        
        #region Remove
        /// <summary>
        /// Removes the specified <see cref="global::Sample.Bar"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="bar">The <see cref="global::Sample.Bar"/> to remove.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public void Remove(global::Sample.Bar bar)
        {
            base.BaseRemove(this.GetElementKey(bar));
        }
        #endregion
        
        #region GetItem
        /// <summary>
        /// Gets the <see cref="global::Sample.Bar"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Sample.Bar"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public global::Sample.Bar GetItemAt(int index)
        {
            return ((global::Sample.Bar)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Sample.Bar"/> with the specified key.
        /// </summary>
        /// <param name="crackle">The key of the <see cref="global::Sample.Bar"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public global::Sample.Bar GetItemByKey(float crackle)
        {
            return ((global::Sample.Bar)(base.BaseGet(((object)(crackle)))));
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
    }
}
namespace Sample
{
    
    
    /// <summary>
    /// The Bar Configuration Element.
    /// </summary>
    public partial class Bar : global::System.Configuration.ConfigurationElement
    {
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region Snap Property
        /// <summary>
        /// The XML name of the <see cref="Snap"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string SnapPropertyName = "snap";
        
        /// <summary>
        /// Gets or sets the Snap.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Snap.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.Bar.SnapPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public bool Snap
        {
            get
            {
                return ((bool)(base[global::Sample.Bar.SnapPropertyName]));
            }
            set
            {
                base[global::Sample.Bar.SnapPropertyName] = value;
            }
        }
        #endregion
        
        #region Crackle Property
        /// <summary>
        /// The XML name of the <see cref="Crackle"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        internal const string CracklePropertyName = "crackle";
        
        /// <summary>
        /// Gets or sets the Crackle.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Crackle.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Sample.Bar.CracklePropertyName, IsRequired=true, IsKey=true, IsDefaultCollection=false)]
        public float Crackle
        {
            get
            {
                return ((float)(base[global::Sample.Bar.CracklePropertyName]));
            }
            set
            {
                base[global::Sample.Bar.CracklePropertyName] = value;
            }
        }
        #endregion
    }
}
namespace Sample
{
    
    
    /// <summary>
    /// LetterFlags.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
    [global::System.FlagsAttribute()]
    public enum LetterFlags
    {
        
        /// <summary>
        /// A.
        /// </summary>
        A = 1,
        
        /// <summary>
        /// B.
        /// </summary>
        B = 2,
        
        /// <summary>
        /// C.
        /// </summary>
        C = 4,
        
        /// <summary>
        /// D.
        /// </summary>
        D = 8,
    }
}
namespace Sample
{
    
    
    /// <summary>
    /// CustomTypeTypeConverter Custom Converter
    /// </summary>
    public partial class CustomTypeTypeConverter : global::System.Configuration.ConfigurationConverterBase
    {
        
        /// <summary>
        /// Converts from <see cref="string" /> to <see cref="global::Debugging.CustomType" />.
        /// </summary>
        /// <param name="context">The <see cref="global::System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="global::System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="string" /> to convert from.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override object ConvertFrom(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Globalization.CultureInfo culture, object value)
        {
            // IMPORTANT NOTE: The code below does not build by default.
            // This is a custom type validator that must be implemented
            // for it to build. Place the following in a separate file
            // and implement the method.
            // 
            // public partial class CustomTypeTypeConverter
            // {
            //     
            //     private global::Debugging.CustomType ConvertFromStringToCustomType(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Globalization.CultureInfo culture, string value)
            //     {
            //         throw new global::System.NotImplementedException();
            //     }
            // }
            // 
            return this.ConvertFromStringToCustomType(context, culture, ((string)(value)));
        }
        
        /// <summary>
        /// Converts from <see cref="global::Debugging.CustomType" /> to <see cref="string" />.
        /// </summary>
        /// <param name="context">The <see cref="global::System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="global::System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="string" /> to convert from.</param>
        /// <param name="type">The <see cref="global::System.Type" /> to convert the value parameter to.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "1.6.1.0")]
        public override object ConvertTo(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Globalization.CultureInfo culture, object value, global::System.Type type)
        {
            // IMPORTANT NOTE: The code below does not build by default.
            // This is a custom type validator that must be implemented
            // for it to build. Place the following in a separate file
            // and implement the method.
            // 
            // public partial class CustomTypeTypeConverter
            // {
            //     
            //     private string ConvertToCustomTypeFromString(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Globalization.CultureInfo culture, global::Debugging.CustomType value, global::System.Type type)
            //     {
            //         return value.ToString();
            //     }
            // }
            // 
            return this.ConvertToCustomTypeFromString(context, culture, ((global::Debugging.CustomType)(value)), type);
        }
    }
}
