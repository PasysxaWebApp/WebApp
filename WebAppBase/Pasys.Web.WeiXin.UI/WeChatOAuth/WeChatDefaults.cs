// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Authentication.WeChat
{
    public static class WeChatDefaults
    {
        public const string AuthenticationScheme = "WeChat";

        public static readonly string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";

        public static readonly string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";

        public static readonly string UserInformationEndpoint = "https://api.weixin.qq.com/sns/userinfo";
    }
}
 