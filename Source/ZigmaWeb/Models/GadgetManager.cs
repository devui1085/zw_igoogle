using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigmaWeb.Models
{
    public class GadgetManager
    {
        public static Gadget GetByName(string systemName)
        {
            Gadget gadget;
            using (AppDataModelContainer context = new AppDataModelContainer()) {
                gadget = context.Gadgets.Where<Gadget>(g => g.SystemName == systemName).First();
            }

            return gadget;
        }

        public static Gadget GetById(int id)
        {
            AppDataModelContainer context = new AppDataModelContainer();
            return context.Gadgets.Where<Gadget>(g => g.Id == id).First();
        }

        public static Gadget AddGadget(Gadget g)
        {
            AppDataModelContainer context = new AppDataModelContainer();
            g = context.Gadgets.Add(g);
            context.SaveChanges();

            return g;
        }

        /// <summary>
        /// Removes a gadget from database. 
        /// </summary>
        /// <param name="g"></param>
        public static void RemoveGadget(Gadget g)
        {
            AppDataModelContainer context = new AppDataModelContainer();
            context.Gadgets.Remove(g);
            context.SaveChanges();
        }


    }
}