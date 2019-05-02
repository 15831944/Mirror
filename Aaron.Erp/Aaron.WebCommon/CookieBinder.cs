﻿using Aaron.Common;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aaron.WebCommon
{
    public class CookieBinder : DefaultModelBinder, IModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            //从cookie中获取jwt字符串
            var jwtStr = CookieHelper.GetCookie("Authorization");
            var baseModel = model as BaseModel;
            if (baseModel != null)
            {
                return baseModel;
            }
            if (!string.IsNullOrEmpty(jwtStr))
            {
                //解密赋值给BaseModel
                var dicJson = JwtHelper.SerializeJWT(jwtStr);
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(dicJson);
                //根据jwtHelper分装的格式解封
                var userInfoJson = dic["userInfo"].ToString();

                baseModel = JsonConvert.DeserializeObject<BaseModel>(userInfoJson);
                return baseModel;
            }
            return null;
        }
    }
}