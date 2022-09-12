
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Models
{
    public class JsonModelReturnViewCustomer
    {
        public Customer Customer { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewCountry
    {
        public CountryViewModel Country { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewProvince
    {
        public ProvinceViewModel Province { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewWard
    {
        public WardViewModel Ward { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewDistrict
    {
        public DistrictViewModel District { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewStreet
    {
        public StreetViewModel Street { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewCustomerCategory
    {
        public CustomerCategoryViewModel CustomerCategory { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewEstate_Project
    {
        public Estate_ProjectViewModel Estate_Project { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewEstate_Investor
    {
        public Estate_InvestorViewModel Estate_Investor { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewCertificate_Type
    {
        public Certificate_TypeViewModel Certificate_Type { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewEstate_Status
    {
        public Estate_StatusViewModel Estate_Status { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
    public class JsonModelReturnViewEstate_Group
    {
        public Estate_GroupViewModel Estate_Group { get; set; }
        public bool isError { get; set; }
        public bool createSuccess { get; set; }
        public bool editSuccess { get; set; }
        public bool isExit { get; set; }
        public string messages { get; set; }
    }
}
