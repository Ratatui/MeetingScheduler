
namespace MeetingScheduler.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Implements application logic using the MeetingSchedulerEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class SchedulerDomainService : LinqToEntitiesDomainService<MeetingSchedulerEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'AppointmentResources' query.
        public IQueryable<AppointmentResource> GetAppointmentResources()
        {
            return this.ObjectContext.AppointmentResources;
        }

        public void InsertAppointmentResources(AppointmentResource appointmentResources)
        {
            if ((appointmentResources.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(appointmentResources, EntityState.Added);
            }
            else
            {
                this.ObjectContext.AppointmentResources.AddObject(appointmentResources);
            }
        }

        public void UpdateAppointmentResources(AppointmentResource currentAppointmentResources)
        {
            this.ObjectContext.AppointmentResources.AttachAsModified(currentAppointmentResources, this.ChangeSet.GetOriginal(currentAppointmentResources));
        }

        public void DeleteAppointmentResources(AppointmentResource appointmentResources)
        {
            if ((appointmentResources.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(appointmentResources, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.AppointmentResources.Attach(appointmentResources);
                this.ObjectContext.AppointmentResources.DeleteObject(appointmentResources);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Resource' query.
        public IQueryable<Resource> GetResource()
        {
            return this.ObjectContext.Resource;
        }

        public void InsertResource(Resource resource)
        {
            if ((resource.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(resource, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Resource.AddObject(resource);
            }
        }

        public void UpdateResource(Resource currentResource)
        {
            this.ObjectContext.Resource.AttachAsModified(currentResource, this.ChangeSet.GetOriginal(currentResource));
        }

        public void DeleteResource(Resource resource)
        {
            if ((resource.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(resource, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Resource.Attach(resource);
                this.ObjectContext.Resource.DeleteObject(resource);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ResourceTypes' query.
        public IQueryable<Resource> GetResources()
        {
            return this.ObjectContext.Resource.Include("ResourceType");
        }

        public void InsertResourceTypes(ResourceType resourceTypes)
        {
            if ((resourceTypes.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(resourceTypes, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ResourceTypes.AddObject(resourceTypes);
            }
        }

        public void UpdateResourceTypes(ResourceType currentResourceTypes)
        {
            this.ObjectContext.ResourceTypes.AttachAsModified(currentResourceTypes, this.ChangeSet.GetOriginal(currentResourceTypes));
        }

        public void DeleteResourceTypes(ResourceType resourceTypes)
        {
            if ((resourceTypes.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(resourceTypes, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.ResourceTypes.Attach(resourceTypes);
                this.ObjectContext.ResourceTypes.DeleteObject(resourceTypes);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SqlAppointments' query.
        public IQueryable<SqlAppointment> GetSqlAppointments()
        {
            return this.ObjectContext.SqlAppointments.Include("AppointmentResources");
        }

        public void InsertSqlAppointments(SqlAppointment sqlAppointments)
        {
            if ((sqlAppointments.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sqlAppointments, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SqlAppointments.AddObject(sqlAppointments);
            }
        }

        public void UpdateSqlAppointments(SqlAppointment currentSqlAppointments)
        {
            this.ObjectContext.SqlAppointments.AttachAsModified(currentSqlAppointments, this.ChangeSet.GetOriginal(currentSqlAppointments));
        }

        public void DeleteSqlAppointments(SqlAppointment sqlAppointments)
        {
            if ((sqlAppointments.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sqlAppointments, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.SqlAppointments.Attach(sqlAppointments);
                this.ObjectContext.SqlAppointments.DeleteObject(sqlAppointments);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'sysdiagrams' query.
        public IQueryable<sysdiagrams> GetSysdiagrams()
        {
            return this.ObjectContext.sysdiagrams;
        }

        public void InsertSysdiagrams(sysdiagrams sysdiagrams)
        {
            if ((sysdiagrams.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sysdiagrams, EntityState.Added);
            }
            else
            {
                this.ObjectContext.sysdiagrams.AddObject(sysdiagrams);
            }
        }

        public void UpdateSysdiagrams(sysdiagrams currentsysdiagrams)
        {
            this.ObjectContext.sysdiagrams.AttachAsModified(currentsysdiagrams, this.ChangeSet.GetOriginal(currentsysdiagrams));
        }

        public void DeleteSysdiagrams(sysdiagrams sysdiagrams)
        {
            if ((sysdiagrams.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(sysdiagrams, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.sysdiagrams.Attach(sysdiagrams);
                this.ObjectContext.sysdiagrams.DeleteObject(sysdiagrams);
            }
        }
    }
}


