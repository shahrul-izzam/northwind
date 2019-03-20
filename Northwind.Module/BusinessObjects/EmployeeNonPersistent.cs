using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace Northwind.Module.BusinessObjects
{
    [DomainComponent, DefaultClassOptions]
    public class EmployeeNonPersistent : IObjectSpaceLink, INotifyPropertyChanged, IXafEntityObject
    {
        int fEmployeeID;
        [DevExpress.ExpressApp.Data.Key]
        public int EmployeeID
        {
            get { return fEmployeeID; }
            set { fEmployeeID = value; }
        }
        string fLastName;
        [Indexed(Name = @"LastName")]
        [Size(20)]
        public string LastName
        {
            get { return fLastName; }
            set { fLastName = value; }
        }
        string fFirstName;
        [Size(10)]
        public string FirstName
        {
            get { return fFirstName; }
            set { fFirstName = value; }
        }
        string fTitle;
        [Size(30)]
        public string Title
        {
            get { return fTitle; }
            set { fTitle= value; }
        }
        string fTitleOfCourtesy;
        [Size(25)]
        public string TitleOfCourtesy
        {
            get { return fTitleOfCourtesy; }
            set { fTitleOfCourtesy= value; }
        }
        DateTime fBirthDate;
        public DateTime BirthDate
        {
            get { return fBirthDate; }
            set { fBirthDate= value; }
        }
        DateTime fHireDate;
        public DateTime HireDate
        {
            get { return fHireDate; }
            set { fHireDate= value; }
        }
        string fAddress;
        [Size(60)]
        public string Address
        {
            get { return fAddress; }
            set { fAddress= value; }
        }
        string fCity;
        [Size(15)]
        public string City
        {
            get { return fCity; }
            set { fCity= value; }
        }
        string fRegion;
        [Size(15)]
        public string Region
        {
            get { return fRegion; }
            set { fRegion= value; }
        }
        string fPostalCode;
        [Indexed(Name = @"PostalCode")]
        [Size(10)]
        public string PostalCode
        {
            get { return fPostalCode; }
            set { fPostalCode= value; }
        }
        string fCountry;
        [Size(15)]
        public string Country
        {
            get { return fCountry; }
            set { fCountry= value; }
        }
        string fHomePhone;
        [Size(24)]
        public string HomePhone
        {
            get { return fHomePhone; }
            set { fHomePhone= value; }
        }
        string fExtension;
        [Size(4)]
        public string Extension
        {
            get { return fExtension; }
            set { fExtension= value; }
        }
        byte[] fPhoto;
        [Size(SizeAttribute.Unlimited)]
        [MemberDesignTimeVisibility(true)]
        public byte[] Photo
        {
            get { return fPhoto; }
            set { fPhoto= value; }
        }
        string fNotes;
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get { return fNotes; }
            set { fNotes= value; }
        }
        string fPhotoPath;
        [Size(255)]
        public string PhotoPath
        {
            get { return fPhotoPath; }
            set { fPhotoPath= value; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IObjectSpace objectSpace;
        [Browsable(false)]
        public IObjectSpace ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }

        public void OnCreated()
        {
        }

        public void OnSaving()
        {
        }

        public void OnLoaded()
        {
        }
    }
}
