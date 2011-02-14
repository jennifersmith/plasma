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
using Plasma.Core;

namespace Plasma.WebDriver
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
