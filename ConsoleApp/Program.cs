using NinjaDemo.DataModel;
using NinjaDomain.Classes;
using NinjaDomain.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertMultipleNinjas();
            //SimpleNinjaQueries();
            //QueryAndUpdateNinja();
            //DeleteNinja();
            //InsertNinjaWithEquipment();
            SimpleNinjaGraphQuery();
            Console.ReadKey();            
        }

        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Leonardo",
                ServedInWaiban = false,
                DateOfBirth = new DateTime(1984, 1, 1),
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "Raphael",
                ServedInWaiban = false,
                DateOfBirth = new DateTime(1985, 1, 1),
                ClanId = 1
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja> { ninja1, ninja2 });
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQueries()
        {
            using(var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.ToList();
                ////Just a query
                //var query = context.Ninjas;
                //// can trigger query by enumeration
                //foreach (var ninja in query)
                //{
                //    Console.WriteLine(ninja.Name);
                //}
            }
        }

        private static void QueryAndUpdateNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                ninja = context.Ninjas.FirstOrDefault();               
            }

            ninja.ServedInWaiban = !ninja.ServedInWaiban;            

            using (var context = new NinjaContext())
            {
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                ninja = context.Ninjas.FirstOrDefault();
            }

            using (var context = new NinjaContext())
            {
                //context.Ninjas.Attach(ninja);
                //context.Ninjas.Remove(ninja);

                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }

        }

        private static void InsertNinjaWithEquipment()
        {
            
            using (var context = new NinjaContext())
            {
                Ninja ninja = new Ninja
                {
                    Name = "Katy Catanzaro",
                    ServedInWaiban = false,
                    DateOfBirth = new DateTime(1990, 1, 14),
                    ClanId = 1
                };

                var eq = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool
                };

                var eq1 = new NinjaEquipment
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };

                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.AddRange(new List<NinjaEquipment> { eq, eq1});
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                // Eager loading
                //var ninja = context.Ninjas.Include(n => n.EquipmentOwned)
                //    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));
                
                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));

                // Explicit loading
                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }
        }
    }
}
