
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System;
using QSDMS.Model;
using QSDMS.Data.IService;
using QSDMS.Util.Extension;
using QSDMS.Util;
using QSDMS.Util.WebControl;
using iFramework.Framework.Security;
using iFramework.Framework;

namespace QSDMS.Data.Service.SqlServer
{
    /// <summary>   
    /// 描 述：用户管理
    /// </summary>
    public class UserService : IUserService
    {

        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetList()
        {
            var sql = PetaPoco.Sql.Builder.Append(@"select * from Base_User where 1=1 and UserId<>'System' and EnabledMark=1 and DeleteMark=0");
            sql.Append(" order by CreateDate desc");
            var list = Base_User.Query(sql);
            return EntityConvertTools.CopyToList<Base_User, UserEntity>(list.ToList());
        }

        /// <summary>
        /// 获取部门下面用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetDepartmentUserList()
        {
            var sql = PetaPoco.Sql.Builder.Append(@"SELECT  u.* FROM    Base_User u
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1
     AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0");
            var list = Base_User.Query(sql);
            return EntityConvertTools.CopyToList<Base_User, UserEntity>(list.ToList());
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var sql = PetaPoco.Sql.Builder.Append(@"select * from Base_User where 1=1 and UserId<>'System' and EnabledMark=1 and DeleteMark=0");

            var queryParam = queryJson.ToJObject();
            //公司主键
            if (!queryParam["organizeId"].IsEmpty())
            {
                string organizeId = queryParam["organizeId"].ToString();
                sql.Append(" and OrganizeId=@0", organizeId);
            }
            //部门主键
            if (!queryParam["departmentId"].IsEmpty())
            {
                string departmentId = queryParam["departmentId"].ToString();
                sql.Append(" and DepartmentId=@0", departmentId);

            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Account":            //账户
                        sql.Append("and (charindex(@0,Account)>0)", keyord);
                        break;
                    case "RealName":          //姓名
                        sql.Append("and (charindex(@0,RealName)>0)", keyord);
                        break;
                    case "Mobile":          //手机
                        sql.Append("and (charindex(@0,Mobile)>0)", keyord);
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(pagination.sidx))
            {
                sql.OrderBy(new object[] { pagination.sidx + " " + pagination.sord });
            }
            var currentpage = Base_User.Page(pagination.page, pagination.rows, sql);
            //数据对象
            var pageList = currentpage.Items;
            //分页对象

            pagination.records = Converter.ParseInt32(currentpage.TotalItems);
            return EntityConvertTools.CopyToList<Base_User, UserEntity>(pageList.ToList());
        }

        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntity(string keyValue)
        {
            var user = Base_User.SingleOrDefault("where UserId=@0", keyValue);
            return EntityConvertTools.CopyToModel<Base_User, UserEntity>(user, null);
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public UserEntity CheckLogin(string username)
        {
            var user = Base_User.SingleOrDefault("where (Account=@0 or Mobile=@0 or Email=@0)", username);
            return EntityConvertTools.CopyToModel<Base_User, UserEntity>(user, null);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistAccount(string account, string keyValue)
        {
            var sql = PetaPoco.Sql.Builder.Append(@"select * from Base_User where 1=1");
            if (!string.IsNullOrEmpty(account))
            {
                sql.Append(" and Account=@0", account);
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                sql.Append(" and UserId!=@0", keyValue);
            }
            return Base_User.Query(sql).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            Base_User.Delete("where UserId=@0", keyValue);
        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, UserEntity userEntity)
        {

            try
            {
                using (var tran = QSDMS_SQLDB.GetInstance().GetTransaction())
                {
                    #region 基本信息
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        userEntity.Modify(keyValue);
                        userEntity.Password = null;
                        Base_User model = Base_User.SingleOrDefault("where UserId=@0", keyValue);
                        model = EntityConvertTools.CopyToModel<UserEntity, Base_User>(userEntity, model);
                        model.UserId = keyValue;
                        model.Update();
                    }
                    else
                    {
                        userEntity.Create();
                        keyValue = userEntity.UserId;
                        userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                        userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                        userEntity.EnabledMark = 1;
                        userEntity.DeleteMark = 0;
                        Base_User model = EntityConvertTools.CopyToModel<UserEntity, Base_User>(userEntity, null);
                        model.Insert();

                    }
                    #endregion

                    #region 默认添加 角色、岗位、职位
                    Base_UserRelation.Delete("where UserId=@0 and IsDefault=1", userEntity.UserId);
                    List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                    //角色
                    if (!string.IsNullOrEmpty(userEntity.RoleId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = (int)QSDMS.Model.Enums.UserCategoryEnum.角色,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = userEntity.UserId,
                            ObjectId = userEntity.RoleId,
                            CreateDate = DateTime.Now,
                            CreateUserId = OperatorProvider.Provider.Current().UserId,
                            CreateUserName = OperatorProvider.Provider.Current().UserName,
                            IsDefault = 1,
                        });
                    }
                    //岗位
                    if (!string.IsNullOrEmpty(userEntity.DutyId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = (int)QSDMS.Model.Enums.UserCategoryEnum.岗位,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = userEntity.UserId,
                            ObjectId = userEntity.DutyId,
                            CreateDate = DateTime.Now,
                            CreateUserId = OperatorProvider.Provider.Current().UserId,
                            CreateUserName = OperatorProvider.Provider.Current().UserName,
                            IsDefault = 1,
                        });
                    }
                    //职位
                    if (!string.IsNullOrEmpty(userEntity.PostId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = (int)QSDMS.Model.Enums.UserCategoryEnum.职位,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = userEntity.UserId,
                            ObjectId = userEntity.PostId,
                            CreateDate = DateTime.Now,
                            CreateUserId = OperatorProvider.Provider.Current().UserId,
                            CreateUserName = OperatorProvider.Provider.Current().UserName,
                            IsDefault = 1,
                        });
                    }
                    //插入用户关系表
                    foreach (UserRelationEntity userRelationItem in userRelationEntitys)
                    {
                        Base_UserRelation model = EntityConvertTools.CopyToModel<UserRelationEntity, Base_UserRelation>(userRelationItem, null);
                        model.Insert();
                    }
                    #endregion

                    tran.Complete();
                }
                return keyValue;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码（MD5 小写）</param>
        public void RevisePassword(string keyValue, string Password)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.UserId = keyValue;
            userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
            userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Password, userEntity.Secretkey).ToLower(), 32).ToLower();
            Base_User user = Base_User.SingleOrDefault("where UserId=@0", keyValue);
            user = EntityConvertTools.CopyToModel<UserEntity, Base_User>(userEntity, user);
            user.UserId = keyValue;
            user.Update();
        }
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateState(string keyValue, int State)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.Modify(keyValue);
            userEntity.EnabledMark = State;
            Base_User model = Base_User.SingleOrDefault("where userId=@0", keyValue);
            model.Update();

        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userEntity">实体对象</param>
        public void UpdateEntity(UserEntity userEntity)
        {
            Base_User moudle = Base_User.SingleOrDefault("where UserId=@0", userEntity.UserId);
            moudle = EntityConvertTools.CopyToModel<UserEntity, Base_User>(userEntity, moudle);
            moudle.Update();
        }
        #endregion
    }
}
