using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using Northwind.Module.BusinessObjects;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Module.Controllers
{
    public class InitializeNonPersistentListViewController : WindowController
    {
        private static IList<EmployeeNonPersistent> objectsCache;

        public InitializeNonPersistentListViewController() : base() {
            TargetWindowType = WindowType.Main;
        }

        protected override void OnActivated() {
            base.OnActivated();
            Application.ObjectSpaceCreated += ApplicationOnObjectSpaceCreated;
        }

        private void ApplicationOnObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            if (e.ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace) {
                nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpaceOnObjectsGetting;
                nonPersistentObjectSpace.ObjectByKeyGetting += NonPersistentObjectSpaceOnObjectByKeyGetting;
                nonPersistentObjectSpace.ObjectGetting += NonPersistentObjectSpaceOnObjectGetting;
                nonPersistentObjectSpace.Committing += NonPersistentObjectSpaceOnCommitting;
            }
        }

        private void NonPersistentObjectSpaceOnObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
            BindingList<EmployeeNonPersistent> objects = new BindingList<EmployeeNonPersistent>
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };

            var objectSpace = Application.CreateObjectSpace();
            var employees = objectSpace.GetObjects<Employees>();
            foreach (var employee in employees)
            {
                var employeeNonPersistent = new EmployeeNonPersistent
                {
                    LastName = employee.LastName,
                    Address = employee.Address,
                    BirthDate = employee.BirthDate,
                    City = employee.City,
                    Country = employee.Country,
                    EmployeeID = employee.EmployeeID,
                    Extension = employee.Extension,
                    FirstName = employee.FirstName,
                    HireDate = employee.HireDate,
                    HomePhone = employee.HomePhone,
                    Photo = employee.Photo,
                    PhotoPath = employee.PhotoPath,
                    PostalCode = employee.PostalCode,
                    Region = employee.Region,
                    Title = employee.Title,
                    TitleOfCourtesy = employee.TitleOfCourtesy
                };
                objects.Add(employeeNonPersistent);
            }

            objectsCache = objects;
            e.Objects = objects;
        }

        private void NonPersistentObjectSpaceOnCommitting(object sender, CancelEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectSpace.ModifiedObjects) {
                if (obj is EmployeeNonPersistent) {
                    if (objectSpace.IsNewObject(obj)) {
                        objectsCache.Add((EmployeeNonPersistent) obj);
                    } else if (objectSpace.IsDeletedObject(obj)) {
                        objectsCache.Remove((EmployeeNonPersistent) obj);
                    }
                }
            }
        }

        private void NonPersistentObjectSpaceOnObjectGetting(object sender, ObjectGettingEventArgs e)
        {
            if (e.SourceObject is IObjectSpaceLink) {
                ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
            }
        }

        private void NonPersistentObjectSpaceOnObjectByKeyGetting(object sender, ObjectByKeyGettingEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectsCache) {
                if (obj.GetType() == e.ObjectType && Equals(objectSpace.GetKeyValue(obj), e.Key)) {
                    e.Object = objectSpace.GetObject(obj);
                    break;
                }
            }
        }
        
        protected override void OnDeactivated() {
            base.OnDeactivated();
            Application.ObjectSpaceCreated -= ApplicationOnObjectSpaceCreated;
        }
    }
}
