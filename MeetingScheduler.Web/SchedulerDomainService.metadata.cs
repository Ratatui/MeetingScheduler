
namespace MeetingScheduler.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies AppointmentResourcesMetadata as the class
    // that carries additional metadata for the AppointmentResources class.
    [MetadataTypeAttribute(typeof(AppointmentResource.AppointmentResourcesMetadata))]
    public partial class AppointmentResource
    {

        // This class allows you to attach custom attributes to properties
        // of the AppointmentResources class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class AppointmentResourcesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AppointmentResourcesMetadata()
            {
            }

            [Key]
            public int Id { get; set; }

            public Resource Resource { get; set; }

            public int ResourceId { get; set; }

            public Guid SqlAppointmentId { get; set; }

            public SqlAppointment SqlAppointments { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ResourceMetadata as the class
    // that carries additional metadata for the Resource class.
    [MetadataTypeAttribute(typeof(Resource.ResourceMetadata))]
    public partial class Resource
    {

        // This class allows you to attach custom attributes to properties
        // of the Resource class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ResourceMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ResourceMetadata()
            {
            }

            public EntityCollection<AppointmentResource> AppointmentResources { get; set; }

            public string DisplayName { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }

            public int ResourceTypeId { get; set; }

            [Include]
            public ResourceType ResourceTypes { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ResourceTypesMetadata as the class
    // that carries additional metadata for the ResourceTypes class.
    [MetadataTypeAttribute(typeof(ResourceType.ResourceTypesMetadata))]
    public partial class ResourceType
    {

        // This class allows you to attach custom attributes to properties
        // of the ResourceTypes class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ResourceTypesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ResourceTypesMetadata()
            {
            }

            public Nullable<bool> AllowMultipleSelection { get; set; }

            public string Color { get; set; }

            public string DisplayName { get; set; }

            [Key]
            public int Id { get; set; }

            public string Name { get; set; }

            public EntityCollection<Resource> Resource { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies SqlAppointmentsMetadata as the class
    // that carries additional metadata for the SqlAppointments class.
    [MetadataTypeAttribute(typeof(SqlAppointment.SqlAppointmentsMetadata))]
    public partial class SqlAppointment
    {

        // This class allows you to attach custom attributes to properties
        // of the SqlAppointments class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SqlAppointmentsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SqlAppointmentsMetadata()
            {
            }

            [Include]
            public EntityCollection<AppointmentResource> AppointmentResources { get; set; }

            public string Body { get; set; }

            public string Category { get; set; }

            public DateTime End { get; set; }

            public Nullable<DateTime> ExceptionDate { get; set; }

            [Key]
            public Guid Id { get; set; }

            public string Importance { get; set; }

            public bool IsAllDayEvent { get; set; }

            public string Location { get; set; }

            public Nullable<Guid> ParentId { get; set; }

            public string RecurrencePattern { get; set; }

            public EntityCollection<SqlAppointment> SqlAppointments1 { get; set; }

            public SqlAppointment SqlAppointments2 { get; set; }

            public DateTime Start { get; set; }

            public string Subject { get; set; }

            public string TimeMarker { get; set; }

            public string TimeZoneString { get; set; }

            public int Type { get; set; }

            public string Url { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies sysdiagramsMetadata as the class
    // that carries additional metadata for the sysdiagrams class.
    [MetadataTypeAttribute(typeof(sysdiagrams.sysdiagramsMetadata))]
    public partial class sysdiagrams
    {

        // This class allows you to attach custom attributes to properties
        // of the sysdiagrams class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class sysdiagramsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private sysdiagramsMetadata()
            {
            }

            public byte[] definition { get; set; }

            public int diagram_id { get; set; }

            public string name { get; set; }

            public int principal_id { get; set; }

            public Nullable<int> version { get; set; }
        }
    }
}
