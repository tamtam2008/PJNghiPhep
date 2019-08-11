﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCGrid.Models;
using MVCGrid.Web;
using ProjectNghiPhep.Models;

namespace ProjectNghiPhep.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /DonNghiPhep/
           
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            var result = new QueryResult<User>();
            NghiphepEntities db = new NghiphepEntities();
            // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
            // Use Entity Framework, a module from your IoC Container, or any other method.
            // Return QueryResult object containing IEnumerable<YouModelItem>
            var query = (from user in db.Users
                         join contract in db.ContractTypes
                         on user.contractId equals contract.C_id
                         where user.C_id == id
                         select new
                         {
                             C_id = user.C_id,
                             username = user.username,
                             dayOff = user.dayOff,
                             contractId = user.contractId,
                             createdAt = user.createdAt,
                             ContractType = contract,
                             fullName = user.fullName,
                             email = user.email,
                             mobile = user.mobile
                         });
            var user_result = query.ToList().Select(r => new User
            {
                C_id = r.C_id,
                username = r.username,
                fullName = r.fullName,
                dayOff = r.dayOff,
                createdAt = r.createdAt,
                contractId = r.ContractType.name,
                ContractType = r.ContractType,
                email = r.email,
                mobile = r.mobile
            }).ToList()[0];

            var contract_query = (from contract in db.ContractTypes
                         select new
                         {
                             C_id = contract.C_id,
                             code = contract.code,
                             name = contract.name,
                             dayOff = contract.dayOff
                         });
            var contracts = contract_query.ToList().Select(c => new ContractType
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
                dayOff = c.dayOff
            }).ToList();
            ViewData["contracts"] = contracts; 
            return View(user_result);
        }

        public ActionResult Profile(string id)
        {
            var result = new QueryResult<User>();
            NghiphepEntities db = new NghiphepEntities();
            // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
            // Use Entity Framework, a module from your IoC Container, or any other method.
            // Return QueryResult object containing IEnumerable<YouModelItem>
            var query = (from user in db.Users
                         join contract in db.ContractTypes
                         on user.contractId equals contract.C_id
                         where user.C_id == id
                         select new
                         {
                             C_id = user.C_id,
                             username = user.username,
                             dayOff = user.dayOff,
                             contractId = user.contractId,
                             createdAt = user.createdAt,
                             ContractType = contract,
                             fullName = user.fullName,
                             email = user.email,
                             mobile = user.mobile
                         });
            var user_result = query.ToList().Select(r => new User
            {
                C_id = r.C_id,
                username = r.username,
                fullName = r.fullName,
                dayOff = r.dayOff,
                createdAt = r.createdAt,
                contractId = r.ContractType.name,
                ContractType = r.ContractType,
                email = r.email,
                mobile = r.mobile
            }).ToList()[0];

            var contract_query = (from contract in db.ContractTypes
                                  select new
                                  {
                                      C_id = contract.C_id,
                                      code = contract.code,
                                      name = contract.name,
                                      dayOff = contract.dayOff
                                  });
            var contracts = contract_query.ToList().Select(c => new ContractType
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
                dayOff = c.dayOff
            }).ToList();
            ViewData["contracts"] = contracts;
            return View(user_result);
        }

        public ActionResult DeleteUser(string id)
        {
            NghiphepEntities db = new NghiphepEntities();
            var result = db.Users.SingleOrDefault(b => b.C_id == id);
            if (result != null)
            {
                result.isActive = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult SaveUser(User user)
        {
            NghiphepEntities db = new NghiphepEntities();
            var result = db.Users.SingleOrDefault(b => b.username == user.username);
            if (result != null)
            {
                result.username = user.username;
                result.fullName = user.fullName;
                result.email = user.email;
                result.mobile = user.mobile;
                //result.contractId = user.contractId;
                result.dayOff = user.dayOff;
                db.SaveChanges();
            }
            return RedirectToAction("Edit/" + result.C_id);
        }

        public ActionResult SaveUserProfile (User user)
        {
            NghiphepEntities db = new NghiphepEntities();
            var result = db.Users.SingleOrDefault(b => b.username == user.username);
            if (result != null)
            {
                result.username = user.username;
                result.fullName = user.fullName;
                result.email = user.email;
                result.mobile = user.mobile;
                //result.contractId = user.contractId;
                result.dayOff = user.dayOff;
                db.SaveChanges();
            }
            return RedirectToAction("Profile/" + result.C_id);
        }
    }
}
