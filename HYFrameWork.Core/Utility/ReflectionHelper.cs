using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：反射相关
    /// </summary>
    public static class ReflectionHelper
    {
        #region 字段、属性

        /// <summary>
        /// 复制对象属性
        /// </summary>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <typeparam name="TOut">引用输出类型</typeparam>
        /// <param name="inEntity">输入实体</param>
        /// <param name="outEntity">输出实体</param>
        /// <param name="ignoreNull">为空不赋值,默认：True</param>
        public static void CopyProperties<TIn, TOut>(TIn inEntity, ref TOut outEntity, bool ignoreNull = false) where TOut : class, new()
        {
            if (outEntity == null)
                outEntity = new TOut();

            if (inEntity == null)
                return;

            PropertyInfo[] OutPropertyInfos = typeof(TOut).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] InPropertyInfos = typeof(TIn).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pIn in InPropertyInfos)
            {
                foreach (var pOut in OutPropertyInfos)
                {
                    if (string.Compare(pOut.Name, pIn.Name, false) == 0)
                    {
                        if (!pOut.CanWrite) break;

                        if (ignoreNull && object.Equals(pIn.GetValue(inEntity, null), null)) break;

                        pOut.SetValue(outEntity, pIn.GetValue(inEntity, null), null);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 反射获取某个类型下的字段
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="flags">指定控制绑定以及通过反射执行成员和类型搜索的方式的标记。</param>
        /// <returns>字段数组</returns>
        public static FieldInfo[] GetFields<T>(T t, BindingFlags flags)
        {
            System.Type Type = t.GetType();
            FieldInfo[] fields = Type.GetFields(flags);
            return fields;
        }
        /// <summary>
        /// 获取某个类型下属性特性数据字典
        /// </summary>
        /// <typeparam name="T">要获取特性的类</typeparam>
        /// <typeparam name="T2">要获取的特性类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>结果字典</returns>
        public static Dictionary<string, List<T2>> GetPublicAttributes<T, T2>(T t = default(T))
            where T2 : Attribute
            where T : class
        {
            List<T2> list;
            Dictionary<string, List<T2>> dic = new Dictionary<string, List<T2>>();
            var vs = GetPropertys(t);
            foreach (var v in vs)
            {
                list = new List<T2>();
                object[] obj = v.GetCustomAttributes(typeof(T2), false);
                for (int j = 0; j < obj.Length; j++)
                {
                    T2 a = (T2)obj[j];
                    list.Add(a);
                }
                if (list.Count > 0)
                    dic.Add(v.Name, list);
            }
            return dic;
        }
        /// <summary>
        /// 将某个对象的公共属性和值序转换字典
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>属性字典</returns>
        public static Dictionary<string, string> ToDictionary<T>(T t) where T : new()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string value = string.Empty;
            System.Type type = t.GetType();
            PropertyInfo[] pros = type.GetProperties();
            foreach (PropertyInfo f in pros)
            {
                dic.Add(f.Name, f.GetValue(t, null).ToString());
            }
            return dic;
        }

        #endregion

        #region 对象/方法
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类名</param>
        /// <returns>实例</returns>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className) where T : new()
        {
            try
            {
                string fullName = nameSpace + "." + className;
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);
                return (T)ect;
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="fullName">命名空间.类名</param>
        /// <returns>实例</returns>
        public static T CreateInstance<T>(string assemblyName, string fullName) where T : new()
        {
            try
            {
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);
                return (T)ect;
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 创建对象实例（构造函数有参数）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="fullName">命名空间名称.类名</param>
        /// <param name="args">构造函数参数集</param>
        /// <param name="ignoreCase">是否忽略空值</param>
        /// <param name="bindingAttr">指定控制绑定以及通过反射执行成员和类型搜索的方式的标记。</param>
        /// <param name="binder">Binder对象</param>
        /// <param name="culture">CultureInfo对象</param>
        /// <returns>实例</returns>
        public static T CreateInstance<T>(
            string assemblyName,
            string fullName,
            object[] args,
            bool ignoreCase=false, 
            BindingFlags bindingAttr=BindingFlags.CreateInstance, 
            Binder binder=null, 
            CultureInfo culture=null)
        {
            try
            {
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName, ignoreCase, bindingAttr, binder, args,culture,null);
                return (T)ect;
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 调用指定dll对象实例中的有参无返回值方法
        /// </summary>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <param name="methodName">调用方法名</param>
        /// <param name="mType">调用方法的参数类型</param>
        /// <param name="paras">调用方法的参数实例</param>
        public static void UsingInstanceMethod(string assemblyName, string nameSpace, string className, string methodName, System.Type[] mType, object[] paras)
        {

            try
            {
                string fullName = nameSpace + "." + className;
                System.Type type = Assembly.Load(assemblyName).GetType(fullName);
                MethodInfo method = type.GetMethod(methodName, mType);
                object obj = Activator.CreateInstance(type);
                method.Invoke(obj, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  获取实体对象的成员值
        /// </summary>
        /// <param name="member">成员</param>
        /// <param name="instance">实体对象</param>
        /// <returns>对象值</returns>
        public static object GetValue(this MemberInfo member, object instance)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).GetValue(instance, null);
                case MemberTypes.Field:
                    return ((FieldInfo)member).GetValue(instance);
                default:
                    throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// 设置实体对象的成员值
        /// </summary>
        /// <param name="member">成员</param>
        /// <param name="instance">实体</param>
        /// <param name="value">值</param>
        public static void SetValue(this MemberInfo member, object instance, object value)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    var pi = (PropertyInfo)member;
                    pi.SetValue(instance, value, null);
                    break;
                case MemberTypes.Field:
                    var fi = (FieldInfo)member;
                    fi.SetValue(instance, value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region 数据表转换
        /// <summary>
        /// 将某个字典转换为对象的公共属性和值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dic">属性字典</param>
        /// <param name="t">对象</param>
        public static void ToObject<T>(Dictionary<string, string> dic, T t) where T :class, new()
        {
            var value = string.Empty;
            var pros = t.GetType().GetProperties();
            foreach (PropertyInfo f in pros)
            {
                if (dic.ContainsKey(f.Name))
                {
                    f.SetValue(t, Convert.ChangeType(dic[f.Name], f.PropertyType), null);
                }
            }
        }
        /// <summary>
        /// 把数据表通过反射转换成指定对象列表，并赋值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns>对象列表</returns>
        public static IEnumerable<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var list = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                var pros = GetPropertys<T>();
                foreach (DataRow r in dt.Rows)
                {
                    T t2 = typeof(T).Assembly.CreateInstance(typeof(T).FullName) as T;
                    pros.ForEach(p =>
                    {
                        if (r[p.Name] != null && r[p.Name] != DBNull.Value && !p.PropertyType.IsEnum)
                        {
                            p.SetValue(t2, Convert.ChangeType(r[p.Name], p.PropertyType), null);
                        }
                        else if (p.PropertyType.IsEnum)
                        {
                            p.SetValue(t2, System.Enum.Parse(p.PropertyType, r[p.Name].ToString()), null);
                        }
                        list.Add(t2);
                    });
                }
            }
            return list;
        }
        /// <summary>
        /// 将指定对象列表转化为表格
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="list">对象列表</param>
        /// <returns>数据表</returns>
        public static DataTable ToTable<T>(this IEnumerable<T> list) where T : class, new()
        {
            var index = 0;
            DataTable dt = null;
            if (list != null)
            {
                foreach (var t in list)
                {
                    if (index == 0)
                    {
                        dt = ConvertToTableWithoutValue(t);
                    }
                    dt.Rows.Add(GetPropertysValue(t));
                    index++;
                }
            }
            return dt;
        }
        /// <summary>
        /// 将一个对象转换成为DataTable，但不包含对象值（仅有列名）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">名称</param>
        /// <returns></returns>
        public static DataTable ConvertToTableWithoutValue<T>(T t)
        {
            var dt = new DataTable();
            var pros = GetPropertys(t);
            pros.ForEach(p =>
            {
                dt.Columns.Add(p.Name);
            });
            return dt;
        }
        /// <summary>
        ///  获取将某个对象的公共属性转换成object数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">实体</param>
        /// <returns>属性数组</returns>
        public static object[] GetPropertysValue<T>(T t)
        {
            var index = 0;
            var pros = GetPropertys(t);
            var arr = new object[pros.Length];
            pros.ForEach(p =>
            {
                arr[index] = p.GetValue(t) ?? "";
                index++;
            });
            return arr;
        }

        
        /// <summary>
        /// 获取对象某个公共属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="propertyname">属性名</param>
        /// <param name="t">实体</param>
        /// <returns>属性</returns>
        public static PropertyInfo GetProperty<T>(string propertyname,T t = default(T))
        {
            System.Type type = null;
            PropertyInfo property = null;
            if (t != null)
            {
                type = t.GetType();
            }
            else
            {
                type = typeof(T);
            }
            property = type.GetProperty(propertyname);
            return property;
        }
        /// <summary>
        /// 获取某个对象下的所有指定类型属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">实体</param>
        /// <param name="flags">指定控制绑定以及通过反射执行成员和类型搜索的方式的标记。</param>
        /// <returns>属性集合</returns>
        public static PropertyInfo[] GetPropertys<T>(T t = default(T), BindingFlags flags= BindingFlags.Instance | BindingFlags.Public)
        {
            System.Type type = null;
            PropertyInfo[] propertys = null;
            if (t!=null)
            {
                type = t.GetType();
            }
            else
            {
                type = typeof(T);
            }
            propertys = type.GetProperties(flags);
            return propertys;
        }
        #endregion

    }
}
