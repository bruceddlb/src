//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2017-06-11 15:18:04 by 群升科技
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
    
using System;
namespace QSDMS.Model 
{
    /// <summary>
    /// 数据表实体类：Base_LogEntity 
    /// </summary>
    [Serializable()]
    public class Base_LogEntity
    {    
		            
		/// <summary>
		/// varchar:日志主键
		/// </summary>	
                 
		public string LogId { get; set; }

                    
		/// <summary>
		/// int:分类Id 1-登陆2-访问3-操作4-异常
		/// </summary>	
                 
		public int? CategoryId { get; set; }

                    
		/// <summary>
		/// varchar:来源对象主键
		/// </summary>	
                 
		public string SourceObjectId { get; set; }

                    
		/// <summary>
		/// text:来源日志内容
		/// </summary>	
                 
		public string SourceContentJson { get; set; }

                    
		/// <summary>
		/// datetime:操作时间
		/// </summary>	
                 
		public DateTime? OperateTime { get; set; }

                    
		/// <summary>
		/// varchar:操作用户Id
		/// </summary>	
                 
		public string OperateUserId { get; set; }

                    
		/// <summary>
		/// varchar:操作用户
		/// </summary>	
                 
		public string OperateAccount { get; set; }

                    
		/// <summary>
		/// varchar:操作类型Id
		/// </summary>	
                 
		public string OperateTypeId { get; set; }

                    
		/// <summary>
		/// varchar:操作类型
		/// </summary>	
                 
		public string OperateType { get; set; }

                    
		/// <summary>
		/// varchar:系统功能主键
		/// </summary>	
                 
		public string ModuleId { get; set; }

                    
		/// <summary>
		/// varchar:系统功能
		/// </summary>	
                 
		public string Module { get; set; }

                    
		/// <summary>
		/// varchar:IP地址
		/// </summary>	
                 
		public string IPAddress { get; set; }

                    
		/// <summary>
		/// varchar:IP地址所在城市
		/// </summary>	
                 
		public string IPAddressName { get; set; }

                    
		/// <summary>
		/// varchar:主机
		/// </summary>	
                 
		public string Host { get; set; }

                    
		/// <summary>
		/// varchar:浏览器
		/// </summary>	
                 
		public string Browser { get; set; }

                    
		/// <summary>
		/// int:执行结果状态
		/// </summary>	
                 
		public int? ExecuteResult { get; set; }

                    
		/// <summary>
		/// text:执行结果信息
		/// </summary>	
                 
		public string ExecuteResultJson { get; set; }

                    
		/// <summary>
		/// varchar:备注
		/// </summary>	
                 
		public string Description { get; set; }

                    
		/// <summary>
		/// int:删除标记
		/// </summary>	
                 
		public int? DeleteMark { get; set; }

                    
		/// <summary>
		/// int:有效标志
		/// </summary>	
                 
		public int? EnabledMark { get; set; }

           
    }    
}
	