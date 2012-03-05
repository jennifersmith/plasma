/* **********************************************************************************
 *
 * Copyright 2010 ThoughtWorks, Inc.  
 * ThoughtWorks provides the software "as is" without warranty of any kind, either express or implied, including but not limited to, 
 * the implied warranties of merchantability, satisfactory quality, non-infringement and fitness for a particular purpose.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).  
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plasma.Sample.Web.Mvc.Controllers
{
    public class CookiesController : Controller
    {
        //
        // GET: /Cookies/

        public ActionResult Show()
        {
            return View();
        }

        public ActionResult Set()
        {
            return View();
        }

        public ActionResult Expire()
        {
            return View();
        }

        public ActionResult SetMany()
        {
            return View();
        }

    }
}
