//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExportManager.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class License_Item
    {
        public int Id { get; set; }
        public int License_Id { get; set; }
        public int Item_Id { get; set; }
        public int No_Units { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual License License { get; set; }
    }
}
