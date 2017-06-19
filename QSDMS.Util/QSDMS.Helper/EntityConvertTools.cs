using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QSDMS.Util
{
    /// <summary>
    /// 实体转换类,适应将不同名称属性相同的实体进行转换操作
    /// </summary>
    public class EntityConvertTools
    {
        /// <summary>
        /// 将一个实体对象转换为另一个实体对象,如果第转换的值为空则取被转换的实体之前对象
        /// </summary>
        /// <typeparam name="T1">from 第一个实体对象</typeparam>
        /// <typeparam name="T2">to 第二个实体对象</typeparam>
        /// <param name="source">转换的实体对象</param>
        /// <param name="source2">被转换的实体对象</param>
        /// <returns>新的实体数据对象</returns>
        public static T2 CopyToModel<T1, T2>(T1 source, T2 source2)
        {
            if (source == null)
            {
                return default(T2);
            }
            T2 model = default(T2);
            PropertyInfo[] pi = typeof(T2).GetProperties();
            PropertyInfo[] pi1 = typeof(T1).GetProperties();

            model = Activator.CreateInstance<T2>();
            for (int i = 0; i < pi.Length; i++)
            {
                for (int j = 0; j < pi1.Length; j++)
                {
                    if (pi[i].Name == pi1[j].Name)
                    {
                        var value = pi1[j].GetValue(source, null);
                        if (value == null && source2 != null)
                        {
                            value = pi[j].GetValue(source2, null);
                        }
                        pi[i].SetValue(model, value, null);
                        break;
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 将一个实体集合转换为另一个实体集合
        /// </summary>
        /// <typeparam name="T1">form 实体对象</typeparam>
        /// <typeparam name="T2">to 实体对象</typeparam>
        /// <param name="source">转换集合源</param>
        /// <returns></returns>
        public static List<T2> CopyToList<T1, T2>(List<T1> source)
        {
            if (source == null)
            {
                return null;
            }
            List<T2> list = new List<T2>();
            if (source != null)
            {               
                foreach (T1 item in source)
                {
                    T2 model = default(T2);
                    PropertyInfo[] pi = typeof(T2).GetProperties();
                    PropertyInfo[] pi1 = typeof(T1).GetProperties();
                    model = Activator.CreateInstance<T2>();
                    for (int i = 0; i < pi.Length; i++)
                    {
                        for (int j = 0; j < pi1.Length; j++)
                        {
                            if (pi[i].Name == pi1[j].Name)
                            {
                                pi[i].SetValue(model, pi1[j].GetValue(item, null), null);
                                break;
                            }
                        }
                    }                   
                    list.Add(model);
                }                
            }
            return list;
        }
    }
}
