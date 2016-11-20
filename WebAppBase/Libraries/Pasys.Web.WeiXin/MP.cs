using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    /// <summary>
    /// 微信公众号
    /// </summary>
    public class MP
    {
		#region Model
		private string _wxname;
		private string _wxid;
		private string _yixinid;
		private string _weixincode;
		private string _wxpwd;
		private string _headerpic;
		private string _apiurl;
		private string _wxtoken;
		private string _wxprovince;
		private string _wxcity;
		private string _appid;
		private string _appsecret;
		private string _access_token;
		private string _openidstr;
		private DateTime? _createdate= DateTime.Now;
		private DateTime? _enddate;
		private int? _wenzimaxnum=0;
		private int? _tuwenmaxnum=0;
		private int? _yuyinmaxnum=0;
		private int? _wenzidefinenum=0;
		private int? _tuwendefinenum=0;
		private int? _yuyindefinenum=0;
		private int? _requestttnum=1000;
		private int? _requestusednum=0;
		private int? _smsttnum=0;
		private int? _smsusednum=0;
		private bool _isdelete;
		private DateTime? _deletedate;
		private int? _wxtype;
		private string _remark;
		private int? _seq;

        /// <summary>
        /// 
        /// </summary>
        public int MPId { get; set; }
        /// <summary>
        /// 组织Id
        /// </summary>
        public string OrganizationId { get; set; }
		/// <summary>
		/// 公众帐号名称
		/// </summary>
		public string wxName
		{
			set{ _wxname=value;}
			get{return _wxname;}
		}
		/// <summary>
		/// 公众号原始id
		/// </summary>
		public string wxId
		{
			set{ _wxid=value;}
			get{return _wxid;}
		}
		/// <summary>
		/// 易信原始id
		/// </summary>
		public string yixinId
		{
			set{ _yixinid=value;}
			get{return _yixinid;}
		}
		/// <summary>
		/// 微信号
		/// </summary>
		public string weixinCode
		{
			set{ _weixincode=value;}
			get{return _weixincode;}
		}
		/// <summary>
		/// 微信公众平台密码
		/// </summary>
		public string wxPwd
		{
			set{ _wxpwd=value;}
			get{return _wxpwd;}
		}
		/// <summary>
		/// 头像地址（url）
		/// </summary>
		public string Avatar
		{
			set{ _headerpic=value;}
			get{return _headerpic;}
		}
		/// <summary>
		/// 接口地址
		/// </summary>
		public string apiurl
		{
			set{ _apiurl=value;}
			get{return _apiurl;}
		}
		/// <summary>
		/// TOKEN值
		/// </summary>
		public string wxToken
		{
			set{ _wxtoken=value;}
			get{return _wxtoken;}
		}
		/// <summary>
		/// 省份
		/// </summary>
		public string wxProvince
		{
			set{ _wxprovince=value;}
			get{return _wxprovince;}
		}
		/// <summary>
		/// 城市
		/// </summary>
		public string wxCity
		{
			set{ _wxcity=value;}
			get{return _wxcity;}
		}
		/// <summary>
		/// 服务号的AppId
		/// </summary>
		public string AppId
		{
			set{ _appid=value;}
			get{return _appid;}
		}
		/// <summary>
		/// 服务号的AppSecret
		/// </summary>
		public string AppSecret
		{
			set{ _appsecret=value;}
			get{return _appsecret;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Access_Token
		{
			set{ _access_token=value;}
			get{return _access_token;}
		}
		/// <summary>
		/// 关注用户openid字符串
		/// </summary>
		public string openIdStr
		{
			set{ _openidstr=value;}
			get{return _openidstr;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? createDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 到期时间
		/// </summary>
		public DateTime? endDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
		}
		/// <summary>
		/// 文本上限
		/// </summary>
		public int? wenziMaxNum
		{
			set{ _wenzimaxnum=value;}
			get{return _wenzimaxnum;}
		}
		/// <summary>
		/// 图文上限
		/// </summary>
		public int? tuwenMaxNum
		{
			set{ _tuwenmaxnum=value;}
			get{return _tuwenmaxnum;}
		}
		/// <summary>
		/// 语音上限
		/// </summary>
		public int? yuyinMaxNum
		{
			set{ _yuyinmaxnum=value;}
			get{return _yuyinmaxnum;}
		}
		/// <summary>
		/// 文本定义条数
		/// </summary>
		public int? wenziDefineNum
		{
			set{ _wenzidefinenum=value;}
			get{return _wenzidefinenum;}
		}
		/// <summary>
		/// 图文定义条数
		/// </summary>
		public int? tuwenDefineNum
		{
			set{ _tuwendefinenum=value;}
			get{return _tuwendefinenum;}
		}
		/// <summary>
		/// 语音定义条数
		/// </summary>
		public int? yuyinDefineNum
		{
			set{ _yuyindefinenum=value;}
			get{return _yuyindefinenum;}
		}
		/// <summary>
		/// 总请求数
		/// </summary>
		public int? requestTTNum
		{
			set{ _requestttnum=value;}
			get{return _requestttnum;}
		}
		/// <summary>
		/// 已经使用的请求数
		/// </summary>
		public int? requestUsedNum
		{
			set{ _requestusednum=value;}
			get{return _requestusednum;}
		}
		/// <summary>
		/// 短信总条数
		/// </summary>
		public int? smsTTNum
		{
			set{ _smsttnum=value;}
			get{return _smsttnum;}
		}
		/// <summary>
		/// 已经使用的短信条数
		/// </summary>
		public int? smsUsedNum
		{
			set{ _smsusednum=value;}
			get{return _smsusednum;}
		}
		/// <summary>
		/// 删除
		/// </summary>
		public bool DeleteFlag
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 删除时间
		/// </summary>
		public DateTime? deleteDate
		{
			set{ _deletedate=value;}
			get{return _deletedate;}
		}
		/// <summary>
		/// 微信公众帐号类型（订阅号，服务号）
		/// </summary>
		public int? wxType
		{
			set{ _wxtype=value;}
			get{return _wxtype;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string Note
		{
			set{ _remark=value;}
			get{return _remark;}
		}
	
		#endregion Model
    }
}
