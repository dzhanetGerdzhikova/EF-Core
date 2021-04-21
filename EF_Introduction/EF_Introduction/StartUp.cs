using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUni = new SoftUniContext();
            //3
            var resultFromQuery3 = GetEmployeesFullInformation(softUni);
            Console.WriteLine(resultFromQuery3);
            //4
            var resultFromQuery4 = GetEmployeesWithSalaryOver50000(softUni);
            Console.WriteLine(resultFromQuery4);
            //5
            var resultFromQuery5 = GetEmployeesFromResearchAndDevelopment(softUni);
            Console.WriteLine(resultFromQuery5);
            //6
            var resultFromQuery6 = AddNewAddressToEmployee(softUni);
            Console.WriteLine(resultFromQuery6);
            //7
            var resultFromQuery7 = GetEmployeesInPeriod(softUni);
            Console.WriteLine(resultFromQuery7);
            //8
            var resultFromQuery8 = GetAddressesByTown(softUni);
            Console.WriteLine(resultFromQuery8);
            //9
            var resultFromQuery9 = GetEmployee147(softUni);
            Console.WriteLine(resultFromQuery9);
            //10
            var resultFromQuery10 = GetDepartmentsWithMoreThan5Employees(softUni);
            Console.WriteLine(resultFromQuery10);
            //11
            var resultFromQuery11 = GetLatestProjects(softUni);
            Console.WriteLine(resultFromQuery11);
            //12
            var resultFromQuery12 = IncreaseSalaries(softUni);
            Console.WriteLine(resultFromQuery12);
            //13
            var resultFromQuery13 = GetEmployeesByFirstNameStartingWithSa(softUni);
            Console.WriteLine(resultFromQuery13);
            //14
            var resultFromQuery14 = DeleteProjectById(softUni);
            Console.WriteLine(resultFromQuery14);
            //15
            var resultFromQuery15 = RemoveTown(softUni);
            Console.WriteLine(resultFromQuery15);
        }

        //Problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            context = new SoftUniContext();
            //var employyes = context.Employees.Select(x => x.FirstName + " " + x.LastName + " " + x.MiddleName + " " + x.JobTitle + " " + x.Salary.ToString("f2")).OrderBy(e => e.EmployeeId).ToList();
            var employees = context.Employees.OrderBy(e => e.EmployeeId);

            foreach (var currentEmpl in employees)
            {
                result.AppendLine(currentEmpl.ToString());
            }
            return result.ToString().TrimEnd();
        }

        //Problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            //context = new SoftUniContext();
            StringBuilder result = new StringBuilder();

            var employees = context.Employees.Where(x => x.Salary > 50000).OrderBy(e => e.FirstName).Select(e => new
            {
                e.FirstName,
                e.Salary
            }).ToList();

            foreach (var em in employees)
            {
                result.AppendLine($"{em.FirstName} - {em.Salary:F2}");
            }
            return result.ToString().TrimEnd();
        }

        //Problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            var employees = context.Employees.
                   Where(e => e.Department.Name == "Research and Development").
                   Select(e => new { e.FirstName, e.LastName, DepartmentName = e.Department.Name, e.Salary }).
                   OrderBy(e => e.Salary).
                   ThenByDescending(e => e.FirstName).
                   ToList();
            foreach (var em in employees)
            {
                result.AppendLine($"{em.FirstName} {em.LastName} from {em.DepartmentName} - ${em.Salary:f2}");
            }
            return result.ToString().TrimEnd();
        }

        //Problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var address = new Address()
            { AddressText = "Vitoshka 15", TownId = 4 };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
            employee.Address = address;

            context.SaveChanges();

            var addresses = context.Employees.OrderByDescending(e => e.AddressId).Take(10).Select(e =>
             e.Address.AddressText).ToList();


            foreach (var adr in addresses)
            {
                result.AppendLine(adr);

            }

            return result.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var empl = context.Employees
                .Where(e => e.EmployeesProjects
                       .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFName = e.Manager.FirstName,
                    ManagetLName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        PrName = ep.Project.Name,
                        PrStart = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        PrEnd = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                    })
                })
                .ToList();


            //var emProj = context.EmployeesProjects.Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003).Take(10).Select(ep => new
            //{
            //    ep.Employee.FirstName,
            //    ep.Employee.LastName,
            //    ManagerFirstName = ep.Employee.Manager.FirstName,
            //    ManagerLastName = ep.Employee.Manager.LastName,
            //    ProjectName = ep.Project.Name,
            //    ProjectStart = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture),
            //    ProjectEnd = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
            //}).ToList();

            //foreach (var em in emProj)
            //{
            //    result.AppendLine($"{em.ManagerFirstName} {em.ManagerLastName} - Manager: {em.FirstName} {em.LastName}");
            //    foreach (var pr in em.pro)
            //    {

            //    }
            //    result.AppendLine($"--{em.ProjectName} - {em.ProjectStart} - {em.ProjectEnd}");
            //}
            //return result.ToString().TrimEnd();



            foreach (var em in empl)
            {
                result.AppendLine($"{em.FirstName} {em.LastName} - Manager: {em.ManagerFName} {em.ManagetLName}");

                foreach (var pr in em.Projects)
                {
                    result.AppendLine($"-- {pr.PrName} - {pr.PrStart} - {pr.PrEnd} ");
                }
            }

            return result.ToString().TrimEnd();
        }

        //Problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var adresses = context.Addresses.Select(a => new
            {
                Name = a.AddressText,
                Town = a.Town.Name,
                CountEmployees = a.Employees.Count()

            })
                .OrderByDescending(a => a.CountEmployees)
                .ThenBy(a => a.Name)
                .ThenBy(a => a.Town)
                .Take(10)
                .ToList();

            foreach (var adr in adresses)
            {
                result.AppendLine($"{adr.Name}, {adr.Town} - {adr.CountEmployees} employees");
            }

            return result.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employee = context.Employees.Where(e => e.EmployeeId == 147).Select
                (e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                    .Select(ep => ep.Project.Name)
                    .OrderBy(name => name)
                    .ToList()
                })
                .Single();

            result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var pr in employee.Projects)
            {
                result.AppendLine(pr);
            }
            return result.ToString().TrimEnd();
        }

        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFName = d.Manager.FirstName,
                    ManagerLName = d.Manager.LastName,
                    Employees = d.Employees.Select(
                               e => new
                               {
                                   EmplFirstName = e.FirstName,
                                   EmplLastName = e.LastName,
                                   JobTitle = e.JobTitle,
                               })
                             .OrderBy(e => e.EmplFirstName)
                             .ThenBy(e => e.EmplLastName).ToList(),
                })
                .ToList();

            foreach (var d in departments)
            {
                result.AppendLine($"{d.Name} - {d.ManagerFName} {d.ManagerLName}");

                foreach (var em in d.Employees)
                {
                    result.AppendLine($"{em.EmplFirstName} {em.EmplLastName} - {em.JobTitle}");
                }
            }

            return result.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projects = context.Projects.OrderByDescending(p => p.StartDate).Take(10).OrderBy(p => p.Name).ToList();

            foreach (var pr in projects)
            {
                result.AppendLine(pr.Name);
                result.AppendLine(pr.Description);
                result.AppendLine(pr.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return result.ToString().TrimEnd();
        }


        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employeesToEncrease = context.Employees
                .Where(e =>
                e.Department.Name == "Engineering" ||
                e.Department.Name == "Tool Design" ||
                e.Department.Name == "Marketing" ||
                e.Department.Name == "Information Services ")
                .OrderBy(e => e.FirstName)
                .ThenByDescending(e => e.LastName);

            foreach (var em in employeesToEncrease)
            {
                em.Salary *= 1.12m;

            }
            context.SaveChanges();

            var empl = employeesToEncrease
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .ToList();

            foreach (var emp in empl)
            {
                result.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return result.ToString().TrimEnd();
        }

        //Problem13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            string letters = "Sa".ToUpper();

            var employees = context.Employees
                .Where(e => e.FirstName.ToUpper().StartsWith(letters))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var em in employees)
            {
                result.AppendLine($"{em.FirstName} {em.LastName} - {em.JobTitle} - (${em.Salary:f2})");
            }

            return result.ToString().TrimEnd();
        }

        // Problem14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projectId2 = context.Projects
                .Single(p => p.ProjectId == 2);

            var emProjId2 = context.EmployeesProjects.Single(ep => ep.ProjectId == projectId2.ProjectId);

            context.EmployeesProjects.Remove(emProjId2);

            context.Projects.Remove(projectId2);

            context.SaveChanges();

            var projectsName = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList();

            foreach (var name in projectsName)
            {
                result.AppendLine(name);
            }

            return result.ToString().TrimEnd();
        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns.FirstOrDefault(t => t.Name == "Seattle");

            var addresses = context.Addresses.Where(a => a.TownId == townToDelete.TownId);

            int countOfAddresses = addresses.Count();

            var employyes = context.Employees.Where(e => e.Address.TownId == townToDelete.TownId);

            foreach (var em in employyes)
            {
                em.Address = null;
            }

            foreach (var ad in addresses)
            {
                context.Addresses.Remove(ad);
            }

            context.Towns.Remove(townToDelete);
            context.SaveChanges();

            return $"{countOfAddresses} addresses in {townToDelete.Name} were deleted";
        }
    }
}
