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
    /// 数据表实体类：Base_ModuleFormEntity 
    /// </summary>
    [Serializable()]
    public class Base_ModuleFormEntity
    {    
		            
		/// <summary>
		/// varchar:表单主键
		/// </summary>	
                 
		public string FormId { get; set; }

                    
		/// <summary>
		/// varchar:功能主键
		/// </summary>	
                 
		public string ModuleId { get; set; }

                    
		/// <summary>
		/// varchar:编码
		/// </summary>	
                 
		public string EnCode { get; set; }

                    
		/// <summary>
		/// varchar:名称
		/// </summary>	
                 
		public string FullName { get; set; }

                    
		/// <summary>
		/// varchar:表单控件Json
		/// </summary>	
                 
		public string FormJson { get; set; }

                    
		/// <summary>
		/// int:排序码
		/// </summary>	
                 
		public int? SortCode { get; set; }

                    
		/// <summary>
		/// int:删除标记
		/// </summary>	
                 
		public int? DeleteMark { get; set; }

                    
		/// <summary>
		/// int:有效标志
		/// </summary>	
                 
		public int? EnabledMark { get; set; }

                    
		/// <summary>
		/// varchar:备注
		/// </summary>	
                 
		public string Description { get; set; }

                    
		/// <summary>
		/// datetime:创建日期
		/// </summary>	
                 
		public DateTime? CreateDate { get; set; }

                    
		/// <summary>
		/// varchar:创建用户主键
		/// </summary>	
                 
		public string CreateUserId { get; set; }

                    
		/// <summary>
		/// varchar:创建用户
		/// </summary>	
                 
		public string CreateUserName { get; set; }

                    
		/// <summary>
		/// datetime:修改日期
		/// </summary>	
                 
		public DateTime? ModifyDate { get; set; }

                    
		/// <summary>
		/// varchar:修改用户主键
		/// </summary>	
                 
		public string ModifyUserId { get; set; }

                    
		/// <summary>
		/// varchar:修改用户
		/// </summary>	
                 
		public string ModifyUserName { get; set; }

           
    }    
}
	