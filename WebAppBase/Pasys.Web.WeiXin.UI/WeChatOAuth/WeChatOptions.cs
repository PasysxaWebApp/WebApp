// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.WeChat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Configuration options for <see cref="WeChatMiddleware"/>.
    /// </summary>
    public class WeChatOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="WeChatOptions"/>.
        /// </summary>
        public WeChatOptions()
        {
            AuthenticationScheme = WeChatDefaults.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            CallbackPath = new PathString("/signin-wechat");
            StateAddition = "#wechat_redirect";
            AuthorizationEndpoint = WeChatDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeChatDefaults.TokenEndpoint;
            UserInformationEndpoint = WeChatDefaults.UserInformationEndpoint;
            //SaveTokens = true;           

            //BaseScope （不弹出授权页面，直接跳转，只能获取用户openid），
            //InfoScope （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
            WeChatScope = InfoScope;
        }

        // WeChat uses a non-standard term for this field.
        /// <summary>
        /// Gets or sets the WeChat-assigned appId.
        /// </summary>
        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        // WeChat uses a non-standard term for this field.
        /// <summary>
        /// Gets or sets the WeChat-assigned app secret.
        /// </summary>
        public string AppSecret
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }

        public string StateAddition { get; set; }
        public string WeChatScope { get; set; }

        public string BaseScope = "snsapi_base";

        public string InfoScope = "snsapi_userinfo";
    }
}

WeChatOptions.cs