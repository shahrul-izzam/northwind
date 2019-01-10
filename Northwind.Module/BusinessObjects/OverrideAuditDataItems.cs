using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Northwind.Module.BusinessObjects
{
    public class OverrideAuditDataItems : AuditDataStore<AuditDataItemPersistent, AuditedObjectWeakReference> {
        
        public override void Save(DevExpress.Xpo.Session session, System.Collections.Generic.List<AuditDataItem> itemsToSave, IAuditTimestampStrategy timestampStrategy, string currentUserName) {
        
            UnitOfWork unitOfWork = session.DataLayer != null ? new UnitOfWork(session.DataLayer) : new UnitOfWork(session.ObjectLayer);
            Dictionary<object, AuditedObjectWeakReference> AuditedObjectWeakReferenceCache = new Dictionary<object, AuditedObjectWeakReference>();
        
            timestampStrategy.OnBeginSaveTransaction(unitOfWork);
        
            List<AuditDataItemPersistent> storeItemsToSave = new List<AuditDataItemPersistent>();
            List<AuditDataItem> sortedItemsToSave = new List<AuditDataItem>(itemsToSave);
        
            sortedItemsToSave.Sort(new AuditDataItemComparer());
            int correctionValue = 0;
        
            foreach (AuditDataItem item in sortedItemsToSave) {
                AuditDataItemPersistent auditDataStoreItem = null;

                if (item.OperationType == AuditOperationType.ObjectChanged) {
                    object auditObjectKey = session.GetKeyValue(item.AuditObject);
                    CriteriaOperator criteria = CriteriaOperator.Parse("AuditedObject.TargetKey = ? and OperationType = ? and UserName = ?", AuditedObjectWeakReference.KeyToString(auditObjectKey), GetDefaultStringRepresentation(item.OperationType), currentUserName);
                    auditDataStoreItem = unitOfWork.FindObject<AuditDataItemPersistent>(criteria);
                    if (auditDataStoreItem == null) {
                        auditDataStoreItem = (AuditDataItemPersistent)CreateAuditDataStoreItem(AuditedObjectWeakReferenceCache, unitOfWork, session, item, currentUserName);
                    }
                    auditDataStoreItem.Description = null;
                } else {
                    auditDataStoreItem = (AuditDataItemPersistent)CreateAuditDataStoreItem(AuditedObjectWeakReferenceCache, unitOfWork, session, item, currentUserName);
                }

                auditDataStoreItem.ModifiedOn = timestampStrategy.GetTimestamp(item).AddMilliseconds((correctionValue++) / 100000.0);
                storeItemsToSave.Add(auditDataStoreItem);
            }

            storeItemsToSave.Sort(new AuditDataStoreItemComparer<AuditedObjectWeakReference>());
            for (int i = 0; i < storeItemsToSave.Count; i++) {
                unitOfWork.Save(storeItemsToSave[i]);
            }

            unitOfWork.CommitChanges();
        }
}
}