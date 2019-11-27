using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstatePropertyShared.Models
{
    public class RealEstateProperty
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Zpid { get; set; }
        public bool IsSaved { get; set; } = false;
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        public double? Price { get; set; }

        public string PriceAsString
        {
            get
            {
                if (Price.HasValue == false)
                {
                    return "0";
                }
                return Price.Value.ToString("C0", CultureInfo.CurrentCulture);
            }
        }

        public string PropertyType { get; set; }
        public int? Bedrooms { get; set; }
        public double? Bathrooms { get; set; }
        public int? FinishedSqFt { get; set; }
        public int? LotSizeSqFt { get; set; }
        public int? NumRooms { get; set; }
        public string Roof { get; set; }
        public string ExterialMaterial { get; set; }
        public string ParkingType { get; set; }
        public string HeatingSystem { get; set; }
        public string CoolingSystem { get; set; }
        public string FloorCovering { get; set; }
        public string Architecture { get; set; }
        public string Basement { get; set; }
        public string Appliances { get; set; }
        public int? NumFloors { get; set; }

        public string ImagesValues { get; set; }
        public List<String> Images { get; set; }

        public List<string> Strings
        {
            get { return Images; }
            set { Images = value; }
        }

        public string ImagesAsString
        {
            get { return String.Join(",", Images); }
            set { Images = value.Split(',').ToList(); }
        }

        public string HomeDescription { get; set; }

        public double DownPaymentPercentage { get; set; } = 0.20;
        public double? MorgagePerYear => (_rateOfInterest * LoanAmount.Value) / (1 - Math.Pow(1 + _rateOfInterest, _numOfPayments * -1));

        public double? PropertyTaxPerYear => Price.Value * 0.930 / 100;


        public double? RentPerYear {
            get 
            {
                
                if (Bedrooms.Value > 4)
                {
                    return 2000.0 * 12;
                }
                else if (Bedrooms.Value >= 2)
                {
                    return 1000.0 * 12;
                }
                else
                {
                    return 700.0 * 12;
                }
            }
        }
        public double? PropertyManagerPerYear => RentPerYear.Value * 0.10;
        public double? VacancyPerYear => RentPerYear.Value * 0.05;


        int _insurance_per_year = 110 * 12;
        int _repairs_per_year = 100 * 12;
        double _capital_expenses_per_year = 100 * 12;
        double _rateOfInterest = 5.0 / 1200;
        double _numOfPayments = 30 * 12;

        public double? NOI => RentPerYear.Value - (VacancyPerYear.Value + PropertyTaxPerYear.Value + PropertyManagerPerYear.Value + _insurance_per_year + _repairs_per_year + _capital_expenses_per_year);

        public string NOIAsString
        {
            get
            {
                if (NOI.HasValue == false)
                {
                    return "0";
                }
                return NOI.Value.ToString("C0", CultureInfo.CurrentCulture);
            }

        }
        public double? CapRate => (NOI.Value / Price.Value);
        public string CapRateAsString
        {
            get
            {
                if (CapRate.HasValue == false)
                {
                    return "0";
                }
                return CapRate.Value.ToString("P2", CultureInfo.CurrentCulture);
            }

        }
        public double? Cashflow => NOI.Value - MorgagePerYear.Value;

        public string CashflowAsString
        {
            get
            {
                if (Cashflow.HasValue == false)
                {
                    return "0";
                }
                return Cashflow.Value.ToString("C0", CultureInfo.CurrentCulture);
            }
        }

        public double? LoanAmount => Price.Value * 0.80;


        public double? COC => (Cashflow.Value / (Price.Value * DownPaymentPercentage + 6000));
        public string COCAsString
        {
            get
            {
                if (COC.HasValue == false)
                {
                    return "0";
                }
                return COC.Value.ToString("P2", CultureInfo.CurrentCulture);
            }

        }


    }
}
