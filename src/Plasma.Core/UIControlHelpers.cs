/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Plasma.Core
{
    public class Button
    {
        public static AspNetRequest Click(AspNetForm form, string controlName) 
        {
            form[controlName] = "Button";
            return form.GenerateFormPostRequest();
        }
    }

    public class LinkButton
    {
        public static AspNetRequest Click(AspNetForm form, string controlName)
        {
            form["__EVENTTARGET"] = controlName;
            return form.GenerateFormPostRequest();
        }
    }
}
