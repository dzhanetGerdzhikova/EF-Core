using CarDealer.Data;
using CarDealer.Dto.Import;
using CarDealer.Models;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using System;
using CarDealer.Dto.Export;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dataBase = new CarDealerContext();
            //CreateDataBase();

            //Problem 1
            //string suppliersXml = File.ReadAllText("Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(dataBase, suppliersXml));

            //Problem 2
            //string partsXml = File.ReadAllText("Datasets/parts.xml");
            //Console.WriteLine(ImportParts(dataBase, partsXml));

            //Problem 3
            //string carsXml = File.ReadAllText("Datasets/cars.xml");
            //Console.WriteLine(ImportCars(dataBase, carsXml));

            //Problem 4
            //string customerXml = File.ReadAllText("Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(dataBase,customerXml));

            //Problem 5
            //string salesXml = File.ReadAllText("Datasets/sales.xml");
            //Console.WriteLine(ImportSales(dataBase, salesXml));

            //Problem 6
            //string carWithDistanse = GetCarsWithDistance(dataBase);

            //Problem 7
            //string bmwCars = GetCarsFromMakeBmw(dataBase);

            //Problem 8
            //string localSuppliers = GetLocalSuppliers(dataBase);

            //Problem 9
            //string carsWithparts = GetCarsWithTheirListOfParts(dataBase);

            //Problem 10
            Console.WriteLine( GetTotalSalesByCustomer(dataBase));
        }
        public static void CreateDataBase()
        {
            var dataBase = new CarDealerContext();
            dataBase.Database.EnsureDeleted();
            dataBase.Database.EnsureCreated();
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SupplierImportDto>), new XmlRootAttribute("Suppliers"));

            var suppliersImport = (List<SupplierImportDto>)serializer.Deserialize(File.OpenText("Datasets/suppliers.xml"));

            var suppliers = suppliersImport.Select(s => new Supplier
            {
                Name = s.Name,
                IsImporter = s.IsImporter
            }).ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<PartImportDto>), new XmlRootAttribute("Parts"));

            var partsXml = (List<PartImportDto>)serializer.Deserialize(File.OpenRead("Datasets/parts.xml"));

            List<Part> parts = partsXml.Select(p => new Part
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                SupplierId = p.SupplierId
            }).ToList();

            foreach (var part in parts.Where(e => e.SupplierId < 32))
            {
                context.Parts.Add(part);
                context.SaveChanges();
            }

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CarImportDto>), new XmlRootAttribute("Cars"));

            List<CarImportDto> carsXml = (List<CarImportDto>)serializer.Deserialize(File.OpenRead("Datasets/cars.xml"));

            var cars = new List<Car>();

            foreach (var carXml in carsXml)
            {
                var uniqueParts = carXml.PartsId.Select(p => p.Id).Distinct();
                var realParts = uniqueParts.Where(id => context.Parts.Any(p => p.Id == id));

                var car = new Car
                {
                    Make = carXml.Make,
                    Model = carXml.Model,
                    TravelledDistance = carXml.TraveledDistance,
                    PartCars = realParts.Select(id => new PartCar
                    {
                        PartId = id
                    }).ToArray()
                };
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CustomerImportDto>), new XmlRootAttribute("Customers"));
            var customersXml = (List<CustomerImportDto>)serializer.Deserialize(File.OpenRead("datasets/customers.xml"));

            List<Customer> customers = customersXml.Select(c => new Customer
            {
                Name = c.Name,
                BirthDate = c.BirthDate,
                IsYoungDriver = c.IsYoungDriver
            }).ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SaleImportDto>), new XmlRootAttribute("Sales"));
            var salesXml = (List<SaleImportDto>)serializer.Deserialize(File.OpenRead("Datasets/sales.xml"));


            var existingCars = salesXml.Where(c => context.Cars.Any(cdb => cdb.Id == c.CarId));

            List<Sale> sales = existingCars.Select(c => new Sale
            {
                CarId = c.CarId,
                CustomerId = c.CustomerId,
                Discount = c.Discount
            }).ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            List<ExportCarDto> cars = context.Cars.Where(c => c.TravelledDistance > 2000000)
                .Select(c => new ExportCarDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                }).OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();


            var serializer = new XmlSerializer(typeof(List<ExportCarDto>));
            serializer.Serialize(File.OpenWrite("../../../Result/cars.xml"), cars);

            return "";
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsDb = context.Cars.Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var bmwCars = carsDb.Select(c => new ExportBmwCarDto
            {
                Id = c.Id,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance
            }).ToList();

            var serializer = new XmlSerializer(typeof(List<ExportBmwCarDto>), new XmlRootAttribute("cars"));
            serializer.Serialize(File.OpenWrite("../../../Result/bmw-cars.xml"), bmwCars);


            return "";
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersDb = context.Suppliers.Where(s => !s.IsImporter)
                .Select(s => new ExportSupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                }).ToList();

            var serializer = new XmlSerializer(typeof(List<ExportSupplierDto>), new XmlRootAttribute("suppliers"));

            using StringWriter textWriter = new StringWriter();
            serializer.Serialize(textWriter, suppliersDb);
            return textWriter.ToString();

            //serializer.Serialize(File.OpenWrite("../../../Result/local-suppliers.xml"), suppliersDb);

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars.Select(c => new ExportCarsWithPartsDto
            {
                Make = c.Make,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance,
                Parts = c.PartCars.Select(pc => new PartDto
                {
                    Name = pc.Part.Name,
                    Price = pc.Part.Price
                })
                .OrderByDescending(p => p.Price)
                .ToList()
            })
           .OrderByDescending(c => c.TravelledDistance)
           .ThenBy(c => c.Model)
           .Take(5)
           .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCarsWithPartsDto>), new XmlRootAttribute("cars"));

            using StringWriter writer = new StringWriter();
            serializer.Serialize(writer, cars);

            //serializer.Serialize(File.OpenWrite("../../../Result/cars-and-parts.xml"), cars);

            return writer.ToString();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            
            var customersDb = context.Sales.Where(c =>c.Customer.Sales.Count>0)
                .Select(c => new ExportCustomerSalesDto
                {
                    FullName = c.Customer.Name,
                    BoughtCars = c.Customer.Sales.Count,
                   SpentMoney = c.Car.PartCars.Select(pc=>pc.Part.Price).Sum()
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCustomerSalesDto>), new XmlRootAttribute("customers"));

            using StringWriter writer  = new StringWriter();

            serializer.Serialize(writer, customersDb);

            return writer.ToString();
        }
    }
}
